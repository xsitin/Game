using System;
using System.Linq;
using System.Windows.Forms;

namespace GameCore.Model
{
    public class Game
    {
        private readonly EnemyFactory _enemyFactory;
        private int _counter;

        private Team<EnemyHero> _enemy;
        public (int exp, int money) Reward;

        public BasicCreature CurrentCreature;
        public Team<Hero> Heroes;
        public bool IsEnd;
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

        public Team<EnemyHero> Enemy
        {
            get => _enemy;
            set
            {
                _counter = 0;
                _enemy = value ?? throw new ArgumentNullException(nameof(value));
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
                MessageBox.Show("You lose!");
                IsEnd = true;
                Application.OpenForms["Main"]?.Controls["MainCntrl"].Refresh();
                return;
            }

            if (!Enemy.GetTeamList().Any())
            {
                var res = MessageBox.Show("Хотите продолжить?", "", MessageBoxButtons.YesNo);
                var heroes = Heroes.GetTeamList();
                var level = heroes.Sum(x => ((Hero) x).Level) / heroes.Count();
                Reward.exp += (int) Math.Round(100 + level * 100 + 300 * Math.Pow(level, 0.5)) / 3;
                Reward.money += level * 200;
                if (res == DialogResult.Yes)
                {
                    Enemy = _enemyFactory.GetEnemyTeam();
                    _counter++;
                    Application.OpenForms["Main"]?.Controls["MainCntrl"].Refresh();
                    return;
                }

                foreach (var h in Heroes.GetTeamList()) ((Hero) h).Exp += Reward.exp;
                IsEnd = true;
                Application.OpenForms["Main"]?.Controls["MainCntrl"].Refresh();
                return;
            }

            if (CurrentCreature is EnemyHero && CurrentCreature.Characteristics[Characteristics.Health] > 0)
            {
                Bot.MakeAMove(this);
                NextStep();
            }

            //Todo give step to player
        }
    }
}