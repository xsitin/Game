using System.Collections.Generic;

namespace GameCore.Model
{
    public class EnemyHero : BasicCreature
    {
        public EnemyHero(string name, Dictionary<Characteristics, int> characteristics, List<ActiveItem> inventory,
            Specialization specialization, Position position, Location location) : base(name, characteristics,
            inventory, specialization, location)
        {
            Position = position;
        }

        public EnemyHero(string name, Dictionary<Characteristics, int> characteristics, List<ActiveItem> inventory,
            Specialization specialization, Location location) : base(name, characteristics, inventory, specialization,
            location)
        {
            Position = Helper.Transfer[specialization];
        }
    }
}