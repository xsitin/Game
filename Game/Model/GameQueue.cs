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
            Queue = creatures.OrderBy(x =>
                x.Characteristics[Characteristics.Initiative]).ToList();
        }

        public GameQueue(List<BasicCreature> creatures)
        {
            Queue = creatures.OrderBy(x =>
                x.Characteristics[Characteristics.Initiative]).ToList();
        }

        public void Update()
        {
            var count = Queue.Take(_count).Count(x =>
                ((x is null)) || x.Characteristics[Characteristics.Health] <= 0);
            Queue = Queue.Where(x =>
                (!(x is null))&&x.Characteristics[Characteristics.Health] > 0).ToList();
            _count -= count;
        }

        public BasicCreature GetNextPerson()
        {
            _count++;
            if (_count >= Queue.Count) _count = 0;
            return Queue[_count];
        }
    }
}