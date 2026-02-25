using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Core.Combat;

public sealed class TurnQueue
{
    private readonly List<Combatant> _order;
    private int _cursor = -1;

    public TurnQueue(IEnumerable<Combatant> combatants)
    {
        _order = combatants.OrderByDescending(x => x.Initiative).ToList();
        if (_order.Count == 0)
        {
            throw new ArgumentException("At least one combatant is required.", nameof(combatants));
        }
    }

    public Combatant Next()
    {
        if (_order.All(x => !x.IsAlive))
        {
            return _order[0];
        }

        while (true)
        {
            _cursor++;
            if (_cursor >= _order.Count)
            {
                _cursor = 0;
                _order.Sort((a, b) => b.Initiative.CompareTo(a.Initiative));
            }

            var candidate = _order[_cursor];
            if (candidate.IsAlive)
            {
                return candidate;
            }
        }
    }
}
