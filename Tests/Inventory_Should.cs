﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Game.Model;

namespace Tests {
    class Inventory_Should {
        private Inventory plainInventory;
        private List<ActiveItem> thingsForAdding;

        [SetUp]
        public void SetUp() {
            var poison = new ActiveItem("poison", target => target.Characteristics[Characteristics.Health] -= 20);
            var healingPotion = new ActiveItem("Heal", target => target.Characteristics[Characteristics.Health] += 20);
            var commonItems = new List<ActiveItem>() { poison, healingPotion };
            plainInventory = new Inventory(commonItems, 4);

            thingsForAdding = new List<ActiveItem>();
            for (int i = 0; i < 100; i++) {
                var randomThing = new ActiveItem($"Test item №{i}", target => target.Characteristics[Characteristics.Evasion]++);
                thingsForAdding.Add(randomThing);
            }
        }

        [Test]
        public void Test_EmptyInventory() {
            Assert.IsEmpty(new Inventory().Heap);
            Assert.AreEqual(0, new Inventory().Size);
        }

        [Test]
        public void Test_InventorySizeExceeded() {
            foreach (var thing in thingsForAdding)
                plainInventory.Add(thing);
            Assert.AreEqual(4, plainInventory.Size);
        }

        [Test]
        public void Test_ContainsItemAtName() {
            Assert.True(plainInventory.Contains("Heal"));
        }

        [Test]
        public void Test_RemoveItem() {
            var newItem = new ActiveItem("newItem", item => { });
            plainInventory.Add(newItem);
            Assert.True(plainInventory.Heap.Contains(newItem));
            Assert.AreEqual(3, plainInventory.Heap.Count);
            plainInventory.Heap.Remove(newItem);
            Assert.AreEqual(2, plainInventory.Heap.Count);
            Assert.False(plainInventory.Heap.Contains(newItem));
        }

        [Test]
        public void Test_RemoveItemAtName() {
            Assert.True(plainInventory.Contains("poison"));
            plainInventory.Remove("poison");
            Assert.AreEqual(1, plainInventory.Heap.Count);
            Assert.False(plainInventory.Contains("poison"));
        }

        [Test]
        public void Test_TryRemoveUnexistedItem() {
            Assert.False(plainInventory.Contains("sword"));
            var exception = Assert.Throws<ArgumentException>(
                () => plainInventory.Remove("sword")
            );
            Assert.AreEqual("Item is not contained in inventory!", exception.Message);
        }
    }
}
