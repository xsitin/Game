﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Game.Model;

namespace Tests {
    class Saves_Should {
        private Player player;
        private Player loadedPlayer;
        private Hero heroWarrior;
        private Hero heroArcher;
        private ActiveItem healingPotion;

        [SetUp]
        public void SetUp() {
            healingPotion = new ActiveItem("heal", new[] { (Characteristics.Health, 20) });
            heroWarrior = new Hero("Герой", new Dictionary<Characteristics, int>() { [Characteristics.Health] = 200 },
               new Inventory(), Specialization.Warrior, Position.Melee, Location.SomeLocation);
            heroArcher = new Hero("Герой", new Dictionary<Characteristics, int>() { [Characteristics.Health] = 200 },
               new Inventory(), Specialization.Archer, Position.Range, Location.SomeLocation);
            player = new Player() {
                PlayerName = "Player1",
                Gold = 100,
                Heroes = new List<Hero>() { heroWarrior, heroArcher },
                Mercenaries = new List<Hero>(),
                Shop = new List<ActiveItem>(),
                Storage = new List<ActiveItem>() { healingPotion },
            };

            Database.SaveGame(player);
            loadedPlayer = Database.LoadGame("Player1");
        }

        [Test]
        public void Test_CompareNames() => Assert.AreEqual(player.PlayerName, loadedPlayer.PlayerName);

        [Test]
        public void Test_CompareGold() => Assert.AreEqual(player.Gold, loadedPlayer.Gold);

        [Test]
        public void Test_CompareHeroes() {
            for (int i = 0; i < player.Heroes.Count; i++) {
                var hero = player.Heroes[i];
                var loadedHero = loadedPlayer.Heroes[i];
                if (!hero.Equals(loadedHero))
                    Assert.Fail();
            }
            Assert.Pass();
        }

        [Test]
        public void Test_CompareLists() {
            for (int i = 0; i < player.Heroes.Count; i++) {
                var hero = player.Mercenaries[i];
                var loadedHero = loadedPlayer.Mercenaries[i];
                if (!hero.Equals(loadedHero))
                    Assert.Fail();
            }
            Assert.Pass();
        }

        [Test]
        public void Test_CompareShops() {
            for (int i = 0; i < player.Shop.Count; i++) {
                var hero = player.Shop[i];
                var loadedHero = loadedPlayer.Shop[i];
                if (!hero.Equals(loadedHero))
                    Assert.Fail();
            }
            Assert.Pass();
        }

        [Test]
        public void Test_CompareStorage() {
            for (int i = 0; i < player.Storage.Count; i++) {
                var hero = player.Storage[i];
                var loadedHero = loadedPlayer.Storage[i];
                if (!hero.Equals(loadedHero))
                    Assert.Fail();
            }
            Assert.Pass();
        }
    }
}