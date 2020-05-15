using System;

namespace Game.Model
{
    public class Buff
    {
        public (Characteristics characteristic, int value)[] Buffs { get; set; }
        public BasicCreature _target { get; set; }

        public Buff(int duration, string name, params (Characteristics characteristic, int value)[] buffs)
        {
            Duration = duration;
            Name = name;
            Buffs = buffs;
        }

        public Buff()
        {
            
        }

        public Buff(BasicCreature target, int duration, string name,
            params (Characteristics characteristic, int value)[] buffs)
        {
            Duration = duration;
            Name = name;
            Buffs = buffs;
            Target = target;
        }

        public string Name { get; set; }

        public BasicCreature Target
        {
            get => _target;
            set
            {
                _target = value;
                if(_target != null) foreach (var (characteristic, val) in Buffs) Target.Characteristics[characteristic] += val;
            }
        }

        public int Duration { get; set; }

        ~Buff()
        {
            if (_target != null)
                foreach (var (characteristic, value) in Buffs)
                    _target.Characteristics[characteristic] -= value;
        }

        public Buff ToTarget(BasicCreature target)
        {
            var buff = new Buff(Duration, Name, Buffs);
            buff._target = target;
            foreach (var (characteristic, value) in Buffs) buff._target.Characteristics[characteristic] += value;
            return buff;
        }
    }
}