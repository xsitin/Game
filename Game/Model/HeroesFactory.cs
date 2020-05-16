using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Model
{
    public class HeroesFactory
    {
        private int _level;
        private Player Player;

        public HeroesFactory(Player player)
        {
            Player = player;
            _level = (player.Heroes.Max(x => x.Level) / 5) * 5;
        }
        
        public void Update() => _level = (Player.Heroes.Max(x => x.Level) / 5) * 5;

        private Specialization last = Specialization.Wizard;
        public Hero GetRandomHero()
        {
            var spec = (Specialization) new Random().Next(0, 2);
            while(spec==last)
                spec = (Specialization) new Random().Next(0, 2);
            last = spec;
            return GetHero(spec);
        }

        private Hero GetHero(Specialization spec)
        {
            var random = new Random();
            var hero = new Hero(Helper.GetName(), new Dictionary<Characteristics, int>(), new Inventory(), spec,
                Location.SomeLocation);

            while (hero.Skills.Count < 2)
            {
                var skill = Helper.BasicSkills[spec][
                    random.Next(0, Helper.BasicSkills[spec].Count)];
                if (hero.Skills.All(x => x.Name != skill.Name))
                    hero.Skills.Add(skill);
            }

            for (var j = 1; j < _level; j++)
                if (random.Next(0, 2) == 1)
                {
                    var character =
                        (Characteristics) random.Next(0, Enum.GetValues(typeof(Characteristics)).Length - 1);
                    if (((character == Characteristics.Evasion || character == Characteristics.MagicalProtection ||
                          character == Characteristics.PhysicalProtection) &&
                         (int) (hero.Characteristics[character] * 1.2) > 100) ||
                        (character == Characteristics.PhysicalProtection &&
                         hero.Specialization == Specialization.Wizard))
                    {
                        j--;
                        continue;
                    }

                    hero.Characteristics[character] = (int) (hero.Characteristics[character] * 1.2);
                }
                else
                    hero.Skills[random.Next(0, hero.Skills.Count - 1)].Upgrade();

            hero.Level = _level;
            return hero;
        }
    }
}