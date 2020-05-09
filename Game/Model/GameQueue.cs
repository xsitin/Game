using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Model
{
    public class GameQueue
    {
        private int _count;
        public List<BasicCreature> Queue;
        public void GetQueue(List<BasicCreature> creatures)
        {
            Queue = creatures.OrderByDescending(x =>
                x.Characteristics[Characteristics.Initiative]).ToList();
        }

        public GameQueue(List<BasicCreature> creatures)
        {
            if(!creatures.Any()) throw new FormatException("Чел ты даун?");
            Queue = creatures.OrderByDescending(x =>
                x.Characteristics[Characteristics.Initiative]).ToList();
        }

        public void Update()
        {
            var count = Queue.Take(_count).Count(x =>
                ((x is null)) || x.Characteristics[Characteristics.Health] <= 0 || x.Characteristics[Characteristics.Initiative] <= 0);
            Queue = Queue.Where(x =>
                (!(x is null))&&x.Characteristics[Characteristics.Health] > 0).ToList();
            _count -= count;
        }

        public BasicCreature GetNextPerson()
        {
            Update();
            _count++;
            if (_count >= Queue.Count) _count = 0;
            return Queue[_count];
        }
    }
}