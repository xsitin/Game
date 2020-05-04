using System.Collections.Generic;

namespace Game.Model
{
    //TODO
    public class Hero : BasicCreature
    {
        public Specialization Specialization { get; }
        public Position Position { get; }
         
        
        public Hero(string name, Dictionary<Characteristics, int> characteristics, Inventory inventory, Specialization specialization, Position position) : base(name, characteristics, inventory)
        {
            Specialization = specialization;
            Position = position;
        }
    }

    public enum Specialization
    {
        Wizard,
        Warrior, 
        Archer
    }

    public enum Position
    {
        Melee,
        Range
    }
    
}