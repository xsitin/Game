using System;
using System.Collections.Generic;

namespace GameCore.Model
{
    public abstract class BasicCreature
    {
        //TODO move some methods from heroes there and add effect from characters
        protected static readonly Dictionary<Characteristics, int> BaseCharacteristics =
            new()
            {
                {Model.Characteristics.Health, 100},
                {Model.Characteristics.Evasion, 10},
                {Model.Characteristics.Initiative, 30},
                {Model.Characteristics.Mana, 100},
                {Model.Characteristics.MagicalProtection, 10},
                {Model.Characteristics.PhysicalDamage, 15},
                {Model.Characteristics.PhysicalProtection, 20}
            };

        public readonly List<Buff> Buffs = new();
        public List<ActiveItem>? Inventory;

        public string? Name { get; set; }

        public Dictionary<Characteristics, int> Characteristics { get; set; }

        public int Level { get; set; }

        public Location Location { get; set; }

        public Specialization Specialization { get; set; }

        public List<Skill>? Skills { get; set; }

        public Position Position { get; set; }

        public BasicCreature(string name, Dictionary<Characteristics, int> characteristics, List<ActiveItem> inventory,
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

        public void HpChange(int change, bool isMagic)
        {
            if (isMagic)
            {
                Characteristics[Model.Characteristics.Health] += (int) (
                    change * (1 - (double) Characteristics[Model.Characteristics.MagicalProtection] / 100));
            }
            else if (new Random().Next(0, 100) > Characteristics[Model.Characteristics.Evasion])
            {
                Characteristics[Model.Characteristics.Health] +=
                    (int) (change * (1 - (double) Characteristics[Model.Characteristics.PhysicalProtection] / 100));
            }
        }

        public void UseSkill(Skill action, params BasicCreature[] targets)
        {
            if (action.ManaCost > Characteristics[Model.Characteristics.Mana]) return;
            Characteristics[Model.Characteristics.Mana] -= action.ManaCost;
            foreach (var target in targets)
            {
                foreach (var (characteristic, value) in action.Effect)
                    if (characteristic == Model.Characteristics.Health)
                        if (action.Name == "BaseHit")
                            target.HpChange(Characteristics[Model.Characteristics.PhysicalDamage], false);
                        else
                            target.HpChange(value, action.IsMagic);
                    else if (target.Characteristics != null) target.Characteristics[characteristic] += value;

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