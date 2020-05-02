using System.Collections.Generic;

namespace Game.Model
{
    public class Hero:BaseCreature
    {
        public Hero(string name, int health, CreatureType type) : base(name, health, type)
        { }

        public List<Item> Inventory = new List<Item>();
    }
}