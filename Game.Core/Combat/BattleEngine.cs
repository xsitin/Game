using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Core.Combat;

public sealed class BattleEngine
{
    private readonly List<Combatant> _heroes;
    private readonly List<Combatant> _enemies;
    private readonly Random _random;
    private readonly TurnQueue _queue;

    public BattleEngine() : this(BattleSetup.Prototype)
    {
    }

    public BattleEngine(BattleSetup setup)
    {
        if (setup.Heroes.Count == 0)
        {
            throw new ArgumentException("At least one hero is required.", nameof(setup));
        }

        if (setup.Enemies.Count == 0)
        {
            throw new ArgumentException("At least one enemy is required.", nameof(setup));
        }

        _heroes = setup.Heroes.Select(x => x.ToCombatant()).ToList();
        _enemies = setup.Enemies.Select(x => x.ToCombatant()).ToList();
        _random = new Random(setup.RandomSeed);
        _queue = new TurnQueue(_heroes.Concat(_enemies));
        ActiveCombatant = _queue.Next();
        SkipDead();
    }

    public bool IsBattleFinished => Winner is not null;

    public string? Winner
    {
        get
        {
            if (_heroes.All(x => !x.IsAlive)) return "Enemies";
            if (_enemies.All(x => !x.IsAlive)) return "Heroes";
            return null;
        }
    }

    public Combatant ActiveCombatant { get; private set; }
    public Combatant ActiveHero => ActiveCombatant;
    public IReadOnlyList<Combatant> Heroes => _heroes;
    public IReadOnlyList<Combatant> Enemies => _enemies;

    public IReadOnlyList<BattleSkill> GetActiveHeroSkills() =>
        ActiveCombatant.IsEnemy ? [] : ActiveCombatant.Skills;

    public IReadOnlyList<Combatant> GetTargetsForActiveHeroSkill(int skillIndex)
    {
        if (ActiveCombatant.IsEnemy)
        {
            return [];
        }

        var skill = SafeGetSkill(ActiveCombatant, skillIndex);
        if (skill is null)
        {
            return [];
        }

        return GetTargetPool(ActiveCombatant, skill.Range, _heroes, _enemies, skill)
            .Where(x => x.IsAlive)
            .ToList();
    }

    public IReadOnlyList<string> ApplyHeroSkill(int skillIndex, int targetIndex = 0)
    {
        if (IsBattleFinished)
        {
            return ["Battle is already finished."];
        }

        if (ActiveCombatant.IsEnemy)
        {
            return ["It is not the hero turn."];
        }

        var skill = SafeGetSkill(ActiveCombatant, skillIndex);
        if (skill is null)
        {
            return ["Selected skill is unavailable."];
        }

        var log = new List<string>();
        ExecuteSkill(ActiveCombatant, skill, _heroes, _enemies, log, targetIndex);

        CompleteTurnAndAdvance(log);

        while (!IsBattleFinished && ActiveCombatant.IsEnemy)
        {
            ApplyEnemyTurn(log);
            CompleteTurnAndAdvance(log);
        }

        if (IsBattleFinished)
        {
            log.Add($"Battle finished. Winner: {Winner}.");
        }

        return log;
    }

    public IReadOnlyList<string> ApplyHeroAction(BattleAction action)
    {
        return action switch
        {
            BattleAction.Attack => ApplyHeroSkill(IndexOfSkill(ChooseHeroAttackSkill(ActiveCombatant))),
            BattleAction.Heal => ApplyHeroSkill(IndexOfSkill(ChooseHeroHealSkill(ActiveCombatant))),
            BattleAction.Skip => ApplyHeroSkip(),
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
        };
    }

    public string BuildStatusText()
    {
        var heroes = string.Join("\n", _heroes.Select(x => $"[color=lightgreen]{x.ToStatusLine()}[/color]"));
        var enemies = string.Join("\n", _enemies.Select(x => $"[color=salmon]{x.ToStatusLine()}[/color]"));
        return $"[b]Heroes[/b]\n{heroes}\n\n[b]Enemies[/b]\n{enemies}";
    }

    private IReadOnlyList<string> ApplyHeroSkip()
    {
        if (IsBattleFinished)
        {
            return ["Battle is already finished."];
        }

        if (ActiveCombatant.IsEnemy)
        {
            return ["It is not the hero turn."];
        }

        var log = new List<string> {$"{ActiveCombatant.Name} skips the turn."};

        CompleteTurnAndAdvance(log);

        while (!IsBattleFinished && ActiveCombatant.IsEnemy)
        {
            ApplyEnemyTurn(log);
            CompleteTurnAndAdvance(log);
        }

        if (IsBattleFinished)
        {
            log.Add($"Battle finished. Winner: {Winner}.");
        }

        return log;
    }

    private void ApplyEnemyTurn(List<string> log)
    {
        var enemy = ActiveCombatant;
        if (!enemy.IsAlive)
        {
            return;
        }

        var skill = ChooseEnemySkill(enemy);
        ExecuteSkill(enemy, skill, _enemies, _heroes, log);
    }

