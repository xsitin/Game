using System;

namespace Game.Model
{
    public class Buff
    {
        public (Characteristics characteristic, int value)[] _buffs;
        private BasicCreature _target;

        public Buff(int duration, string name, params (Characteristics characteristic, int value)[] buffs)
        {
            Duration = duration;
            Name = name;
            _buffs = buffs;
        }

        public Buff(BasicCreature target, int duration, string name,
            params (Characteristics characteristic, int value)[] buffs)
        {
            Duration = duration;
            Name = name;
            _buffs = buffs;
            Target = target;
        }

        public string Name { get; }

        public BasicCreature Target
        {
            get => _target;
            set
            {
                _target = value ?? throw new ArgumentNullException(nameof(value));
                foreach (var (characteristic, val) in _buffs) Target.Characteristics[characteristic] += val;
            }
        }

        public int Duration { get; set; }

        ~Buff()
        {
            if (_target != null)
                foreach (var (characteristic, value) in _buffs)
                    _target.Characteristics[characteristic] -= value;
        }

        public Buff ToTarget(BasicCreature target)
        {
            var buff = new Buff(Duration, Name, _buffs);
            buff._target = target;
            foreach (var (characteristic, value) in _buffs) buff._target.Characteristics[characteristic] += value;
            return buff;
        }
    }
}