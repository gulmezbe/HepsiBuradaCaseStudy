using System;
using System.Threading;

namespace HepsiBuradaCaseStudy
{
    class Program
    {
        public const String Left = "L";
        public const String Right = "R";
        public const String Move = "M";
        public const String West = "W";
        public const String North = "N";
        public const String East = "E";
        public const String South = "S";

        public static void RoverThread(Object inputString)
        {
            String[] directions = { "W", "N", "E", "S" };               //List of directions

            String input = (String) inputString;
            String[] inputList = input.Split("/");                      //Splitting parameter to rectangle size, location and movements 

            String plateauSize = inputList[0];                          //Rectangle size
            String[] plateauCoords = plateauSize.Split(" ");
            int xMax = Convert.ToInt32(plateauCoords[0]);
            int yMax = Convert.ToInt32(plateauCoords[1]);

            String startLocation = inputList[1];                        //Location and direction
            String[] startCoords = startLocation.Split(" ");
            int x = Convert.ToInt32(startCoords[0]);
            int y = Convert.ToInt32(startCoords[1]);
            String direction = startCoords[2];
            int directionIndex = Array.IndexOf(directions, direction);

            String movements = inputList[2];                            //Movements
            
            String roverNo = inputList[3];                              //Rover Number

            while (movements.Length > 0)                                //Loop until movements are over
            {
                String movement = movements.Substring(0, 1);

                if (movement.Equals(Left) || movement.Equals(Right))    //Change direction
                {
                    directionIndex = GetDirection(movement, directionIndex);
                    direction = directions[directionIndex];
                }
                else if (movement.Equals(Move))                         //Move
                {
                    var newLocation = MoveRover(x, y, direction);
                    x = newLocation.Item1;
                    y = newLocation.Item2;

                    if(!LocationValidation(x, y, xMax, yMax))           //Is current location in borders
                    {
                        Console.WriteLine("Rover{0} went out of bounds.", roverNo);
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect data for Rover{0}", roverNo);
                    return;
                }

                movements = movements.Substring(1);
            }

            String result = x.ToString() + " " + y.ToString() + " " + direction;

            Console.WriteLine("Rover{0}: {1}", roverNo, result);
        }

        public static int GetDirection(String movement, int directionIndex)
        {
            if (movement.Equals(Left))              //Direction index will be used for direction info
                directionIndex -= 1;
            else if (movement.Equals(Right))
                directionIndex += 1;

            if (directionIndex == -1)               //Check for index borders
                directionIndex = 3;
            else if (directionIndex == 4)
                directionIndex = 0;

            return directionIndex;
        }

        public static Tuple<int, int> MoveRover(int x, int y, String direction)
        {
            if (direction.Equals(West))                     //Movement in the desired direction
                x -= 1;
            else if (direction.Equals(North))
                y += 1;
            else if (direction.Equals(East))
                x += 1;
            else if (direction.Equals(South))
                y -= 1;

            return Tuple.Create(x, y);
        }

        public static Boolean LocationValidation(int x, int y, int xMax, int yMax)
        {
            if (x >= 0 && x <= xMax && y >= 0 && y <= yMax)             //Is current location in borders
                return true;
            else
                return false;
        }

        static void Main(string[] args)
        {
            /*
            String input1 = Console.ReadLine();                 //Inputs from user
            String input2 = Console.ReadLine();                 //Open here to test
            String input3 = Console.ReadLine();
            String input4 = Console.ReadLine();
            String input5 = Console.ReadLine();
            */
            
            String input1 = "5 5";                              //Example inputs
            String input2 = "1 2 N";
            String input3 = "LMLMLMLMM";
            String input4 = "3 3 E";
            String input5 = "MMRMMRMRRM";
            
            String rover1Data = input1 + "/" + input2 + "/" + input3 + "/1";       //User input for first rover
            String rover2Data = input1 + "/" + input4 + "/" + input5 + "/2";       //User input for second rover

            Thread rover1 = new Thread(RoverThread);
            Thread rover2 = new Thread(RoverThread);

            rover1.Start(rover1Data);
            rover2.Start(rover2Data);
        }
    }
}
