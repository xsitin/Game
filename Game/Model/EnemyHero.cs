using System.Collections.Generic;

namespace Game.Model
{
    public class EnemyHero : BasicCreature
    {
        public Location Location;
        public List<Skill> Skills = new List<Skill>();

        public EnemyHero(string name, Dictionary<Characteristics, int> characteristics, Inventory inventory,
            Specialization specialization, Position position, Location location) : base(name, characteristics,
            inventory)
        {
            Specialization = specialization;
            Position = position;
            Location = location;
        }
        
        public EnemyHero(string name, Dictionary<Characteristics, int> characteristics, Inventory inventory,
            Specialization specialization, Location location) : base(name, characteristics,
            inventory)
        {
            Specialization = specialization;
            Position = Transfer[specialization];
            Location = location;
        }

        public Specialization Specialization { get; }
        public Position Position { get; }

        public void UseSkill(Skill action, params BasicCreature[] targets)
        {
            if (action.ManaCost <= Characteristics[Model.Characteristics.Mana])
                foreach (var target in targets)
                {
                    foreach ((var characteristic, var value) in action.Effect)
                        target.Characteristics[characteristic] += value;
                    if (action.Buff != null)
                        target.Buffs.Add(action.Buff.ToTarget(target));
                }
        }
        
        public override string ToString()
        {
            return Name;
        }

        private static readonly Dictionary<Specialization, Position> Transfer = new Dictionary<Specialization, Position>()
        {
            {Specialization.Wizard, Position.Range},
            {Specialization.Warrior,Position.Melee},
            {Specialization.Archer, Position.Range}
        };
    }
    
}