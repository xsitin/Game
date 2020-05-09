using System;

namespace Game.Model
{
    public abstract class Item
    {
        public string Name { get; }
        public Item(string name)
        {
            Name = name;
        }
    }

    public class ActiveItem : Item 
    {
        private readonly Action<BasicCreature> _action;

        public ActiveItem(string name, Action<BasicCreature> action) : base(name)
        {
            if (action is null)
                throw new ArgumentException("Action shouldn't be null!");
            _action = action;
        }

        public void Use(params BasicCreature[] targets)
        {
            foreach (var target in targets)
                _action(target);
        }
    }
}