namespace Game.Model
{
    public class Skill
    {
        public string Name { get; }
        public int ManaCost { get; }
        public SkillRange Range;
        public (Characteristics characteristic , int value)[] Effect { get; }

        public Buff Buff { get; }
        public Skill(int manaCost, (Characteristics characteristic, int value)[] effect, SkillRange range, string name, Buff buff)
        {
            ManaCost = manaCost;
            Effect = effect;
            Range = range;
            Name = name;
            Buff = buff;
        }
    }

    public enum SkillRange
    {
        All,
        Friendly,
        Enemies, 
        Single
    }
}