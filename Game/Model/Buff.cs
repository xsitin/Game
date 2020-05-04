using System;

namespace Game.Model
{
    public class Buff
    {
        //TODO
        public BasicCreature Target { get; }
        private (Characteristics characteristic , int value)[] buffs;
        public int Duration { get; set; }

        public Buff(BasicCreature target, int duration, params (Characteristics characteristic, int value)[] buffs)
        {
            Target = target;
            Duration = duration;
            this.buffs = buffs;
            foreach (var (characteristic, value) in buffs) target.Characteristics[characteristic] += value;
        }

        ~Buff()
        {
            foreach (var (characteristic, value) in buffs) Target.Characteristics[characteristic] -= value;
        }
    }
}