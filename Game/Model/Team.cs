using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Model
{
    public class Team<T> where T: BasicCreature
    {
        public List<T> FirstLine { get; }
        public List<T> SecondLine { get; }

        public Team(List<T> firstLine, List<T> secondLine)
        {
            if (firstLine.Count == 0 && secondLine.Count == 0)
                throw new System.ArgumentException("Lines shouldn't be empty!");
            if (firstLine == secondLine)
                throw new System.ArgumentException("Lines shouldn't be equals!");
            FirstLine = firstLine;
            SecondLine = secondLine;
        }

        public List<BasicCreature> GetTeamList() => FirstLine.Concat(SecondLine).Cast<BasicCreature>().ToList();
    }
}