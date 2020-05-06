using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Model
{
    public class Game
    {
        public Team<Hero> Heroes;
        public GameQueue Queue;
        public Team<EnemyHero> Enemy
        {
            get => Enemy;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _counter = 0;
                Queue = new GameQueue(Heroes.GetTeamList().Concat(Enemy.GetTeamList()).ToList());
            }
        }

        private EnemyFactory _enemyFactory;
        public Location Location;

        public BasicCreature CurrentCreature;
        private int _counter;
        public Game(Team<Hero> heroes, Location location)
        {
            Heroes = heroes;
            Location = location;
            _enemyFactory = new EnemyFactory(heroes,Location);
            Enemy = _enemyFactory.GetEnemyTeam();
            Queue = new GameQueue(Heroes.GetTeamList().Concat(Enemy.GetTeamList()).ToList());
            _counter = 0;
        }

        public void NextStep()
        {
            Queue.Update();
            CurrentCreature = Queue.GetNextPerson();
            foreach (var hero in Heroes.GetTeamList())
                for (var i = 0; i < hero.Buffs.Count; i++)
                    if (!(hero.Buffs[i] is null))
                    {
                        hero.Buffs[i].Duration--;
                        if (hero.Buffs[i].Duration <= 0)
                            hero.Buffs.Remove(hero.Buffs[i]);
                    }
            foreach (var enemy in Enemy.GetTeamList())
                for (var i = 0; i < enemy.Buffs.Count; i++)
                    if (!(enemy.Buffs[i] is null))
                    {
                        enemy.Buffs[i].Duration--;
                        if (enemy.Buffs[i].Duration <= 0)
                            enemy.Buffs.Remove(enemy.Buffs[i]);
                    }
        }
    }
}