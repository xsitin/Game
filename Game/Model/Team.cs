using System.Collections.Generic;

namespace Game.Model
{
    public class Team<T> where T: BasicCreature
    {
        public List<T> FirstLine { get; }
        public List<T> SecondLine { get; }

        public Team(List<T> firstLine, List<T> secondLine)
        {
            FirstLine = firstLine;
            SecondLine = secondLine;
        }
    }
}