﻿using System.Collections.Generic;
using System.Linq;

namespace Game.Model
{
    public class Inventory
    {
        public List<ActiveItem> Heap { get;  set; }
        public int Size { get;  set; }

        public Inventory(List<ActiveItem> heap = null, int size = 0) {
            Heap = heap ?? new List<ActiveItem>();
            Size = size;
        }

        public void Add(ActiveItem item)
        {
            if (Heap.Count < Size)
                Heap.Add(item);
        }
        
        public void Remove(string name) {
            var item = Heap.FirstOrDefault(element => element.Name == name);
            if (item is null)
                throw new System.ArgumentException("Item is not contained in inventory!");
            Heap.Remove(item);
        }

        public bool Contains(string name) => Heap.Any(item => item.Name == name);
    }
}