    private void ExecuteSkill(
        Combatant actor,
        BattleSkill skill,
        IReadOnlyList<Combatant> allies,
        IReadOnlyList<Combatant> opponents,
        List<string> log,
        int selectedTargetIndex = 0)
    {
        if (!actor.TrySpendMana(skill.ManaCost))
        {
            log.Add($"{actor.Name} tries to use {skill.Name}, but lacks mana.");
            if (skill.Name != "Base Hit")
            {
                ExecuteSkill(actor, BaseHitFor(actor), allies, opponents, log, selectedTargetIndex);
            }

            return;
        }

        var targets = SelectTargets(actor, skill.Range, allies, opponents, skill, selectedTargetIndex);
        if (targets.Count == 0)
        {
            log.Add($"{actor.Name} uses {skill.Name}, but there are no valid targets.");
            return;
        }

        foreach (var target in targets)
        {
            foreach (var effect in skill.Effects)
            {
                var result = target.ApplyEffect(effect, skill.IsMagic, _random.Next(0, 100));
                if (effect.Characteristic == Characteristic.Health)
                {
                    if (effect.Value < 0)
                    {
                        log.Add(result == 0
                            ? $"{actor.Name} uses {skill.Name} on {target.Name}, but misses."
                            : $"{actor.Name} uses {skill.Name} on {target.Name} for {result}.");
                    }
                    else
                    {
                        log.Add($"{actor.Name} uses {skill.Name} on {target.Name}, healing {result}.");
                    }
                }
                else
                {
                    var sign = effect.Value >= 0 ? "+" : string.Empty;
                    log.Add($"{actor.Name} changes {target.Name} {effect.Characteristic} by {sign}{effect.Value}.");
                }

                if (!target.IsAlive)
                {
                    log.Add($"{target.Name} is defeated.");
                }
            }

            if (skill.Buff is not null && target.IsAlive)
            {
                target.AddBuff(skill.Buff);
                log.Add($"{actor.Name} applies {skill.Buff.Name} to {target.Name}.");
            }
        }
    }

    private static BattleSkill ChooseHeroAttackSkill(Combatant hero) =>
        hero.Skills.FirstOrDefault(x => x.Range is SkillRange.Single or SkillRange.Enemies or SkillRange.All && x.Effects.Any(e => e.Characteristic == Characteristic.Health && e.Value < 0))
        ?? BaseHitFor(hero);

    private static BattleSkill ChooseHeroHealSkill(Combatant hero) =>
        hero.Skills.FirstOrDefault(x => x.Effects.Any(e => e.Characteristic == Characteristic.Health && e.Value > 0))
        ?? new BattleSkill("Field Aid", 0, SkillRange.Friendly, [new BattleEffect(Characteristic.Health, hero.HealPower)]);

    private BattleSkill ChooseEnemySkill(Combatant enemy)
    {
        var available = enemy.Skills.Where(x => x.ManaCost <= enemy.Mana).ToList();
        if (available.Count == 0)
        {
            return BaseHitFor(enemy);
        }

        var preferHeal = enemy.Health <= enemy.MaxHealth / 2;
        if (preferHeal)
        {
            var heal = available.FirstOrDefault(x => x.Effects.Any(e => e.Characteristic == Characteristic.Health && e.Value > 0));
            if (heal is not null)
            {
                return heal;
            }
        }

        var offensive = available.Where(x => x.Effects.Any(e => e.Characteristic == Characteristic.Health && e.Value < 0)).ToList();
        return offensive.Count > 0 ? offensive[_random.Next(offensive.Count)] : available[_random.Next(available.Count)];
    }

    private static List<Combatant> SelectTargets(
        Combatant actor,
        SkillRange range,
        IReadOnlyList<Combatant> allies,
        IReadOnlyList<Combatant> opponents,
        BattleSkill skill,
        int selectedTargetIndex)
    {
        var pool = GetTargetPool(actor, range, allies, opponents, skill)
            .Where(x => x.IsAlive)
            .ToList();

        if (range == SkillRange.Single)
        {
            if (pool.Count == 0)
            {
                return [];
            }

            var index = int.Clamp(selectedTargetIndex, 0, pool.Count - 1);
            return [pool[index]];
        }

        return pool;
    }

    private static IEnumerable<Combatant> GetTargetPool(
        Combatant actor,
        SkillRange range,
        IReadOnlyList<Combatant> allies,
        IReadOnlyList<Combatant> opponents,
        BattleSkill skill)
    {
        if (range == SkillRange.Single)
        {
            var isHarm = skill.Effects.Any(x => x.Characteristic == Characteristic.Health && x.Value < 0);
            return isHarm ? opponents : allies;
        }

        return range switch
        {
            SkillRange.All => allies.Concat(opponents),
            SkillRange.Enemies => opponents,
            SkillRange.Friendly => allies,
            _ => []
        };
    }

    private static BattleSkill BaseHitFor(Combatant actor) =>
        new("Base Hit", 0, SkillRange.Single, [new BattleEffect(Characteristic.Health, -actor.Damage)]);

    private int IndexOfSkill(BattleSkill skill)
    {
        var idx = ActiveCombatant.Skills.ToList().FindIndex(x => x.Name == skill.Name);
        return idx < 0 ? 0 : idx;
    }

    private static BattleSkill? SafeGetSkill(Combatant actor, int skillIndex)
    {
        if (skillIndex < 0 || skillIndex >= actor.Skills.Count)
        {
            return null;
        }

        return actor.Skills[skillIndex];
    }

    private void CompleteTurnAndAdvance(List<string> log)
    {
        foreach (var buffMessage in ActiveCombatant.TickBuffs())
        {
            log.Add(buffMessage);
        }

        ActiveCombatant = _queue.Next();
        SkipDead();
    }

    private void SkipDead()
    {
        while (!IsBattleFinished && !ActiveCombatant.IsAlive)
        {
            ActiveCombatant = _queue.Next();
        }
    }
}
