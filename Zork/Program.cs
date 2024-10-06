using System;

namespace Zork
{
    class Program()
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Commands command = Commands.UNKNOWN;
            while (command != Commands.QUIT) 
            {
                Console.Write("> ");
                command = ToCommand(Console.ReadLine().Trim());

                switch (command)
                {
                    case Commands.LOOK:
                        Console.WriteLine("A rubber mat saying 'Welcome to Zork!' lies by the door.");
                        break;
                    case Commands.NORTH:
                        Console.WriteLine("You moved " + command.ToString());
                        break;
                    case Commands.SOUTH:
                        Console.WriteLine("You moved " + command.ToString());
                        break;
                    case Commands.EAST:
                        Console.WriteLine("You moved " + command.ToString());
                        break;
                    case Commands.WEST:
                        Console.WriteLine("You moved " + command.ToString());
                        break;
                    case Commands.UNKNOWN:
                        Console.WriteLine("Unknown command.");
                        break;
                    case Commands.QUIT:
                        Console.WriteLine("Thank you for playing! \nPress any key to continue. . .");
                        break;
                }

            }

            Console.WriteLine(command);
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;        
    }
}
