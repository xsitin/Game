using System.Collections.Generic;
using System.Linq;

namespace Game.Core.Combat;

public sealed class BattleBuff
{
    public BattleBuff(string name, int duration, IReadOnlyList<BattleEffect> effects)
    {
        Name = name;
        Duration = duration;
        Effects = effects;
    }

    public string Name { get; }
    public int Duration { get; private set; }
    public IReadOnlyList<BattleEffect> Effects { get; }

    public BattleBuff Clone() => new(Name, Duration, Effects.ToArray());

    public bool Tick()
    {
        Duration--;
        return Duration <= 0;
    }
}
