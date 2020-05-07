using System.Collections.Generic;

namespace Game.Model
{
    public class Inventory
    {
        public List<ActiveItem> Heap { get; private set; }
        public int Size { get; private set; }

        public void Add(ActiveItem item)
        {
            if (Heap.Count < Size)
                Heap.Add(item);
        }   
    }
}