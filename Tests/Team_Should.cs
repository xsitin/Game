using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Game.Model;

namespace Tests {
    public class Team_Should {
        private Hero heroWarrior;
        private Hero heroArcher;
        private EnemyHero enemyWarrior;
        private EnemyHero enemyWizard;

        [SetUp]
        public void SetUp() {
            heroWarrior = new Hero("Герой", new Dictionary<Characteristics, int>() { [Characteristics.Health] = 200 },
               new Inventory(), Specialization.Warrior, Position.Melee, Location.SomeLocation);
            heroArcher = new Hero("Герой", new Dictionary<Characteristics, int>() { [Characteristics.Health] = 200 },
              new Inventory(), Specialization.Archer, Position.Range, Location.SomeLocation);
            enemyWarrior = new EnemyHero("Злодей", new Dictionary<Characteristics, int>() { [Characteristics.Health] = 200 },
               new Inventory(), Specialization.Warrior, Position.Melee, Location.SomeLocation);
            enemyWizard = new EnemyHero("Злодей", new Dictionary<Characteristics, int>() { [Characteristics.Health] = 200 },
               new Inventory(), Specialization.Warrior, Position.Range, Location.SomeLocation);
        }

        [Test]
        public void Test_Exception_OnEmptyList() {
            var exception = Assert.Throws<ArgumentException>(
                () => new Team<BasicCreature>(new List<BasicCreature>(), new List<BasicCreature>())
            );
            Assert.AreEqual("Lines shouldn't be empty!", exception.Message);
        }

        [Test]
        public void Test_SecondLineIsEmpty() {
            var warriors = new List<Hero>() { heroWarrior, heroWarrior };
            var team = new Team<Hero>(warriors, new List<Hero>());
            Assert.AreEqual(warriors, team.GetTeamList());
        }

        [Test]
        public void Test_LinesAreEquals() {
            var warriors = new List<Hero>() { heroWarrior, heroWarrior };
            var exception = Assert.Throws<ArgumentException>(
                () => new Team<Hero>(warriors, warriors)
            );
            Assert.AreEqual("Lines shouldn't be equals!", exception.Message);
        }

        [Test]
        public void Test_FirstLineIsEmpty() {
            var archers = new List<Hero>() { heroArcher, heroArcher };
            var team = new Team<Hero>(new List<Hero>(), archers);
            Assert.AreEqual(archers, team.GetTeamList());
        }

        [Test]
        public void Test_WellHeroesSquads() {
            var archers = new List<Hero>() { heroArcher, heroArcher };
            var warriors = new List<Hero>() { heroWarrior, heroWarrior };
            var heroesTeam = new Team<Hero>(warriors, archers);
            Assert.AreEqual(heroesTeam.GetTeamList(), warriors.Concat(archers).ToList());
        }

        [Test]
        public void Test_WellEnemiesSquads() {
            var warriors = new List<EnemyHero>() { enemyWarrior, enemyWarrior };
            var wizards = new List<EnemyHero>() { enemyWizard, enemyWizard };
            var heroesTeam = new Team<EnemyHero>(warriors, wizards);
            Assert.AreEqual(heroesTeam.GetTeamList(), warriors.Concat(wizards).ToList());
        }
    }
}
