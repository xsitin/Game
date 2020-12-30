using System.Collections.Generic;
using Game.Model;
using NUnit.Framework;

namespace Tests
{
    internal class ItemsShould
    {
        private EnemyHero _enemyArcher;
        private Team<EnemyHero> _enemySquad;
        private EnemyHero _enemyWarrior1;
        private EnemyHero _enemyWarrior2;
        private EnemyHero _enemyWizard;
        private Hero _heroArcher;
        private Team<Hero> _heroSquad;
        private Hero _heroWarrior;

        [SetUp]
        public void SetUp()
        {
            _heroWarrior = new Hero("Герой", new Dictionary<Characteristics, int> {[Characteristics.Health] = 200},
                new List<ActiveItem>(), Specialization.Warrior, Position.Melee, Location.SomeLocation);
            _heroArcher = new Hero("Герой", new Dictionary<Characteristics, int> {[Characteristics.Health] = 200},
                new List<ActiveItem>(), Specialization.Archer, Position.Range, Location.SomeLocation);
            _enemyWizard = new EnemyHero("Злодей", new Dictionary<Characteristics, int> {[Characteristics.Health] = 200},
                new List<ActiveItem>(), Specialization.Wizard, Position.Range, Location.SomeLocation);
            _enemyArcher = new EnemyHero("Злодей", new Dictionary<Characteristics, int> {[Characteristics.Health] = 200},
                new List<ActiveItem>(), Specialization.Archer, Position.Range, Location.SomeLocation);
            _enemyWarrior1 = new EnemyHero("Злодей",
                new Dictionary<Characteristics, int> {[Characteristics.Health] = 200},
                new List<ActiveItem>(), Specialization.Warrior, Position.Range, Location.SomeLocation);
            _enemyWarrior2 = new EnemyHero("Злодей",
                new Dictionary<Characteristics, int> {[Characteristics.Health] = 200},
                new List<ActiveItem>(), Specialization.Warrior, Position.Range, Location.SomeLocation);

            var heroMelee = new List<Hero> {_heroWarrior};
            var heroRangers = new List<Hero> {_heroArcher};
            _heroSquad = new Team<Hero>(heroMelee, heroRangers);

            var enemyMelee = new List<EnemyHero> {_enemyWarrior1, _enemyWarrior2};
            var enemyRangers = new List<EnemyHero> {_enemyWizard, _enemyArcher};
            _enemySquad = new Team<EnemyHero>(enemyMelee, enemyRangers);
        }

        [Test]
        public void Test_OneItem_OneTarget()
        {
            var poison = new ActiveItem("poison", new[] {(Characteristics.Health, -20)});
            poison.Use(_enemyWizard);
            Assert.AreEqual(180, _enemyWizard.Characteristics[Characteristics.Health]);
        }

        [Test]
        public void Test_OneItem_SomeTargets()
        {
            var poison = new ActiveItem("poison", new[] {(Characteristics.Health, -20)});
            var targets = _enemySquad.GetTeamList().ToArray();
            poison.Use(targets);

            foreach (var target in targets)
                Assert.AreEqual(180, target.Characteristics[Characteristics.Health]);
        }

        [Test]
        public void Test_SomeItem_OneTarget()
        {
            Assert.AreEqual(200, _enemyWizard.Characteristics[Characteristics.Health]);
            var poison = new ActiveItem("poison", new[] {(Characteristics.Health, -20)});
            var healingPotion = new ActiveItem("heal", new[] {(Characteristics.Health, 20)});
            poison.Use(_enemyWizard);
            healingPotion.Use(_enemyWizard);
            Assert.AreEqual(200, _enemyWizard.Characteristics[Characteristics.Health]);
        }

        [Test]
        public void Test_TwoItems_TwoTargetsDifferentSides()
        {
            var poison = new ActiveItem("poison", new[] {(Characteristics.Health, -20)});
            var healingPotion = new ActiveItem("heal", new[] {(Characteristics.Health, 20)});
            poison.Use(_enemyWizard);
            healingPotion.Use(_heroWarrior);

            Assert.AreEqual(180, _enemyWizard.Characteristics[Characteristics.Health]);
            Assert.AreEqual(220, _heroWarrior.Characteristics[Characteristics.Health]);
        }

        [Test]
        public void Test_SomeItem_SomeTargets()
        {
            var poison = new ActiveItem("poison", new[] {(Characteristics.Health, -20)});
            var healingPotion = new ActiveItem("heal", new[] {(Characteristics.Health, 20)});

            var enemies = _enemySquad.GetTeamList().ToArray();
            var heroes = _heroSquad.GetTeamList().ToArray();
            poison.Use(enemies);
            healingPotion.Use(heroes);

            foreach (var target in enemies)
                Assert.AreEqual(180, target.Characteristics[Characteristics.Health]);
            foreach (var target in heroes)
                Assert.AreEqual(220, target.Characteristics[Characteristics.Health]);
        }

        [Test]
        public void Test_OneMultiItem_OneTarget()
        {
            var heal = new ActiveItem("heal", new[] {(Characteristics.Health, 20), (Characteristics.Initiative, 1)});
            heal.Use(_heroWarrior);

            Assert.AreEqual(220, _heroWarrior.Characteristics[Characteristics.Health]);
            Assert.True(_heroWarrior.Characteristics[Characteristics.Initiative] == 31);
        }

        [Test]
        public void Test_OneMultiItem_OneTargets()
        {
            var poison = new ActiveItem("poison",
                new[] {(Characteristics.Health, -20), (Characteristics.Initiative, -1)});
            var targets = _enemySquad.GetTeamList().ToArray();
            poison.Use(targets);

            foreach (var target in targets)
            {
                Assert.AreEqual(180, target.Characteristics[Characteristics.Health]);
                Assert.True(target.Characteristics[Characteristics.Initiative] == 29);
            }
        }

        [Test]
        public void Test_Buffusingitem()
        {
            var poising = new Buff(_enemyWizard, 1, "poising", (Characteristics.Health, -10));
            var arrowWithPoison = new ActiveItem("ArrowWithPoison", new (Characteristics, int)[0], poising);
            arrowWithPoison.Use(_enemyWizard);
            Assert.True(_enemyWizard.Characteristics[Characteristics.Health] == 190);
        }
    }
}