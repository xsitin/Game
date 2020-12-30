using System;
using System.Collections.Generic;
using Game.Model;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Creatures
    {
        [SetUp]
        public void SetUp()
        {
            _archer = new Hero("ker", new Dictionary<Characteristics, int> {{Characteristics.Health, 100}},
                new List<ActiveItem>(), Specialization.Archer, Position.Range, Location.SomeLocation);
            _archer.Characteristics[Characteristics.Health] = 100;
            _mage = new Hero("gfkrf", new Dictionary<Characteristics, int>
                {
                    {Characteristics.Health, 100}, {Characteristics.Mana, 200}
                },
                new List<ActiveItem>(), Specialization.Wizard, Position.Range, Location.SomeLocation);
            _enemy1 = new EnemyHero("pes", new Dictionary<Characteristics, int>(),
                new List<ActiveItem>(), Specialization.Wizard, Position.Melee, Location.SomeLocation);
            _enemy1.Characteristics[Characteristics.Health] = 50;
            _enemy1.Characteristics[Characteristics.Evasion] = 100;
            _enemy1.Characteristics[Characteristics.Mana] = 100;
            _enemy1.Skills.Add(new Skill(10, new[] {(Characteristics.Health, -10)},
                    SkillRange.Single, "skillName", new Buff(5, "debuff", (Characteristics.Initiative, -100)))
                {IsMagic = true});
            _enemy2 = new EnemyHero("very enemy", new Dictionary<Characteristics, int> {{Characteristics.Health, 100}},
                new List<ActiveItem>(), Specialization.Warrior, Position.Melee, Location.SomeLocation);
            _mage.Skills.Add(new Skill(50, new[] {(Characteristics.Health, -100)}, SkillRange.Enemies, "shpuf",
                new Buff(3, "shpuf debuff", (Characteristics.Evasion, -10))) {IsMagic = true});
        }

        private Hero _archer;
        private Hero _mage;
        private EnemyHero _enemy1;
        private EnemyHero _enemy2;

        [Test]
        public void AddExpToHero()
        {
            _archer.Exp += 100;
            Assert.AreEqual(100, _archer.Exp);
            Assert.AreEqual(1, _archer.Level);
            Assert.AreEqual(0, _archer.UpgradePoints);
            _archer.Exp += 400;
            Assert.AreEqual(2, _archer.Level);
            Assert.AreEqual(0, _archer.Exp);
            Assert.AreEqual(1, _archer.UpgradePoints);
            Assert.Catch(typeof(ArgumentException), () => _archer.Exp -= 100);
        }

        [Test]
        public void Buffing()
        {
            _enemy1.Buffs.Add(new Buff(_enemy1, 4, "buff", (Characteristics.Evasion, -10)));
            Assert.IsNotEmpty(_enemy1.Buffs);
            Assert.True(_enemy1.Characteristics[Characteristics.Evasion] == 90);
            _enemy1.Buffs.RemoveAt(0);
            Assert.IsEmpty(_enemy1.Buffs);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Assert.True(_enemy1.Characteristics[Characteristics.Evasion] == 100);
        }

        [Test]
        public void CorrectCharacteristics()
        {
            Assert.True(_archer.Characteristics[Characteristics.Health] == 100);
            Assert.True(_mage.Characteristics[Characteristics.Health] == 100);
            Assert.True(_mage.Characteristics[Characteristics.Mana] == 200);
            Assert.True(_enemy1.Characteristics[Characteristics.Health] == 50);
            Assert.True(_enemy1.Characteristics[Characteristics.Evasion] == 100);
            Assert.AreEqual("gfkrf", _mage.ToString());
        }

        [Test]
        public void CorrectEnemyConstructorWithoutPistion()
        {
            var range = new EnemyHero(Helper.GetName(), new Dictionary<Characteristics, int>(), new List<ActiveItem>(),
                Specialization.Archer, Location.SomeLocation);
            var melee = new EnemyHero(Helper.GetName(), new Dictionary<Characteristics, int>(), new List<ActiveItem>(),
                Specialization.Warrior, Location.SomeLocation);
            Assert.AreEqual(Position.Melee, melee.Position);
            Assert.AreEqual(Position.Range, range.Position);
        }

        [Test]
        public void SkillUpgrade()
        {
            Assert.AreEqual(-10, _enemy1.Skills[1].Effect[0].value);
            Assert.AreEqual(10, _enemy1.Skills[1].ManaCost);
            Assert.AreEqual(Characteristics.Health, _enemy1.Skills[1].Effect[0].characteristic);
            Assert.AreEqual(-100, _enemy1.Skills[1].Buff.Buffs[0].value);
            Assert.AreEqual(Characteristics.Initiative, _enemy1.Skills[1].Buff.Buffs[0].characteristic);
            _enemy1.Skills[1].Upgrade();
            Assert.AreEqual(-12, _enemy1.Skills[1].Effect[0].value);
            Assert.AreEqual(Characteristics.Health, _enemy1.Skills[1].Effect[0].characteristic);
            Assert.AreEqual(-120, _enemy1.Skills[1].Buff.Buffs[0].value);
            Assert.AreEqual(Characteristics.Initiative, _enemy1.Skills[1].Buff.Buffs[0].characteristic);
            Assert.AreEqual(12, _enemy1.Skills[1].ManaCost);
        }


        [Test]
        public void UsingSkillByEnemyToEnemy()
        {
            Assert.AreEqual(_enemy2.Characteristics[Characteristics.Health], 100);
            _enemy1.UseSkill(_enemy1.Skills[1], _enemy2);
            Assert.AreEqual(90, _enemy2.Characteristics[Characteristics.Health]);
            Assert.AreEqual(_enemy2.Characteristics[Characteristics.Initiative], -70);
            Assert.IsNotEmpty(_enemy2.Buffs);
            _enemy2.Buffs.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Assert.AreEqual(_enemy2.Characteristics[Characteristics.Initiative], 30);
        }

        [Test]
        public void UsingSkillByEnemyToHero()
        {
            _enemy1.UseSkill(_enemy1.Skills[1], _archer);
            Assert.AreEqual(_archer.Characteristics[Characteristics.Health],
                _archer.StandardChars[Characteristics.Health] - 10);
            Assert.IsNotEmpty(_archer.Buffs);
            Assert.AreNotEqual(_archer.Characteristics[Characteristics.Initiative],
                _archer.StandardChars[Characteristics.Initiative]);
            _archer.Buffs.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Assert.AreEqual(_archer.Characteristics[Characteristics.Initiative],
                _archer.StandardChars[Characteristics.Initiative]);
        }

        [Test]
        public void UsingSkillByHeroToEnemy()
        {
            _mage.UseSkill(_mage.Skills[1], _enemy1);
            Assert.IsNotNull(_mage.Skills[1].Buff);
            Assert.True(_enemy1.Characteristics[Characteristics.Health] < 0);
            Assert.True(_enemy1.Buffs.Count > 0);
            Assert.True(_enemy1.Characteristics[Characteristics.Evasion] == 90);
            _enemy1.Buffs.RemoveAt(0);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Assert.True(_enemy1.Characteristics[Characteristics.Evasion] == 100);
        }

        [Test]
        public void UsingSkillByHeroToHero()
        {
            _mage.UseSkill(_mage.Skills[1], _archer);
            Assert.True(_archer.Characteristics[Characteristics.Health] < _archer.StandardChars[Characteristics.Health]);
            Assert.IsNotEmpty(_archer.Buffs);
            _archer.Buffs.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Assert.IsEmpty(_archer.Buffs);
            Assert.IsTrue(_archer.Characteristics[Characteristics.Evasion] ==
                          _archer.StandardChars[Characteristics.Evasion]);
        }
    }
}