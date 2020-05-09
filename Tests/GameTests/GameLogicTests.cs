using System.Collections.Generic;
using Game.Model;
using NUnit.Framework;
using System.Linq;




namespace Tests.GameTests
{
    public class GameLogicTests
    {
        [TestFixture]
        
        public class GameQueueTests
        {
            
            
            
            [Test]
            public void Initialization()
            {
                var queue = new GameQueue(TestData.EnemyTeam.GetTeamList().Concat(TestData.TeamHerous.GetTeamList()).ToList());
                Assert.AreEqual(1,queue.Queue[0].Characteristics[Characteristics.Initiative]);
                Assert.AreEqual(2,queue.Queue[1].Characteristics[Characteristics.Initiative]);
                Assert.AreEqual(3,queue.Queue[2].Characteristics[Characteristics.Initiative]);
                Assert.AreEqual(3,queue.Queue[3].Characteristics[Characteristics.Initiative]);
                Assert.AreEqual(10,queue.Queue[4].Characteristics[Characteristics.Initiative]);
            }
            
        }
    }

    public class TestData
    {
        public static Hero Hero1  = new Hero("Artem", new Dictionary<Characteristics, int>()
        {
            {Characteristics.Health, 10},
            {Characteristics.Initiative, 1}
        }, new Inventory(), Specialization.Warrior, Position.Melee, Location.None);
        
        public static Hero Hero2  = new Hero("NeArtem", new Dictionary<Characteristics, int>()
        {
            {Characteristics.Health, 10},
            {Characteristics.Initiative, 2}
        }, new Inventory(), Specialization.Archer, Position.Range, Location.None);
        
        public static Hero Hero3 = new Hero("NeNeArtem", new Dictionary<Characteristics, int>()
        {
            {Characteristics.Health, 10},
            {Characteristics.Initiative, 4}
        }, new Inventory(), Specialization.Warrior, Position.Melee, Location.None);
        
        public static Hero Hero4  = new Hero("NeNeNeArtem", new Dictionary<Characteristics, int>()
        {
            {Characteristics.Health, 10},
            {Characteristics.Initiative, 5}
        }, new Inventory(), Specialization.Archer, Position.Range, Location.None);
        
        public static Team<Hero> TeamHerous = new Team<Hero>(new List<Hero>() { Hero1 }, new List<Hero>(){Hero2} );
        
        public static EnemyHero Enemy1  = new EnemyHero("Sanuia", new Dictionary<Characteristics, int>()
        {
            {Characteristics.Health, 10},
            {Characteristics.Initiative, 3}
        }, new Inventory(), EnemySpecialization.Warrior, Position.Melee, Location.None);
        
        public static EnemyHero Enemy2 = new EnemyHero("Sania1", new Dictionary<Characteristics, int>()
        {
            {Characteristics.Health, 10},
            {Characteristics.Initiative, 3}
        }, new Inventory(), EnemySpecialization.Wizard, Position.Range, Location.None);
        
        public static EnemyHero Enemy3 = new EnemyHero("Sania2", new Dictionary<Characteristics, int>()
        {
            {Characteristics.Health, 10},
            {Characteristics.Initiative, 10}
        }, new Inventory(), EnemySpecialization.Wizard, Position.Range, Location.None);
        
        public static Team<EnemyHero> EnemyTeam = new Team<EnemyHero>(new List<EnemyHero>() {Enemy1},new List<EnemyHero>() {Enemy2,Enemy3});
    }
}