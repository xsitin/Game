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
            Archer = new Hero("ker", new Dictionary<Characteristics, int> {{Characteristics.Health, 100}},
                new Inventory(), Specialization.Archer, Position.Range, Location.SomeLocation);
            Archer.Characteristics[Characteristics.Health] = 100;
            Mage = new Hero("gfkrf", new Dictionary<Characteristics, int>
                {
                    {Characteristics.Health, 100}, {Characteristics.Mana, 200}
                },
                new Inventory(), Specialization.Wizard, Position.Range, Location.SomeLocation);
            Enemy1 = new EnemyHero("pes", new Dictionary<Characteristics, int>(),
                new Inventory(), Specialization.Wizard, Position.Melee, Location.SomeLocation);
            Enemy1.Characteristics[Characteristics.Health] = 50;
            Enemy1.Characteristics[Characteristics.Evasion] = 100;
            Enemy1.Characteristics[Characteristics.Mana] = 100;
            Enemy1.Skills.Add(new Skill(10, new[] {(Characteristics.Health, -10)},
                SkillRange.Single, "skillName", new Buff(5, "debuff", (Characteristics.Initiative, -100))));
            Enemy2 = new EnemyHero("very enemy", new Dictionary<Characteristics, int> {{Characteristics.Health, 100}},
                new Inventory(), Specialization.Warrior, Position.Melee, Location.SomeLocation);
            Mage.Skills.Add(new Skill(50, new[] {(Characteristics.Health, -100)}, SkillRange.Enemies, "shpuf",
                new Buff(3, "shpuf debuff", (Characteristics.Evasion, -10))));
        }

        private Hero Archer;
        private Hero Mage;
        private EnemyHero Enemy1;
        private EnemyHero Enemy2;

        [Test]
        public void AddExpToHero()
        {
            Archer.Exp += 100;
            Assert.AreEqual(100, Archer.Exp);
            Assert.AreEqual(1, Archer.Level);
            Assert.AreEqual(0, Archer.UpgradePoints);
            Archer.Exp += 400;
            Assert.AreEqual(2, Archer.Level);
            Assert.AreEqual(0, Archer.Exp);
            Assert.AreEqual(1, Archer.UpgradePoints);
            Assert.Catch(typeof(ArgumentException), () => Archer.Exp -= 100);
        }

        [Test]
        public void Buffing()
        {
            Enemy1.Buffs.Add(new Buff(Enemy1, 4, "buff", (Characteristics.Evasion, -10)));
            Assert.IsNotEmpty(Enemy1.Buffs);
            Assert.True(Enemy1.Characteristics[Characteristics.Evasion] == 90);
            Enemy1.Buffs.RemoveAt(0);
            Assert.IsEmpty(Enemy1.Buffs);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Assert.True(Enemy1.Characteristics[Characteristics.Evasion] == 100);
        }

        [Test]
        public void CorrectCharacteristics()
        {
            Assert.True(Archer.Characteristics[Characteristics.Health] == 100);
            Assert.True(Mage.Characteristics[Characteristics.Health] == 100);
            Assert.True(Mage.Characteristics[Characteristics.Mana] == 200);
            Assert.True(Enemy1.Characteristics[Characteristics.Health] == 50);
            Assert.True(Enemy1.Characteristics[Characteristics.Evasion] == 100);
            Assert.AreEqual("gfkrf", Mage.ToString());
        }

        [Test]
        public void CorrectEnemyConstructorWithoutPistion()
        {
            var range = new EnemyHero(Helper.GetName(), new Dictionary<Characteristics, int>(), new Inventory(),
                Specialization.Archer, Location.SomeLocation);
            var melee = new EnemyHero(Helper.GetName(), new Dictionary<Characteristics, int>(), new Inventory(),
                Specialization.Warrior, Location.SomeLocation);
            Assert.AreEqual(Position.Melee, melee.Position);
            Assert.AreEqual(Position.Range, range.Position);
        }

        [Test]
        public void SkillUpgrade()
        {
            Assert.AreEqual(-10, Enemy1.Skills[1].Effect[0].value);
            Assert.AreEqual(10, Enemy1.Skills[1].ManaCost);
            Assert.AreEqual(Characteristics.Health, Enemy1.Skills[1].Effect[0].characteristic);
            Assert.AreEqual(-100, Enemy1.Skills[1].Buff._buffs[0].value);
            Assert.AreEqual(Characteristics.Initiative, Enemy1.Skills[1].Buff._buffs[0].characteristic);
            Enemy1.Skills[1].Upgrade();
            Assert.AreEqual(-12, Enemy1.Skills[1].Effect[0].value);
            Assert.AreEqual(Characteristics.Health, Enemy1.Skills[1].Effect[0].characteristic);
            Assert.AreEqual(-120, Enemy1.Skills[1].Buff._buffs[0].value);
            Assert.AreEqual(Characteristics.Initiative, Enemy1.Skills[1].Buff._buffs[0].characteristic);
            Assert.AreEqual(12, Enemy1.Skills[1].ManaCost);
        }


        [Test]
        public void UsingSkillByEnemyToEnemy()
        {
            Assert.AreEqual(Enemy2.Characteristics[Characteristics.Health], 100);
            Enemy1.UseSkill(Enemy1.Skills[1], Enemy2);
            Assert.AreEqual(Enemy2.Characteristics[Characteristics.Health], 90);
            Assert.AreEqual(Enemy2.Characteristics[Characteristics.Initiative], -70);
            Assert.IsNotEmpty(Enemy2.Buffs);
            Enemy2.Buffs.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Assert.AreEqual(Enemy2.Characteristics[Characteristics.Initiative], 30);
        }

        [Test]
        public void UsingSkillByEnemyToHero()
        {
            Enemy1.UseSkill(Enemy1.Skills[1], Archer);
            Assert.AreEqual(Archer.Characteristics[Characteristics.Health],
                Archer.StandardChars[Characteristics.Health] - 10);
            Assert.IsNotEmpty(Archer.Buffs);
            Assert.AreNotEqual(Archer.Characteristics[Characteristics.Initiative],
                Archer.StandardChars[Characteristics.Initiative]);
            Archer.Buffs.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Assert.AreEqual(Archer.Characteristics[Characteristics.Initiative],
                Archer.StandardChars[Characteristics.Initiative]);
        }

        [Test]
        public void UsingSkillByHeroToEnemy()
        {
            Mage.UseSkill(Mage.Skills[1], Enemy1);
            Assert.IsNotNull(Mage.Skills[1].Buff);
            Assert.True(Enemy1.Characteristics[Characteristics.Health] < 0);
            Assert.True(Enemy1.Buffs.Count > 0);
            Assert.True(Enemy1.Characteristics[Characteristics.Evasion] == 90);
            Enemy1.Buffs.RemoveAt(0);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Assert.True(Enemy1.Characteristics[Characteristics.Evasion] == 100);
        }

        [Test]
        public void UsingSkillByHeroToHero()
        {
            Mage.UseSkill(Mage.Skills[1], Archer);
            Assert.True(Archer.Characteristics[Characteristics.Health] < Archer.StandardChars[Characteristics.Health]);
            Assert.IsNotEmpty(Archer.Buffs);
            Archer.Buffs.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Assert.IsEmpty(Archer.Buffs);
            Assert.IsTrue(Archer.Characteristics[Characteristics.Evasion] ==
                          Archer.StandardChars[Characteristics.Evasion]);
        }
    }
}