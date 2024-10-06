using System;

namespace Zork
{
    class Program()
    {
        private static string[] Rooms = new string[]
        {
            "Forest",
            "West of House",
            "Behind House",
            "Clearing",
            "Canyon View"

            // {"Rocky Trail", "South of House", "Canyon View"},
            // {"Forest", "West of House", "Behind House"},
            // {"Dense Woods", "North of House", "Clearing"}
        };        

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");
            int currentRoomIndex = 1;
            Commands command = Commands.UNKNOWN;
            while (command != Commands.QUIT) 
            {
                Console.Write("> ");
                command = ToCommand(Console.ReadLine().Trim());

                string outputString;
                switch (command)
                {
                    case Commands.QUIT:
                        outputString = "Thank you for playing!";
                        break;

                    case Commands.LOOK:
                        outputString = "This is an open field west of a white house, with a boarded front door.\nA rubber mat saying 'Welcome to Zork!' lies by the door.";
                        break;

                    case Commands.NORTH:                        
                    case Commands.SOUTH:
                        outputString = $"The way is shut!";
                        break;
                    case Commands.EAST:
                        Move(command);
                        if (Move(command) == true)
                        {
                            currentRoomIndex++;
                            outputString = $"You moved {command}.";
                        }
                        else
                        {
                            outputString = $"The way is shut!";
                        }
                        break;
                    case Commands.WEST:
                        Move(command);
                        if (Move(command) == true)
                        {
                            currentRoomIndex--;
                            outputString = $"You moved {command}.";
                        }
                        else 
                        {
                            outputString = $"The way is shut!";
                        }
                        break;

                    default:
                        outputString = "Unknown command.";
                        break;                    
                }

                Console.WriteLine(outputString + $"\n{Rooms[currentRoomIndex]}");
            }            
        }

        static bool Move(Commands moveCommand) 
        {
            bool moveSucceeds;
            int roomIndex = 1;

            switch (moveCommand)
            {
                case Commands.NORTH:
                case Commands.SOUTH:
                    moveSucceeds = false;
                    break;
                case Commands.EAST:
                    if (roomIndex >= Rooms.Length) 
                    {
                        moveSucceeds = false;
                    }
                    else
                    {                        
                        moveSucceeds = true;
                        roomIndex++;
                    }
                    break;
                case Commands.WEST:
                    if (roomIndex < 0)
                    {
                        moveSucceeds = false;
                    }
                    else 
                    {
                        moveSucceeds = true;
                        roomIndex--;
                    }
                    break;
                default:
                    moveSucceeds = false;
                    break;
            }
            return moveSucceeds;
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;        
    }
}
