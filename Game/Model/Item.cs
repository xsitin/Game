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

        public string Name { get; set; }
    }

    public class ActiveItem : Item
    {
        public ActiveItem(string name, (Characteristics characteristic, int value)[] actions, Buff buff = null) :
            base(name)
        {
            Actions = actions;
            Buff = buff;
        }

        public ActiveItem()
        {
        }

        public (Characteristics characteristic, int value)[] Actions { get; set; }
        public Buff Buff { get; }

        public Action<BasicCreature> Action { get; set; }

        public void Use(params BasicCreature[] targets)
        {
            foreach (var target in targets)
            {
                foreach ((var characteristic, var value) in Actions)
                    target.Characteristics[characteristic] += value;
                if (Buff != null)
                    target.Buffs.Add(Buff);
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ActiveItem item)) return false;
            bool equalActions = Actions.Length==item.Actions.Length;
            if (equalActions)
                for (var i = 0; i < Actions.Length; i++)
                    equalActions &= Actions[i] == item.Actions[i];
            return Name == item.Name && equalActions;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}