using System.Collections.Generic;
using System.Linq;

namespace Game.Core.Combat;

public sealed class Combatant
{
    private readonly Dictionary<Characteristic, int> _stats;
    private readonly List<BattleBuff> _buffs = [];

    public Combatant(
        string name,
        int maxHealth,
        int damage,
        int healPower,
        bool isEnemy,
        int physicalProtection,
        int magicalProtection,
        int evasion,
        int mana = 100,
        int initiative = 30,
        IReadOnlyList<BattleSkill>? skills = null)
    {
        Name = name;
        IsEnemy = isEnemy;
        HealPower = healPower;

        _stats = new Dictionary<Characteristic, int>
        {
            [Characteristic.Health] = maxHealth,
            [Characteristic.Mana] = mana,
            [Characteristic.Initiative] = initiative,
            [Characteristic.PhysicalDamage] = damage,
            [Characteristic.PhysicalProtection] = NormalizePercent(physicalProtection),
            [Characteristic.MagicalProtection] = NormalizePercent(magicalProtection),
            [Characteristic.Evasion] = NormalizePercent(evasion)
        };

        Skills = (skills ?? SkillCatalog.BaseHit).Select(CloneSkill).ToList();
        Health = maxHealth;
    }

    public string Name { get; }
    public bool IsEnemy { get; }
    public int MaxHealth => BaseValue(Characteristic.Health);
    public int Health { get; private set; }
    public int Mana => Value(Characteristic.Mana);
    public int Initiative => Value(Characteristic.Initiative);
    public int Damage => Value(Characteristic.PhysicalDamage);
    public int PhysicalProtection => NormalizePercent(Value(Characteristic.PhysicalProtection));
    public int MagicalProtection => NormalizePercent(Value(Characteristic.MagicalProtection));
    public int Evasion => NormalizePercent(Value(Characteristic.Evasion));
    public int HealPower { get; }
    public bool IsAlive => Health > 0;
    public IReadOnlyList<BattleBuff> Buffs => _buffs;
    public IReadOnlyList<BattleSkill> Skills { get; }

    public int ReceivePhysicalDamage(int rawDamage, int evadeRoll)
    {
        if (!IsAlive || evadeRoll < Evasion)
        {
            return 0;
        }

        var appliedDamage = ApplyProtection(rawDamage, PhysicalProtection);
        Health = int.Max(0, Health - appliedDamage);
        return appliedDamage;
    }

    public int ReceiveMagicDamage(int rawDamage)
    {
        if (!IsAlive)
        {
            return 0;
        }

        var appliedDamage = ApplyProtection(rawDamage, MagicalProtection);
        Health = int.Max(0, Health - appliedDamage);
        return appliedDamage;
    }

    public int ReceiveHeal(int rawHeal)
    {
        if (!IsAlive)
        {
            return 0;
        }

        var appliedHeal = rawHeal < 1 ? 1 : rawHeal;
        var before = Health;
        Health = int.Min(MaxHealth, Health + appliedHeal);
        return Health - before;
    }

    public bool TrySpendMana(int manaCost)
    {
        if (manaCost > Mana)
        {
            return false;
        }

        _stats[Characteristic.Mana] -= manaCost;
        return true;
    }

    public int ApplyEffect(BattleEffect effect, bool isMagic, int evadeRoll)
    {
        if (effect.Characteristic == Characteristic.Health)
        {
            if (effect.Value < 0)
            {
                var damage = -effect.Value;
                return isMagic ? ReceiveMagicDamage(damage) : ReceivePhysicalDamage(damage, evadeRoll);
            }

            return ReceiveHeal(effect.Value);
        }

        _stats[effect.Characteristic] = Value(effect.Characteristic) + effect.Value;
        return effect.Value;
    }

    public void AddBuff(BattleBuff buff)
    {
        var applied = buff.Clone();
        _buffs.Add(applied);
        foreach (var effect in applied.Effects)
        {
            if (effect.Characteristic == Characteristic.Health)
            {
                continue;
            }

            _stats[effect.Characteristic] = Value(effect.Characteristic) + effect.Value;
        }
    }

    public IEnumerable<string> TickBuffs()
    {
        var expired = new List<BattleBuff>();
        foreach (var buff in _buffs)
        {
            if (buff.Tick())
            {
                expired.Add(buff);
            }
        }

        foreach (var buff in expired)
        {
            foreach (var effect in buff.Effects)
            {
                if (effect.Characteristic == Characteristic.Health)
                {
                    continue;
                }

                _stats[effect.Characteristic] = Value(effect.Characteristic) - effect.Value;
            }

            _buffs.Remove(buff);
            yield return $"{Name}: buff '{buff.Name}' expired.";
        }
    }

    public string ToStatusLine() =>
        $"{Name}: {Health}/{MaxHealth} HP | Mana {Mana} | Init {Initiative} | PProt {PhysicalProtection}% | MProt {MagicalProtection}% | Eva {Evasion}%";

    private int Value(Characteristic key) => _stats.TryGetValue(key, out var value) ? value : 0;

    private int BaseValue(Characteristic key) => _stats[key];

    private static int NormalizePercent(int value) => int.Clamp(value, 0, 95);

    private static int ApplyProtection(int rawDamage, int protection)
    {
        var baseDamage = rawDamage < 1 ? 1 : rawDamage;
        var reduced = (int)System.Math.Round(baseDamage * (1 - (double)protection / 100));
        return reduced < 1 ? 1 : reduced;
    }

    private static BattleSkill CloneSkill(BattleSkill skill) =>
        new(
            skill.Name,
            skill.ManaCost,
            skill.Range,
            skill.Effects.ToArray(),
            skill.IsMagic,
            skill.Buff is null ? null : new BattleBuff(skill.Buff.Name, skill.Buff.Duration, skill.Buff.Effects.ToArray()));
}
