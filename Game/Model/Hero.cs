using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Game.Model
{
    public class Hero : BasicCreature
    {
        private int _exp;

        public ReadOnlyDictionary<Characteristics, int> StandardChars { get; set; }

        public Hero(string name, Dictionary<Characteristics, int> characteristics, List<ActiveItem> inventory,
            Specialization specialization, Position position, Location location) : base(name, characteristics,
            inventory, specialization, location)
        {
            Exp = 0;
            StandardChars = new ReadOnlyDictionary<Characteristics, int>
                (characteristics.ToDictionary(x => x.Key, y => y.Value));
            if (StandardChars == null || StandardChars.Count < 7)
                FillStandart();
            Position = position;
        }

        private void FillStandart()
        {
            if (StandardChars == null)
                StandardChars = new ReadOnlyDictionary<Characteristics, int>(Characteristics);
            var b = new Dictionary<Characteristics, int>(StandardChars);
            for (var i = 0; i < 7; i++)
                if (!b.ContainsKey((Characteristics) i))
                    b[(Characteristics) i] = BaseCharacteristics[(Characteristics) i];
            StandardChars = new ReadOnlyDictionary<Characteristics, int>(b);
        }

        public Hero(Specialization spec) : base(Helper.GetName(), new Dictionary<Characteristics, int>(),
            new List<ActiveItem>(), spec, Location.SomeLocation)
        {
            if (StandardChars == null || StandardChars.Count < 7)
                FillStandart();
        }

        public Hero(string name, Dictionary<Characteristics, int> characteristics, List<ActiveItem> inventory,
            Specialization specialization, Location location) : base(name, characteristics,
            inventory, specialization, location)
        {
            Exp = 0;
            StandardChars = new ReadOnlyDictionary<Characteristics, int>
                (characteristics.ToDictionary(x => x.Key, y => y.Value));
            if (StandardChars.Count < 7)
                FillStandart();
            Position = Helper.Transfer[specialization];
        }

        public Hero()
        {
        }

        public int UpgradePoints { get; set; }


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