using System;
using System.Linq;
using System.Windows.Forms;

namespace Game.Model
{
    public class Game
    {
        private readonly EnemyFactory _enemyFactory;
        private int _counter;
        public bool IsEnd;
        public (int exp, int money) _reward;

        public BasicCreature CurrentCreature;
        public Team<Hero> Heroes;
        public Location Location;
        public GameQueue Queue;

        public Game(Team<Hero> heroes, Location location)
        {
            Heroes = heroes;
            Location = location;
            _enemyFactory = new EnemyFactory(heroes, Location);
            Enemy = _enemyFactory.GetEnemyTeam();
            Queue = new GameQueue(Heroes.GetTeamList().Concat(Enemy.GetTeamList()).ToList());
            _counter = 0;
        }

        private Team<EnemyHero> _enemy;
        public Team<EnemyHero> Enemy
        {
            get => _enemy;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _counter = 0;
                _enemy = value;
                Queue = new GameQueue(Heroes.GetTeamList().Concat(_enemy.GetTeamList()).ToList());
            }
        }

        public void NextStep()
        {
            Heroes.Update();
            Enemy.Update();
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

            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (!Heroes.GetTeamList().Any())
            {
                //console.log('poshel naxui');
            }

            if (!Enemy.GetTeamList().Any())
            {
                var res = MessageBox.Show("Хотите продолжить?");
                var heroes = Heroes.GetTeamList();
                var level = heroes.Sum(x => (x as Hero).Level) / heroes.Count();
                _reward.exp += (int) Math.Round(100 + level * 100 + 300 * Math.Pow(level, 0.5)) / 4;
                _reward.money += level * 200;
                if (res == DialogResult.Yes)
                {
                    Enemy = _enemyFactory.GetEnemyTeam();
                    return;
                }

                foreach (var h in Heroes.GetTeamList()) ((Hero) h).Exp += _reward.exp;
                IsEnd = true;
            }

            if ((CurrentCreature is EnemyHero) && CurrentCreature.Characteristics[Characteristics.Health] > 0)
                Bot.MakeAMove(this);
            //Todo give step to player
        }
    }
}