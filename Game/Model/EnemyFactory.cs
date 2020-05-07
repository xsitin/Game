using System;

namespace Game.Model
{
    public class EnemyFactory
    {
        public Location Location;
        public Team<Hero> Heroes;
        public EnemyFactory(Team<Hero> heroes, Location location)
        {
            Heroes = heroes;
            Location = location;
        }

        public Team<EnemyHero> GetEnemyTeam()
        {
            throw new Exception("TODO");
            return null;
        }
    }
}