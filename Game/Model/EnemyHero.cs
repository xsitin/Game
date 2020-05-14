using System.Collections.Generic;

namespace Game.Model
{
    public class EnemyHero : BasicCreature
    {

        public Position Position { get; }
        public List<Skill> Skills = new List<Skill>();

        public EnemyHero(string name, Dictionary<Characteristics, int> characteristics, Inventory inventory,
            Specialization specialization, Position position, Location location) : base(name, characteristics,
            inventory, specialization, location)
        {
            Position = position;
            Skills.Add(new Skill(0,
                new[] {(Model.Characteristics.Health, Characteristics[Model.Characteristics.PhysicalDamage])},
                SkillRange.Single, "Base Hit", null));
        }

        public EnemyHero(string name, Dictionary<Characteristics, int> characteristics, Inventory inventory,
            Specialization specialization, Location location) : base(name, characteristics, inventory, specialization, location)
        {
            Position = Helper.Transfer[specialization];
            Skills.Add(new Skill(0,
                new[] {(Model.Characteristics.Health, Characteristics[Model.Characteristics.PhysicalDamage])},
                SkillRange.Single, "Base Hit", null));
        }

        public void UseSkill(Skill action, params BasicCreature[] targets)
        {
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
    }
}