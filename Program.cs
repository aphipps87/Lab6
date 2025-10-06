using System;

namespace Zork
{
    class Program
    {

        private static readonly Room[,] Rooms =
        {
            { new Room("Rocky Trail"), new Room("South of House"), new Room("Canyon View") },
            { new Room("Forest"),      new Room("West of House"),  new Room("Behind House") },
            { new Room("Dense Woods"), new Room("North of House"), new Room("Clearing") }
        };

     private static (int Row, int Column) Location = (1, 1);
     private static Room CurrentRoom => Rooms[Location.Row, Location.Column];


        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");
            InitializeRoomDescriptions();

            Room previousRoom = null;
            Commands command = Commands.UNKNOWN;

            while (command != Commands.QUIT)
            {
                Console.WriteLine(CurrentRoom); // prints room name via Room.ToString()

                // auto-look ONLY if room changed (incl. first turn because previousRoom is null)
                if (previousRoom != CurrentRoom)
                {
                    Console.WriteLine(CurrentRoom.Description);
                    previousRoom = CurrentRoom;
                }

                Console.Write("> ");
                command = ToCommand(Console.ReadLine()?.Trim() ?? string.Empty);

                switch (command)
                {
                    case Commands.LOOK:
                        Console.WriteLine(CurrentRoom.Description);
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        if (!Move(command))
                            Console.WriteLine("The way is shut!");
                        // movement is silent on success; next loop iteration will show the new room + auto-look
                        break;

                    case Commands.QUIT:
                        Console.WriteLine("Thank you for playing!");
                        break;

                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }
            }
        }

        private static Commands ToCommand(string s) =>
            Enum.TryParse(s, true, out Commands result) ? result : Commands.UNKNOWN;

        private static bool Move(Commands direction)
        {
            switch (direction)
            {
                case Commands.SOUTH when Location.Row < Rooms.GetLength(0) - 1 :
                    Location.Row++; return true;
                case Commands.NORTH when Location.Row > 0:
                    Location.Row--; return true;
                case Commands.WEST when Location.Column > 0:
                    Location.Column--; return true;
                case Commands.EAST when Location.Column < Rooms.GetLength(1) - 1:
                    Location.Column++; return true;
                default:
                    return false;
            }
        }

        private static void InitializeRoomDescriptions()
        {
            var roomMap = new Dictionary<string, Room>();
            foreach (Room room in Rooms)           // foreach loop for 2d array
                roomMap[room.Name] = room;         // create name + room map

            roomMap["Rocky Trail"].Description = "You are on a rock-strewn trail.";
            roomMap["South of House"].Description = "You are facing the south side of a white house. There is no door here, and all the windows are barred.";
            roomMap["Canyon View"].Description = "You are at the top of the Great Canyon on its south wall.";

            roomMap["Forest"].Description = "This is a forest, with trees in all directions around you.";
            roomMap["West of House"].Description = "This is an open field west of a white house, with a boarded front door.";
            roomMap["Behind House"].Description = "You are behind the white house. In one corner of the house there is a small window which is slightly ajar.";

            roomMap["Dense Woods"].Description = "This is a dimly lit forest, with large trees all around. To the east, there appears to be sunlight.";
            roomMap["North of House"].Description = "You are facing the north side of a white house. There is no door here, and all the windows are barred.";
            roomMap["Clearing"].Description = "You are in a clearing, with a forest surrounding you on the west and south.";
        }
    }
}
