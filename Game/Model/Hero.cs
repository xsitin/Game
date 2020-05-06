using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Model
{
    public class Hero : BasicCreature
    {
        public Specialization Specialization { get; }

        public int Exp
        {
            get => Exp;
            set
            {
                if (value<0)
                    throw  new Exception("ты шо совсем тупой?");
                Exp += value;
                while (Exp>Math.Round((100+Level*100+300*Math.Pow(Level,0.5))))
                {
                    Exp -=(int) Math.Round((100 + Level * 100 + 300 * Math.Pow(Level, 0.5)));
                    Level++;
                }
            }
        }

        public int Level { get; set; }
        public Position Position { get; }
        public List<Skill> Skills;
        public Location Location;
        public Dictionary<Characteristics, int> Characteristics;
        public Dictionary<Characteristics, int> StandartChars;
        public Hero(string name, Dictionary<Characteristics, int> characteristics, Inventory inventory, Specialization specialization, Position position, Location location) : base(name, characteristics, inventory)
        {
            Specialization = specialization;
            Position = position;
            Location = location;
            Level = 1;
            Characteristics = characteristics;
            StandartChars = characteristics.ToDictionary(x=>x.Key,y=>y.Value);
        }

        public void UseSkill(Skill action, params BasicCreature[] targets )
        {
            if(action.ManaCost <= Characteristics[Model.Characteristics.Mana])
                foreach (var target in targets)
                {

                    foreach (var (characteristic, value) in action.Effect)
                        target.Characteristics[characteristic] += value;
                }
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