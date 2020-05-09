﻿using System;
using System.Collections.Generic;
using Game.Model;
using NUnit.Framework;

namespace Tests
{
    internal class Items_Should
    {
        private EnemyHero enemyArcher;
        private Team<EnemyHero> enemySquad;
        private EnemyHero enemyWarrior1;
        private EnemyHero enemyWarrior2;
        private EnemyHero enemyWizard;
        private Hero heroArcher;
        private Team<Hero> heroSquad;
        private Hero heroWarrior;

        [SetUp]
        public void SetUp()
        {
            heroWarrior = new Hero("Герой", new Dictionary<Characteristics, int> {[Characteristics.Health] = 200},
                new Inventory(), Specialization.Warrior, Position.Melee, Location.SomeLocation);
            heroArcher = new Hero("Герой", new Dictionary<Characteristics, int> {[Characteristics.Health] = 200},
                new Inventory(), Specialization.Archer, Position.Range, Location.SomeLocation);
            enemyWizard = new EnemyHero("Злодей", new Dictionary<Characteristics, int> {[Characteristics.Health] = 200},
                new Inventory(), Specialization.Wizard, Position.Range, Location.SomeLocation);
            enemyArcher = new EnemyHero("Злодей", new Dictionary<Characteristics, int> {[Characteristics.Health] = 200},
                new Inventory(), Specialization.Archer, Position.Range, Location.SomeLocation);
            enemyWarrior1 = new EnemyHero("Злодей",
                new Dictionary<Characteristics, int> {[Characteristics.Health] = 200},
                new Inventory(), Specialization.Warrior, Position.Range, Location.SomeLocation);
            enemyWarrior2 = new EnemyHero("Злодей",
                new Dictionary<Characteristics, int> {[Characteristics.Health] = 200},
                new Inventory(), Specialization.Warrior, Position.Range, Location.SomeLocation);

            var heroMelee = new List<Hero> {heroWarrior};
            var heroRangers = new List<Hero> {heroArcher};
            heroSquad = new Team<Hero>(heroMelee, heroRangers);

            var enemyMelee = new List<EnemyHero> {enemyWarrior1, enemyWarrior2};
            var enemyRangers = new List<EnemyHero> {enemyWizard, enemyArcher};
            enemySquad = new Team<EnemyHero>(enemyMelee, enemyRangers);
        }

        [Test]
        public void Test_NullUsabilityItem()
        {
            var exception = Assert.Throws<ArgumentException>(() => new ActiveItem("test", null));
            Assert.AreEqual(exception.Message, "Action shouldn't be null!");
        }

        [Test]
        public void Test_OneItem_OneTarget()
        {
            var poison = new ActiveItem("test", target => target.Characteristics[Characteristics.Health] -= 20);
            poison.Use(enemyWizard);
            Assert.AreEqual(180, enemyWizard.Characteristics[Characteristics.Health]);
        }

        [Test]
        public void Test_OneItem_SomeTargets()
        {
            var poison = new ActiveItem("test", target => target.Characteristics[Characteristics.Health] -= 20);
            var targets = enemySquad.GetTeamList().ToArray();
            poison.Use(targets);

            foreach (var target in targets)
                Assert.AreEqual(180, target.Characteristics[Characteristics.Health]);
        }

        [Test]
        public void Test_SomeItem_OneTarget()
        {
            Assert.AreEqual(200, enemyWizard.Characteristics[Characteristics.Health]);
            var poison = new ActiveItem("posion", target => target.Characteristics[Characteristics.Health] -= 20);
            var healingPotion = new ActiveItem("heal", target => target.Characteristics[Characteristics.Health] += 20);
            poison.Use(enemyWizard);
            healingPotion.Use(enemyWizard);
            Assert.AreEqual(200, enemyWizard.Characteristics[Characteristics.Health]);
        }

        [Test]
        public void Test_TwoItems_TwoTargetsDifferentSides()
        {
            var poison = new ActiveItem("posion", target => target.Characteristics[Characteristics.Health] -= 20);
            var healingPotion = new ActiveItem("heal", target => target.Characteristics[Characteristics.Health] += 20);
            poison.Use(enemyWizard);
            healingPotion.Use(heroWarrior);

            Assert.AreEqual(180, enemyWizard.Characteristics[Characteristics.Health]);
            Assert.AreEqual(220, heroWarrior.Characteristics[Characteristics.Health]);
        }

        [Test]
        public void Test_SomeItem_SomeTargets()
        {
            var poison = new ActiveItem("posion", target => target.Characteristics[Characteristics.Health] -= 20);
            var healingPotion = new ActiveItem("heal", target => target.Characteristics[Characteristics.Health] += 20);

            var enemies = enemySquad.GetTeamList().ToArray();
            var heroes = heroSquad.GetTeamList().ToArray();
            poison.Use(enemies);
            healingPotion.Use(heroes);

            foreach (var target in enemies)
                Assert.AreEqual(180, target.Characteristics[Characteristics.Health]);
            foreach (var target in heroes)
                Assert.AreEqual(220, target.Characteristics[Characteristics.Health]);
        }
    }
}