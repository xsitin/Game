using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore.Model
{
    public class Team<T> where T : BasicCreature
    {
        public Team(List<T> firstLine, List<T> secondLine)
        {
            if (firstLine.Count == 0 && secondLine.Count == 0)
                throw new ArgumentException("Lines shouldn't be empty!");
            if (firstLine == secondLine)
                throw new ArgumentException("Lines shouldn't be equals!");
            FirstLine = firstLine;
            SecondLine = secondLine;
            MakeStepForward();
        }

        public Team()
        {
            FirstLine = new List<T>();
            SecondLine = new List<T>();
        }

        public List<T> FirstLine { get; protected set; }
        public List<T> SecondLine { get; protected set; }

        public List<BasicCreature> GetTeamList()
        {
            return FirstLine.Concat(SecondLine).Cast<BasicCreature>().ToList();
        }

        public void MakeStepForward()
        {
            if (FirstLine.Count != 0 || SecondLine.Count == 0) return;
            FirstLine = SecondLine;
            SecondLine = new List<T>();
        }

        public void Update()
        {
            FirstLine = FirstLine.Where(x => x.Characteristics[Characteristics.Health] > 0)
                .ToList();
            SecondLine = SecondLine
                .Where(x => x.Characteristics[Characteristics.Health] > 0).ToList();
            MakeStepForward();
        }
    }
}