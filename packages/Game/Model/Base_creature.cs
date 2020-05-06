using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Game.Model
{
    public abstract class BasicCreature
    {
        public string Name { get; }
        public readonly Dictionary<Characteristics, int> Characteristics;
        public Inventory Inventory { get; set; }
        public List<Buff> Buffs = new List<Buff>();

        public BasicCreature(string name, Dictionary<Characteristics, int> characteristics, Inventory inventory)
        {
            Name = name;
            Characteristics = characteristics;
            Inventory = inventory;
        }
    }
}