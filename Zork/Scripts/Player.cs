using System.Collections.Generic;
using Newtonsoft.Json;

namespace Zork.Scripts
{
    public class Player
    {
        public World World { get; }
        public int Moves { get; set; }
        public Room Location { get; private set; }

        [JsonIgnore]
        public string LocationName
        {
            get
            {
                return Location?.Name;
            }
            set
            {
                Location = World?.RoomsByName.GetValueOrDefault(value);
            }
        }

        public Player(World world, string startingLocation)
        {
            World = world;
            LocationName = startingLocation;
        }

        public bool Move(Directions direction)
        {
            bool isValidMove = Location.Neighbors.TryGetValue(direction, out Room destination);
            if (isValidMove)
            {
                Location = destination;
            }

            return isValidMove;
        }
    }
    public enum Directions
    {
        North,
        South,
        East,
        West
    }

}
