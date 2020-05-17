using System;
using System.Collections.Generic;

namespace Game.Model
{
    public abstract class BasicCreature
    {
        public string Name { get; set; }
        public  Dictionary<Characteristics, int> Characteristics { get; set; }
        public Inventory Inventory;
        public int Level { get; set; }
        public Location Location { get; set; }
        public Specialization Specialization { get; set; }
        public List<Skill> Skills { get; set; }
        public Position Position { get; set; }

        public BasicCreature(string name, Dictionary<Characteristics, int> characteristics, Inventory inventory,
            Specialization specialization, Location location)
        {
            Name = name;
            Characteristics = characteristics;
            if (Characteristics.Count != 7)
                FillDictionary();
            Inventory = inventory;
            Specialization = specialization;
            Level = 1;
            Location = location;
            Skills = new List<Skill>();
            Skills.Add(new Skill(0,
                new[] {(Model.Characteristics.Health, -Characteristics[Model.Characteristics.PhysicalDamage])},
                SkillRange.Single, "Base Hit", null));
        }

        public BasicCreature()
        {
        }
        
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
            Level = 1;
            if (Characteristics.Count != 7)
                FillDictionary();
            Inventory = inventory;
        }

        public void HpChange(int change, bool isMagic)
        {
            if (isMagic)
                Characteristics[Model.Characteristics.Health] +=
                    change * (1 - Characteristics[Model.Characteristics.MagicalProtection] / 100);
            else if ((new Random()).Next(0, 100) > Characteristics[Model.Characteristics.Evasion])
                Characteristics[Model.Characteristics.Health] +=
                    change * (1 - Characteristics[Model.Characteristics.PhysicalProtection] / 100);
        }
        
        public void UseSkill(Skill action, params BasicCreature[] targets)
        {
            if (action.ManaCost <= Characteristics[Model.Characteristics.Mana])
                foreach (var target in targets)
                {
                    foreach ((var characteristic, var value) in action.Effect)
                        if(characteristic==Model.Characteristics.Health)
                            target.HpChange(value,action.IsMagic);
                        else
                            target.Characteristics[characteristic] += value;
                    if (action.Buff != null)
                        target.Buffs.Add(action.Buff.ToTarget(target));
                }
        }
        
        private void FillDictionary()
        {
            for (var i = 0; i < 7; i++)
            {
                var ability = (Characteristics) i;
                if (!Characteristics.ContainsKey(ability))
                    Characteristics[ability] = BaseCharacteristics[ability];
            }
        }
        public override string ToString()
        {
            return Name;
        }
    }
}