using System;

namespace Game.Model
{
    public abstract class Item
    {
        public Item(string name)
        {
            Name = name;
        }

        protected Item()
        {
        }

        public string Name { get; }
    }

    public class ActiveItem : Item {
        public (Characteristics characteristic, int value)[] Actions { get; private set; }
        public Buff Buff { get; }

        public ActiveItem(string name, (Characteristics characteristic, int value)[] actions, Buff buff = null) : base(name)
        {
            Actions = actions;
            Buff = buff;
        }

        public ActiveItem()
        {
        }

        public Action<BasicCreature> Action { get; set; }

        public void Use(params BasicCreature[] targets)
        {
            foreach (var target in targets) {
                foreach (var (characteristic, value) in Actions)
                    target.Characteristics[characteristic] += value;
                if (Buff != null)
                    target.Buffs.Add(Buff);
            }

        }

        public override bool Equals(object obj)
        {
            if (!(obj is ActiveItem item)) return false;
            return this.Name == item.Name && this.Actions == item.Actions;

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString() {
            return Name.ToString();
        }
    }
}