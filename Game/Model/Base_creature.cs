using System;

namespace Game.Model
{
    public abstract class BaseCreature//
    {
        public readonly string Name;
        public readonly int Health;
        public readonly CreatureType Type;

        public BaseCreature(string name, int health, CreatureType type)
        {
            Name = name;
            Health = health;
            Type = type;
        }
    }

    public enum CreatureType
    {
        Peaceful,
        Enemy,
        Hero
    }
}