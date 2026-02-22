using System.Collections.Generic;
using System.Linq;

namespace Game.Core.Combat;

public sealed class BattleSkill
{
    public BattleSkill(
        string name,
        int manaCost,
        SkillRange range,
        IReadOnlyList<BattleEffect> effects,
        bool isMagic = false,
        BattleBuff? buff = null)
    {
        Name = name;
        ManaCost = manaCost;
        Range = range;
        Effects = effects.ToArray();
        IsMagic = isMagic;
        Buff = buff;
    }

    public string Name { get; }
    public int ManaCost { get; private set; }
    public SkillRange Range { get; }
    public IReadOnlyList<BattleEffect> Effects { get; private set; }
    public bool IsMagic { get; }
    public BattleBuff? Buff { get; private set; }
    public int Level { get; private set; } = 1;

    public void Upgrade()
    {
        Level++;
        ManaCost = (int)System.Math.Round(ManaCost * 1.2);
        Effects = Effects
            .Select(x => x with { Value = (int)System.Math.Round(x.Value * 1.2) })
            .ToArray();

        if (Buff is not null)
        {
            Buff = new BattleBuff(
                Buff.Name,
                Buff.Duration,
                Buff.Effects.Select(x => x with { Value = (int)System.Math.Round(x.Value * 1.2) }).ToArray());
        }
    }
}

public enum SkillRange
{
    All,
    Friendly,
    Enemies,
    Single
}
