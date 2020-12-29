using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Model
{
    public class GameQueue
    {
        private int _count;
        public List<BasicCreature> Queue;

        public GameQueue(List<BasicCreature> creatures)
        {
            _count = -1;
            if (!creatures.Any()) throw new FormatException("Чел ты даун?");
            Queue = creatures.OrderByDescending(x =>
                x.Characteristics[Characteristics.Initiative]).ToList();
        }

        public void Update()
        {
            var count = Queue.Take(_count).Count(x =>
                x is null || x.Characteristics[Characteristics.Health] <= 0 ||
                x.Characteristics[Characteristics.Initiative] <= 0);
            Queue = Queue.Where(x =>
                !(x is null) && x.Characteristics[Characteristics.Health] > 0).ToList();
            while (_count > -1 && _count < Queue.Count &&
                   Queue[_count].Characteristics[Characteristics.Initiative] <= 0) _count++;
        }

        public BasicCreature GetNextPerson()
        {
            _count++;
            Update();
            if (_count >= Queue.Count)
            {
                _count = 0;
                Queue = Queue.OrderByDescending(x => x.Characteristics[Characteristics.Initiative]).ToList();
            }

            Update();
            return Queue[_count];
        }
    }
}