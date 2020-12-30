using System;
using System.Collections.Generic;
using Game.Model;
using NUnit.Framework;

namespace Tests
{
    internal class InventoryShould
    {
        private Inventory _plainInventory;
        private List<ActiveItem> _thingsForAdding;

        [SetUp]
        public void SetUp()
        {
            var poison = new ActiveItem("Poison", new[] {(Characteristics.Health, -20)});
            var healingPotion = new ActiveItem("Heal", new[] {(Characteristics.Health, 20)});
            var commonItems = new List<ActiveItem> {poison, healingPotion};
            _plainInventory = new Inventory(commonItems, 4);

            _thingsForAdding = new List<ActiveItem>();
            for (var i = 0; i < 100; i++)
            {
                var randomThing = new ActiveItem($"Test item №{i}", new[] {(Characteristics.Evasion, 1)});
                _thingsForAdding.Add(randomThing);
            }
        }

        [Test]
        public void Test_EmptyInventory()
        {
            Assert.IsEmpty(new Inventory().Heap);
            Assert.AreEqual(0, new Inventory().Size);
        }

        [Test]
        public void Test_InventorySizeExceeded()
        {
            foreach (var thing in _thingsForAdding)
                _plainInventory.Add(thing);
            Assert.AreEqual(4, _plainInventory.Size);
        }

        [Test]
        public void Test_ContainsItemAtName()
        {
            Assert.True(_plainInventory.Contains("Heal"));
        }

        [Test]
        public void Test_NotContainsItemAtName()
        {
            Assert.False(_plainInventory.Contains("Test"));
        }

        [Test]
        public void Test_RemoveItem()
        {
            var newItem = new ActiveItem("newItem", new[] {(Characteristics.Health, -20)});
            _plainInventory.Add(newItem);
            Assert.True(_plainInventory.Heap.Contains(newItem));
            Assert.AreEqual(3, _plainInventory.Heap.Count);
            _plainInventory.Heap.Remove(newItem);
            Assert.AreEqual(2, _plainInventory.Heap.Count);
            Assert.False(_plainInventory.Heap.Contains(newItem));
        }

        [Test]
        public void Test_RemoveItemAtName()
        {
            Assert.True(_plainInventory.Contains("Poison"));
            _plainInventory.Remove("Poison");
            Assert.AreEqual(1, _plainInventory.Heap.Count);
            Assert.False(_plainInventory.Contains("Poison"));
        }

        [Test]
        public void Test_TryRemoveUnexistedItem()
        {
            Assert.False(_plainInventory.Contains("sword"));
            var exception = Assert.Throws<ArgumentException>(
                () => _plainInventory.Remove("sword")
            );
            Assert.AreEqual("Item is not contained in inventory!", exception.Message);
        }
    }
}