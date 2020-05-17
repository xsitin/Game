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
        private HeroesFactory Factory;
        private Player Pl;

        [SetUp]
        public void SetUp()
        {
            Pl = new Player();
            Pl.Heroes = new List<Hero>();
            Pl.Heroes.AddRange(Helper.GetHeroTeam(3,3,10000000).GetTeamList().Cast<Hero>());
            Factory = new HeroesFactory(Pl);
        }

        [Test]
        public void MagicTest()
        {
            Assert.AreEqual((Pl.Heroes.Max(x=>x.Level)/5)*5, Factory.GetRandomHero().Level);
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
                return new Team<Hero>(first,second);
            }
        }
    }
}