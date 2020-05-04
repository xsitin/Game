namespace Game.Model
{
    public class Skill
    {
        public int ManaCost { get; }
        private (Characteristics characteristic , int value)[] effect;
        public Skill(int manaCost, (Characteristics characteristic, int value)[] effect)
        {
            ManaCost = manaCost;
            this.effect = effect;
        }
    }
}