namespace GameCore.Model
{
    public class Field
    {
        private Location _location;
        public Team<EnemyHero> Enemy;
        public Team<Hero> Heroes;

        public Field(Location location, Team<Hero> heroes)
        {
            //TODO add debuff on location for some heroes
            _location = location;
            Heroes = heroes;
        }
    }
}