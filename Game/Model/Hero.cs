using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Game.Model
{
    public class Hero : BasicCreature
    {
        private int _exp;
        public List<Skill> Skills = new List<Skill>();
        public Position Position { get; set; }
        public ReadOnlyDictionary<Characteristics, int> StandardChars { get; }

        public Hero(string name, Dictionary<Characteristics, int> characteristics, Inventory inventory,
            Specialization specialization, Position position, Location location) : base(name, characteristics,
            inventory, specialization, location)
        {
            Exp = 0;
            StandardChars = new ReadOnlyDictionary<Characteristics, int>
                (characteristics.ToDictionary(x => x.Key, y => y.Value));
            Position = position;
            Skills.Add(new Skill(0,
                new[] {(Model.Characteristics.Health, StandardChars[Model.Characteristics.PhysicalDamage])},
                SkillRange.Single, "Base Hit", null));
        }
        public Hero(string name, Dictionary<Characteristics, int> characteristics, Inventory inventory,
            Specialization specialization, Location location) : base(name, characteristics,
            inventory, specialization, location)
        {
            Exp = 0;
            StandardChars = new ReadOnlyDictionary<Characteristics, int>
                (characteristics.ToDictionary(x => x.Key, y => y.Value));
            Position = Helper.Transfer[specialization];
            Skills.Add(new Skill(0,
                new[] {(Model.Characteristics.Health, StandardChars[Model.Characteristics.PhysicalDamage])},
                SkillRange.Single, "Base Hit", null));
        }

        public Hero()
        {
        }

        public int UpgradePoints { get; private set; }
        

        public int Exp
        {
            get => _exp;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Changed exp should be more than 0");
                _exp = value;
                while (_exp >= Math.Round(100 + Level * 100 + 300 * Math.Pow(Level, 0.5)))
                {
                    _exp -= (int) Math.Round(100 + Level * 100 + 300 * Math.Pow(Level, 0.5));
                    UpgradePoints++;
                    Level++;
                }
            }
        }

        public void UseSkill(Skill action, params BasicCreature[] targets)
        {
            //logic for choosing goals will be in controls
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

        public override bool Equals(object obj)
        {
            if (!(obj is Hero)) return false;
            var hero = (Hero) obj;
            return Exp == hero.Exp && Name == hero.Name;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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
        SomeLocation
        //TODO
    }
}