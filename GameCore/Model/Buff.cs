namespace GameCore.Model
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
        public BasicCreature? target;

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
            get => target;
            init
            {
                target = value;
                if (target == null || Buffs == null) return;
                foreach (var (characteristic, val) in Buffs)
                    if (Target?.Characteristics != null)
                        Target.Characteristics[characteristic] += val;
            }
        }

        public int Duration { get; set; }

        ~Buff()
        {
            if (target == null || Buffs == null) return;
            foreach (var (characteristic, value) in Buffs)
                if (target.Characteristics != null)
                    target.Characteristics[characteristic] -= value;
        }

        public Buff ToTarget(BasicCreature target)
        {
            var buff = new Buff(Duration, Name, Buffs);
            buff.target = target;
            if (Buffs == null) return buff;
            foreach (var (characteristic, value) in Buffs)
                if (buff.target.Characteristics != null)
                    buff.target.Characteristics[characteristic] += value;
            return buff;
        }
    }
}