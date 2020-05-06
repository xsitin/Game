using System.Collections.Generic;

namespace Game.Model
{
    public class EnemyHero : BasicCreature
    {
        public EnemySpecialization Specialization { get; }
        public Position Position { get; }
        public List<Skill> Skills;
        public Location Location;
        
        public EnemyHero(string name, Dictionary<Characteristics, int> characteristics, Inventory inventory, EnemySpecialization specialization, Position position, Location location) : base(name, characteristics, inventory)
        {
            Specialization = specialization;
            Position = position;
            Location = location;
        }

        public void UseSkill(Skill action, params BasicCreature[] targets )
        {
            if(action.ManaCost <= this.Characteristics[Model.Characteristics.Mana])
                foreach (var target in targets)
                {
                    foreach (var (characteristic, value) in action.Effect)
                        target.Characteristics[characteristic] += value;
                    if (action.Buff!=null)
                        target.Buffs.Add(action.Buff);
                }
        }
    }

    public enum EnemySpecialization
    {
        Wizard,
        Warrior, 
        Archer
    }
}
