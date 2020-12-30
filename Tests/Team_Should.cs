using System;
using System.Collections.Generic;
using System.Linq;
using Game.Model;
using Moq;
using NUnit.Framework;

namespace Tests
{
    public class TeamShould
    {
        private readonly Mock<Hero> _heroMock = new Mock<Hero>();
        private readonly Mock<BasicCreature> _basicMock = new Mock<BasicCreature>();
        [Test]
        public void Test_Exception_OnEmptyList()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new Team<BasicCreature>(new List<BasicCreature>(), new List<BasicCreature>())
            );
            Assert.AreEqual("Lines shouldn't be empty!", exception.Message);
        }

        [Test]
        public void Test_SecondLineIsEmpty()
        {
            
            var warriors = new List<Hero> {_heroMock.Object, _heroMock.Object};
            var team = new Team<Hero>(warriors, new List<Hero>());
            Assert.AreEqual(warriors, team.GetTeamList());
        }

        [Test]
         public void Test_LinesAreEquals()
         {
             var warriors = new List<Hero> {_heroMock.Object, _heroMock.Object};
             var exception = Assert.Throws<ArgumentException>(
                 () => new Team<Hero>(warriors, warriors)
             );
             Assert.AreEqual("Lines shouldn't be equals!", exception.Message);
         }
        
        [Test]
        public void Test_WellHeroesSquads()
        {
            var archers = new List<Hero> {_heroMock.Object, _heroMock.Object};
            var warriors = new List<Hero> {_heroMock.Object, _heroMock.Object};
            var heroesTeam = new Team<Hero>(warriors, archers);
            Assert.AreEqual(heroesTeam.GetTeamList(), warriors.Concat(archers).ToList());
        }
        
        [Test]
        public void Test_WellEnemiesSquads()
        {
            
            var warriors = new List<BasicCreature> {_basicMock.Object, _basicMock.Object};
            var wizards = new List<BasicCreature> {_basicMock.Object, _basicMock.Object};
            var team = new Team<BasicCreature>(warriors, wizards);
            Assert.AreEqual(team.GetTeamList(), warriors.Concat(wizards).ToList());
        }
        
        [Test]
        public void Test_MakeStepForward_FirstLineIsEmptyAndSecondLineNotEmpty()
        {
            var archers = new List<Hero> {_heroMock.Object, _heroMock.Object};
            var team = new Team<Hero>(new List<Hero>(), archers);
            Assert.AreEqual(archers, team.FirstLine);
        }
        
        [Test]
        public void Test_MakeStepForward_LinesNotEmpty()
        {
            var archers = new List<Hero> {_heroMock.Object, _heroMock.Object};
            var warriors = new List<Hero> {_heroMock.Object, _heroMock.Object};
            var team = new Team<Hero>(warriors, archers);

            Assert.AreEqual(warriors, team.FirstLine);
            Assert.AreEqual(archers, team.SecondLine);
        }
    }
}