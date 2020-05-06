namespace Game.Model
{
    public class Skill
    {
        public string Name { get; }
        public int ManaCost { get; }
        public SkillRange Range;
        public (Characteristics characteristic , int value)[] Effect { get; }
        public Skill(int manaCost, (Characteristics characteristic, int value)[] effect, SkillRange range, string name)
        {
            ManaCost = manaCost;
            this.Effect = effect;
            Range = range;
            Name = name;
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