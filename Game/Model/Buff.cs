using System;

namespace Game.Model
{
    public class Buff
    {
        public string Name { get; }
        public BasicCreature Target {
            get => Target;
            set
            {
                Target = value ?? throw new ArgumentNullException(nameof(value));
                foreach ((var characteristic, var val) in buffs) Target.Characteristics[characteristic] += val;
            }
        }
        private (Characteristics characteristic , int value)[] buffs;
        public int Duration { get; set; }

        public Buff(int duration, string name, params (Characteristics characteristic, int value)[] buffs)
        {
            Duration = duration;
            Name = name;
            this.buffs = buffs;
        }

        ~Buff()
        {
            foreach (var (characteristic, value) in buffs) Target.Characteristics[characteristic] -= value;
        }
    }
}