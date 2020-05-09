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

    public class ActiveItem : Item
    {
        public ActiveItem(string name, Action<BasicCreature> action) : base(name)
        {
            if (action is null)
                throw new ArgumentException("Action shouldn't be null!");
            Action = action;
        }

        public ActiveItem()
        {
        }

        public Action<BasicCreature> Action { get; set; }

        public void Use(params BasicCreature[] targets)
        {
            foreach (var target in targets)
                Action(target);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ActiveItem item)) return false;
            return Name == item.Name && Action == item.Action;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}