namespace Game.Model
{
    public class Buff
    {
        public Buff(int duration, string? name, params (Characteristics characteristic, int value)[]? buffs)
        {
            Duration = duration;
            Name = name;
            Buffs = buffs;
        }

        public (Characteristics characteristic, int value)[]? Buffs { get; set; }
        public BasicCreature? _target { get; set; }

/*
        public Buff()
        {
        }
*/

/*
        public Buff(BasicCreature target, int duration, string name,
            params (Characteristics characteristic, int value)[] buffs)
        {
            Duration = duration;
            Name = name;
            Buffs = buffs;
            Target = target;
        }
*/

        public string? Name { get; }

        public BasicCreature? Target
        {
            get => _target;
            init
            {
                _target = value;
                if (_target == null || Buffs == null) return;
                foreach (var (characteristic, val) in Buffs)
                    if (Target?.Characteristics != null)
                        Target.Characteristics[characteristic] += val;
            }
        }

        public int Duration { get; set; }

        ~Buff()
        {
            if (_target == null || Buffs == null) return;
            foreach (var (characteristic, value) in Buffs)
                if (_target.Characteristics != null)
                    _target.Characteristics[characteristic] -= value;
        }

        public Buff ToTarget(BasicCreature target)
        {
            var buff = new Buff(Duration, Name, Buffs);
            buff._target = target;
            if (Buffs != null)
                foreach (var (characteristic, value) in Buffs)
                    if (buff._target.Characteristics != null)
                        buff._target.Characteristics[characteristic] += value;
            return buff;
        }
    }
}