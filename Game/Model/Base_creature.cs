using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Game.Model
{
    public abstract class BasicCreature
    {
        //TODO move some methods from heroes there and add effect from characters
        public static readonly Dictionary<Characteristics, int> BaseCharacteristics =
            new Dictionary<Characteristics, int>
            {
                {Model.Characteristics.Health, 100},
                {Model.Characteristics.Evasion, 10},
                {Model.Characteristics.Initiative, 30},
                {Model.Characteristics.Mana, 100},
                {Model.Characteristics.MagicalProtection, 10},
                {Model.Characteristics.PhysicalDamage, 15},
                {Model.Characteristics.PhysicalProtection, 20}
            };

        public readonly List<Buff> Buffs = new List<Buff>();

        public BasicCreature(string name, Dictionary<Characteristics, int> characteristics, Inventory inventory)
        {
            Name = name;
            Characteristics = characteristics;
            if (Characteristics.Count != 7)
                FillDictionary();
            Inventory = inventory;
        }

        public BasicCreature()
        {
        }

        public string Name { get; set; }
        public Dictionary<Characteristics, int> Characteristics { get; }
        public Inventory Inventory { get; set; }

        private void FillDictionary()
        {
            for (var i = 0; i < 7; i++)
            {
                var ability = (Characteristics) i;
                if (!Characteristics.ContainsKey(ability))
                    Characteristics[ability] = BaseCharacteristics[ability];
            }
        }
    }
}