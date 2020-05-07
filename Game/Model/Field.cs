using System.Collections.Generic;

namespace Game.Model
{
    public class Field
    {
        private Location _location;
        public Team<Hero> Heroes;
        public Team<EnemyHero> Enemy;

        public Field(Location location, Team<Hero> heroes)
        {//TODO add debuff on location for some heroes
            _location = location;
            Heroes = heroes;
        }
    }
}