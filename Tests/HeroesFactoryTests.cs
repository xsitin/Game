using System;
using System.Collections.Generic;
using System.Linq;
using Game.Model;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class HeroesFactoryTests
    {
        [SetUp]
        public void SetUp()
        {
            _pl = new Player();
            _pl.Heroes = new List<Hero>();
            _pl.Heroes.AddRange(Helper.GetHeroTeam(3, 3, 10000000).GetTeamList().Cast<Hero>());
            _factory = new HeroesFactory(_pl);
        }

        private HeroesFactory _factory;
        private Player _pl;

        [Test]
        public void MagicTest()
        {
            Assert.AreEqual(_pl.Heroes.Max(x => x.Level) / 5 * 5, _factory.GetRandomHero().Level);
        }

        private static class Helper
        {
            public static Team<Hero> GetHeroTeam(int firstLine, int secondLine, int expDifference)
            {
                var rnd = new Random();
                var first = new List<Hero>();
                var second = new List<Hero>();
                for (var i = 0; i < firstLine; i++)
                {
                    var hero = new Hero($"Melee{i}", new Dictionary<Characteristics, int>
                    {
                        {Characteristics.Health, (int) Math.Round(rnd.NextDouble() * 30) + 1},
                        {Characteristics.Initiative, (int) Math.Round(rnd.NextDouble() * 1000)},
                        {Characteristics.Evasion, 10},
                        {Characteristics.Mana, 10},
                        {Characteristics.MagicalProtection, 5},
                        {Characteristics.PhysicalDamage, 5},
                        {Characteristics.PhysicalProtection, 5}
                    }, new List<ActiveItem>(), Specialization.Warrior, Position.Melee, Location.SomeLocation);
                    hero.Exp += expDifference;
                    first.Add(hero);
                }

                for (var i = 0; i < secondLine; i++)
                {
                    var hero = new Hero($"Ranger{i}", new Dictionary<Characteristics, int>
                    {
                        {Characteristics.Health, (int) Math.Round(rnd.NextDouble() * 30) + 1},
                        {Characteristics.Initiative, (int) Math.Round(rnd.NextDouble() * 1000)},
                        {Characteristics.Evasion, 10},
                        {Characteristics.Mana, 10},
                        {Characteristics.MagicalProtection, 5},
                        {Characteristics.PhysicalDamage, 5},
                        {Characteristics.PhysicalProtection, 5}
                    }, new List<ActiveItem>(), Specialization.Wizard, Position.Range, Location.SomeLocation);
                    hero.Exp += expDifference;
                    second.Add(hero);
                }

                return new Team<Hero>(first, second);
            }
        }
    }
}