using System.Collections.Generic;

namespace Game.Model
{
    public class Hero : BasicCreature
    {
        public Specialization Specialization { get; }
        public Position Position { get; }
        public List<Skill> Skills;
        public Location Location;
        
        public Hero(string name, Dictionary<Characteristics, int> characteristics, Inventory inventory, Specialization specialization, Position position, Location location) : base(name, characteristics, inventory)
        {
            Specialization = specialization;
            Position = position;
            Location = location;
        }

        public void UseSkill(Skill action, params BasicCreature[] targets )
        {
            if(action.ManaCost <= this.Characteristics[Model.Characteristics.Mana])
                foreach (var target in targets)
                foreach (var (characteristic, value) in action.Effect)
                    target.Characteristics[characteristic] += value;
        }
    }

    public enum Specialization
    {
        Wizard,
        Warrior, 
        Archer
    }

    public enum Location
    {
        //TODO
    }
}