using System;

namespace Game.Model
{
    public class Buff
    {
        public string Name { get; }
        public BasicCreature Target { get; }
        private (Characteristics characteristic , int value)[] buffs;
        public int Duration { get; set; }

        public Buff(BasicCreature target, int duration, string name, params (Characteristics characteristic, int value)[] buffs)
        {
            Target = target;
            Duration = duration;
            Name = name;
            this.buffs = buffs;
            foreach (var (characteristic, value) in buffs) target.Characteristics[characteristic] += value;
        }

        ~Buff()
        {
            foreach (var (characteristic, value) in buffs) Target.Characteristics[characteristic] -= value;
        }
    }
}