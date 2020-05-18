﻿using System;
using System.Collections.Generic;
using System.Linq;
using Game.Model;
using NUnit.Framework;

namespace Tests
{
    public class GameQueueTest
    {
        [TestFixture]
        public class GameQueueTests
        {
            [TestCase(0, 1)]
            [TestCase(1, 0)]
            [TestCase(1, 2)]
            [TestCase(10, 10)]
            [TestCase(100, 100)]
            [TestCase(1000, 1000)]
            public void Initialization(int heroCount, int enemyCount)
            {
                var queue = TestHelper.GetQueue(heroCount, enemyCount);
                var previous = queue.Queue[0].Characteristics[Characteristics.Initiative];
                foreach (var creature in queue.Queue.Skip(1))
                {
                    if (previous < creature.Characteristics[Characteristics.Initiative])
                        Assert.Fail();
                    previous = creature.Characteristics[Characteristics.Initiative];
                }

                Assert.Pass();
            }

            [TestCase(1, 1, 1)]
            [TestCase(10, 10, 10)]
            [TestCase(100, 100, 1)]
            [TestCase(100, 100, 0)]
            [TestCase(1, 1, 2)]
            public void UpdateAfterDeath(int heroCount, int enemyCount, int toKill)
            {
                var queue = TestHelper.GetQueue(heroCount, enemyCount);
                var length = queue.Queue.Count - toKill;
                for (var i = 0; i < toKill; i++)
                    queue.Queue[i].Characteristics[Characteristics.Health] = 0;
                queue.Update();
                Assert.AreEqual(length, queue.Queue.Count);
            }

            [TestCase(1, 1, 1, 2)]
            [TestCase(1, 1, 2, 1)]
            public void NextPerson(int heroCount, int enemyCount, int current, int expect)
            {
                var queue = TestHelper.GetQueue(heroCount, enemyCount);
                var expectation = queue.Queue[expect - 1].Name;
                for (var i = 0; i < current ; i++)
                    queue.GetNextPerson();
                var real = queue.GetNextPerson().Name;
                Assert.AreEqual(expectation, real);
            }

            [TestCase(1, 1, 1)]
            [TestCase(2, 4, 4)]
            [TestCase(10, 10, 2)]
            [TestCase(100, 100, 100)]
            [TestCase(1000, 1000, 1000)]
            public void SaveOrderAfterRandomDeath(int heroCount, int enemyCount, int toKill)
            {
                var queue = TestHelper.GetQueue(heroCount, enemyCount);
                var random = new Random();
                for (var i = 0; i < toKill; i++)
                {
                    var rnd = (int) (random.NextDouble() * (queue.Queue.Count - 4) + 1);
                    queue.Queue[rnd].Characteristics[Characteristics.Health] = 0;
                    queue.Update();
                }

                for (var i = 1; i < queue.Queue.Count; i++)
                    if (queue.Queue[i].Characteristics[Characteristics.Initiative] > queue.Queue[i - 1]
                        .Characteristics[Characteristics.Initiative])
                        Assert.Fail();
                Assert.Pass();
            }

            [Test]
            public void ALotOfDeath()
            {
                var queue = TestHelper.GetQueue(3, 4);
                var expectation = queue.Queue[6].Name;
                queue.Queue[0].Characteristics[Characteristics.Health] = 0;
                queue.Queue[1].Characteristics[Characteristics.Health] = 0;
                queue.Queue[2].Characteristics[Characteristics.Health] = 0;
                queue.Queue[3].Characteristics[Characteristics.Health] = 0;
                queue.Queue[4].Characteristics[Characteristics.Health] = 0;
                queue.Queue[5].Characteristics[Characteristics.Health] = 0;
                Assert.AreEqual(expectation, queue.GetNextPerson().Name);
            }

            [Test]
            public void DieNextSomeCreatures()
            {
                var queue = TestHelper.GetQueue(2, 2);
                var expectation = queue.Queue[0].Name;
                queue.Queue[2].Characteristics[Characteristics.Health] = 0;
                queue.Queue[3].Characteristics[Characteristics.Health] = 0;
                var real = queue.GetNextPerson().Name;
                Assert.AreEqual(expectation, real);
            }

            [Test]
            public void FirstSomeCreatures()
            {
                var queue = TestHelper.GetQueue(2, 2);
                var expectation = queue.Queue[2].Name;
                for (var i = 0; i < 3; i++)
                    queue.GetNextPerson();
                queue.Queue[0].Characteristics[Characteristics.Health] = 0;
                queue.Queue[1].Characteristics[Characteristics.Health] = 0;
                var real = queue.GetNextPerson().Name;
                Assert.AreEqual(expectation, real);
            }

            [Test]
            public void NextPersonFirstDeath()
            {
                var queue = TestHelper.GetQueue(2, 2);
                var expectation = queue.Queue[1].Name;
                for (var i = 0; i < 3; i++)
                    queue.GetNextPerson();
                queue.Queue[0].Characteristics[Characteristics.Health] = 0;
                var real = queue.GetNextPerson().Name;
                Assert.AreEqual(expectation, real);
            }

            [Test]
            public void NextPersonLastDeath()
            {
                var queue = TestHelper.GetQueue(2, 2);
                var expectation = queue.Queue[0].Name;
                for (var i = 0; i < 3; i++)
                    queue.GetNextPerson();
                queue.Queue[3].Characteristics[Characteristics.Health] = 0;
                var real = queue.GetNextPerson().Name;
                Assert.AreEqual(expectation, real);
            }

            [Test]
            public void ThrowExceptionOnEmptyList()
            {
                var exception = Assert.Throws<FormatException>(() => new GameQueue(new List<BasicCreature>()));
                Assert.AreEqual(exception.Message, "Чел ты даун?");
            }
        }
    }

    public static class TestHelper
    {
        public static GameQueue GetQueue(int heroCount, int enemyCount)
        {
            var rnd = new Random();
            var list = new List<BasicCreature>();
            for (var i = 0; i < heroCount; i++)
                list.Add(new Hero($"HeroNum{i}", new Dictionary<Characteristics, int>
                {
                    {Characteristics.Health, (int) Math.Round(rnd.NextDouble() * 30) + 1},
                    {Characteristics.Initiative, (int) Math.Round(rnd.NextDouble() * 1000)},
                    {Characteristics.Evasion, 10},
                    {Characteristics.Mana, 10},
                    {Characteristics.MagicalProtection, 5},
                    {Characteristics.PhysicalDamage, 5},
                    {Characteristics.PhysicalProtection, 5}
                }, new List<ActiveItem>(), Specialization.Warrior, Position.Melee, Location.SomeLocation));
            for (var i = 0; i < enemyCount; i++)
                list.Add(new EnemyHero($"EnemyNum{i}", new Dictionary<Characteristics, int>
                {
                    {Characteristics.Health, (int) Math.Round(rnd.NextDouble() * 30) + 1},
                    {Characteristics.Initiative, (int) Math.Round(rnd.NextDouble() * 1000)},
                    {Characteristics.Evasion, 10},
                    {Characteristics.Mana, 10},
                    {Characteristics.MagicalProtection, 5},
                    {Characteristics.PhysicalDamage, 5},
                    {Characteristics.PhysicalProtection, 5}
                }, new List<ActiveItem>(), Specialization.Archer, Position.Melee, Location.SomeLocation));
            return new GameQueue(list);
        }
    }
}