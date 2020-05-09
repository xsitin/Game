﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Game.Model
{
    public class Hero : BasicCreature
    {
        private int _exp;
        public List<Skill> Skills;

        public Hero(string name, Dictionary<Characteristics, int> characteristics, Inventory inventory,
            Specialization specialization, Position position, Location location) : base(name, characteristics,
            inventory)
        {
            Specialization = specialization;
            Position = position;
            Location = location;
            Level = 1;
            Exp = 0;
            StandardChars = new ReadOnlyDictionary<Characteristics, int>
                (characteristics.ToDictionary(x => x.Key, y => y.Value));
        }

        public int UpgradePoints { get; private set; }

        public Specialization Specialization { get; }

        public int Exp
        {
            get => _exp;
            set
            {
                if (value < 0)
                    throw new Exception("ты шо совсем тупой?");
                _exp = value;
                while (_exp >= Math.Round(100 + Level * 100 + 300 * Math.Pow(Level, 0.5)))
                {
                    _exp -= (int) Math.Round(100 + Level * 100 + 300 * Math.Pow(Level, 0.5));
                    UpgradePoints++;
                    Level++;
                }
            }
        }

        public int Level { get; set; }
        public Position Position { get; }
        public Location Location { get; }
        public ReadOnlyDictionary<Characteristics, int> StandardChars { get; }

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
        
        public override string ToString() {
            return Name;
        }

        public override bool Equals(object obj) {
            if (!(obj is Hero hero)) return false;
            return this.Exp == hero.Exp && this.Name == hero.Name;
        }

        public override int GetHashCode() {
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