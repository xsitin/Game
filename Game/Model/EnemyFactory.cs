using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Game.Model
{
    public class EnemyFactory
    {
        public Location Location;
        public Team<Hero> Heroes;
        public int Counter;
        private Queue<int> last = new Queue<int>(6);
        private Random _random = new Random();

        public EnemyFactory(Team<Hero> heroes, Location location)
        {
            Heroes = heroes;
            Location = location;
            Counter = 1;
        }

        public Team<EnemyHero> GetEnemyTeam()
        {
            var listHero = Heroes.GetTeamList();
            var minLevel = listHero.Min(x => ((Hero) x).Level) + Counter / 4;
            var maxLevel = listHero.Max(x => ((Hero) x).Level) + Counter / 4;
            var heroCount = listHero.Count + Counter > 8 ? 8 : listHero.Count + Counter;
            var enemyCount = GetRandom(Counter >= heroCount ? heroCount - 2 : Counter, heroCount);
            var firstLine = new List<EnemyHero>();
            var secondLine = new List<EnemyHero>();
            for (var i = 0; i < enemyCount; i++)
            {
                var enemy = GetRandomEnemy(minLevel, maxLevel);
                if (enemy.Position == Position.Range && secondLine.Count < 4)
                    secondLine.Add(enemy);
                else
                    firstLine.Add(enemy);
            }

            if (!firstLine.Any())
            {
                firstLine.Add(GetEnemy(minLevel, maxLevel, Specialization.Warrior));
            }

            Counter++;
            return new Team<EnemyHero>(firstLine, secondLine);
        }

        private EnemyHero GetRandomEnemy(int minLevel, int maxLevel)
        {
            var specialization = (Specialization) GetRandom(0, Enum.GetValues(typeof(Specialization)).Length - 1);
            return GetEnemy(maxLevel, maxLevel, specialization);
        }

        private EnemyHero GetEnemy(int minLevel, int maxLevel, Specialization specialization)
        {
            var random = new Random();
            var enemy = new EnemyHero(Helper.GetName(), new Dictionary<Characteristics, int>(), new List<ActiveItem>(),
                specialization, Location);
            var points = GetRandom(minLevel, maxLevel) - 1;
            while (enemy.Skills.Count < 3)
            {
                var skill = Helper.BasicSkills[specialization][
                    GetRandom(0, Helper.BasicSkills[specialization].Count)];
                if (enemy.Skills.All(x => x.Name != skill.Name))
                    enemy.Skills.Add(skill);
            }

            for (var j = 0; j < points; j++)
            {
                if (random.Next(0, 2) == 1)
                {
                    var character =
                        (Characteristics) GetRandom(0, Enum.GetValues(typeof(Characteristics)).Length - 1);
                    if (((character == Characteristics.Evasion || character == Characteristics.MagicalProtection ||
                          character == Characteristics.PhysicalProtection) &&
                         (int) (enemy.Characteristics[character] * 1.2) > 100) ||
                        (character == Characteristics.PhysicalProtection &&
                         enemy.Specialization == Specialization.Wizard))
                    {
                        j--;
                        continue;
                    }

                    enemy.Characteristics[character] = (int) (enemy.Characteristics[character] * 1.2);
                }
                else
                    enemy.Skills[GetRandom(0, enemy.Skills.Count - 1)].Upgrade();
            }

            return enemy;
        }

        private int GetRandom(int from, int to)
        {
            if (from - to < 2 || Enumerable.Range(from, to).All(x => last.Contains(x)))
                return _random.Next(from, to);
            var res = _random.Next(from, to);
            while (last.Contains(res))
                res = _random.Next(from, to);
            last.Enqueue(res);
            if (last.Count > 3) last.Dequeue();
            return res;
        }
    }
}