﻿using System;

namespace Game.Model
{
    public class Skill
    {
        public string Name { get; }
        public int Level { get; private set; }
        public int ManaCost { get; private set; }
        public SkillRange Range;
        public (Characteristics characteristic , int value)[] Effect { get; }
        public bool IsMagic;

        public Buff Buff { get; set; }
        public Skill(int manaCost, (Characteristics characteristic, int value)[] effect, SkillRange range, string name, Buff buff)
        {
            ManaCost = manaCost;
            Effect = effect;
            Range = range;
            Name = name;
            Buff = buff;
            Level = 1;
        }

        public void Upgrade()
        {
            Level++;
            ManaCost = (int) (ManaCost * 1.2);
            for (var i = 0; i < Effect.Length; i++)
                Effect[i].value = (int) (Effect[i].value * 1.2);
            if (Buff != null)
                for (var i = 0; i < Buff.Buffs.Length; i++)
                    Buff.Buffs[i].value = (int) (Buff.Buffs[i].value * 1.2);
        }
    }

    public enum SkillRange
    {
        All,
        Friendly,
        Enemies, 
        Single
    }
}