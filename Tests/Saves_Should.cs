using System.Collections.Generic;
using Game.Model;
using NUnit.Framework;

namespace Tests
{
    internal class SavesShould
    {
        private ActiveItem _healingPotion;
        private Hero _heroArcher;
        private Hero _heroWarrior;
        private Player _loadedPlayer;
        private Player _player;

        [SetUp]
        public void SetUp()
        {
            _healingPotion = new ActiveItem("heal", new[] {(Characteristics.Health, 20)});
            _heroWarrior = new Hero(Helper.GetName(),
                new Dictionary<Characteristics, int> {[Characteristics.Health] = 200},
                new List<ActiveItem>(), Specialization.Warrior, Position.Melee, Location.SomeLocation);
            _heroArcher = new Hero(Helper.GetName(),
                new Dictionary<Characteristics, int> {[Characteristics.Health] = 200},
                new List<ActiveItem>(), Specialization.Archer, Position.Range, Location.SomeLocation);
            _player = new Player
            {
                PlayerName = "Player1",
                Gold = 100,
                Heroes = new List<Hero> {_heroWarrior, _heroArcher},
                Mercenaries = new List<Hero> {_heroWarrior, _heroArcher},
                Shop = new List<ActiveItem>(),
                Storage = new List<ActiveItem> {_healingPotion}
            };

            Helper.SaveGame(_player);
            _loadedPlayer = Helper.LoadGame("Player1");
        }

        [Test]
        public void Test_CompareNames()
        {
            Assert.AreEqual(_player.PlayerName, _loadedPlayer.PlayerName);
        }

        [Test]
        public void Test_CompareGold()
        {
            Assert.AreEqual(_player.Gold, _loadedPlayer.Gold);
        }

        [Test]
        public void Test_CompareHeroes()
        {
            for (var i = 0; i < _player.Heroes.Count; i++)
            {
                var hero = _player.Heroes[i];
                var loadedHero = _loadedPlayer.Heroes[i];
                if (!hero.Equals(loadedHero))
                    Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void Test_CompareMercs()
        {
            for (var i = 0; i < _player.Mercenaries.Count; i++)
            {
                var hero = _player.Mercenaries[i];
                var loadedHero = _loadedPlayer.Mercenaries[i];
                if (!hero.Equals(loadedHero))
                    Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void Test_CompareShops()
        {
            for (var i = 0; i < _player.Shop.Count; i++)
            {
                var hero = _player.Shop[i];
                var loadedHero = _loadedPlayer.Shop[i];
                if (!hero.Equals(loadedHero))
                    Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void Test_CompareStorage()
        {
            for (var i = 0; i < _player.Storage.Count; i++)
            {
                var hero = _player.Storage[i];
                var loadedHero = _loadedPlayer.Storage[i];
                if (!hero.Equals(loadedHero))
                    Assert.Fail();
            }

            Assert.Pass();
        }
    }
}