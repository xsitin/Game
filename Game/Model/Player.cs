using System.Collections.Generic;

namespace Game.Model
{
    public class Player
    {
        public int Gold { get; set; }
        public string PlayerName { get; set; }
        public List<Hero> Heroes { get; set; }
        public List<Hero> Mercenaries { get; set; }
        public List<ActiveItem> Storage { get; set; }
        public Team<Hero> ActiveCommand { get; set; }
        public List<ActiveItem> Shop { get; set; }
        
        public Team<Hero> ActiveTeam { get; set; } = new Team<Hero>();
    }
}