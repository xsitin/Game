using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Game.Model
{
    public class Player:ISerializable
    {
        public int Gold;
        public List<Hero> Heroes;
        public List<Item> Storage;
        public List<Hero> Mercenaries;
        public List<Item> Shop;
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("porps",Gold);
            info.AddValue("porps",Heroes,typeof(List<Hero>));
        }
    }
}
