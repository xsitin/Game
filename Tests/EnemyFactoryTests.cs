using System;
using System.Collections.Generic;
using System.Linq;
using Game.Model;
using NUnit.Framework;

namespace Tests
{
    public class EnemyFactoryTests
    {
        [TestFixture]
        public class FactoryTry
        {
            [TestCase(2, 2, 2000)]
            [TestCase(2, 0, 0)]
            [TestCase(1, 1, 500000)]
            public void LevelUpEnemy(int firstLineCount, int secondLineCount, int exp)
            {
                var team = Helper.GetHeroTeam(firstLineCount, secondLineCount, exp);
                var factory = new EnemyFactory(team, Location.SomeLocation);
                var enemyTeam = factory.GetEnemyTeam();
                var basic = BasicCreature.BaseCharacteristics;
                var counter = enemyTeam.GetTeamList()
                    .SelectMany(enemy => basic.Keys, (enemy, chars) => new {enemy, chars})
                    .Where(t => t.enemy.Characteristics[t.chars] != basic[t.chars] ||
                                t.enemy.Skills.Any(x => x.Level != 1))
                    .Select(t => t.enemy).Count();

                if (team.GetTeamList().Any(x => ((Hero) x).Level > 1) && counter == 0)
                    Assert.Fail();
                else if (!team.GetTeamList().Any(x => ((Hero) x).Level > 1) && counter != 0)
                    Assert.Fail();
                Assert.Pass();
            }

            [TestCase(2000, 2000)]
            [TestCase(2, 2)]
            [TestCase(24, 24)]
            [TestCase(24, 0)]
            public void GenerateOnly8(int firstLineCount, int secondLineCount)
            {
                var team = Helper.GetHeroTeam(firstLineCount, secondLineCount, 0);
                var factory = new EnemyFactory(team, Location.SomeLocation);
                var enemyTeam = factory.GetEnemyTeam();
                Assert.True(enemyTeam.GetTeamList().Count < 9);
            }

            [TestCase(1, 1, 0)]
            [TestCase(2, 2, 2)]
            [TestCase(2, 2, 10)]
            public void MeleeLineAlwaysContainsAny(int firstLineCount, int secondLineCount, int count)
            {
                var team = Helper.GetHeroTeam(firstLineCount, secondLineCount, 0);
                var factory = new EnemyFactory(team, Location.SomeLocation);
                for (var i = 0; i < count; i++)
                {
                    var enemy = factory.GetEnemyTeam();
                    if (!enemy.FirstLine.Any())
                        Assert.Fail();
                }

                Assert.Pass();
            }

            [TestCase(2, 2, 2000)]
            [TestCase(2, 2, 250000)]
            public void SomeCharsAreAlwaysLowerThen100(int firstLineCount, int secondLineCount, int exp)
            {
                var team = Helper.GetHeroTeam(firstLineCount, secondLineCount, exp);
                var factory = new EnemyFactory(team, Location.SomeLocation);
                var enemyTeam = factory.GetEnemyTeam().GetTeamList();
                foreach (var enemy in from enemy in enemyTeam
                    from chars in enemy.Characteristics.Keys
                    where
                        (chars == Characteristics.Evasion || chars == Characteristics.MagicalProtection ||
                         chars == Characteristics.PhysicalProtection)
                        && enemy.Characteristics[chars] > 100
                    select enemy) Assert.Fail();
                Assert.Pass();
            }
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