using System;
using System.Linq;
using System.Threading;

namespace Slowshooter
{
    internal class Program
    {

        static string playField = 
@"         +---+
         |   |
         |   |
         |   |
         +---+";

        static bool isPlaying = true;

        // player input 
        static int p1_x_input;
        static int p1_y_input;

        static int p2_x_input;
        static int p2_y_input;

        // player 1 pos
        static int p1_x_pos = 2;
        static int p1_y_pos = 2;

        // player 2 pos
        static int p2_x_pos = 10;
        static int p2_y_pos = 2;

        // bounds for player movement
        static (int, int) p1_min_max_x = (1, 3);
        static (int, int) p1_min_max_y = (1, 3);
        static (int, int) p2_min_max_x = (9, 11);
        static (int, int) p2_min_max_y = (1, 3);

        //steals
        static bool playerOneSteal = true;
        static bool playerTwoSteal = true;

        // what turn is it? will be 0 after game is drawn the first time
        static int turn = 0;

        // contains the keys that player 1 and player 2 are allowed to press
        static char[] allKeybindings = new char[]{'Q', 'W', 'E', 'A', 'S', 'D', 'Z', 'X', 'C', 'U', 'J', 'O', 'I', 'L', 'K', 'M', ',', '.' };
        static char[] playerOneKeys = new char[] { 'Q', 'W', 'E', 'A', 'S', 'D', 'Z', 'X', 'C' };
        static char[] playerTwoKeys = new char[] { 'U', 'J', 'O', 'I', 'L', 'K', 'M', ',', '.' };
        static ConsoleColor[] playerColors = { ConsoleColor.Red, ConsoleColor.Blue };

        static int[,] ticTacToArray = {
            {0, 0, 0},
            {0, 0, 0},
            {0, 0, 0}
        };

        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            while(isPlaying)
            {
                Draw();
                ProcessInput();
                Update();
                Draw();
                int winner = CheckForWin();
                if (winner == 1)
                {
                    isPlaying = false;
                    Console.WriteLine("Player 1 Wins!!!");
                    break;
                }
                else if (winner == 2) 
                {
                    isPlaying = false;
                    Console.WriteLine("Player 2 Wins!!!");
                    break;
                }
                else if (winner == -1)
                {
                    isPlaying = false;
                    Console.WriteLine("You guys suck its a tie!!!!!");
                    break;
                }
                Draw();
            }
        }

        static int CheckForWin() 
        {
            int winner = 0;

            // check for if game is over
            bool anyEmptySpots = false;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (ticTacToArray[row, col] == 0) anyEmptySpots = true;
                }
            }

            if (!anyEmptySpots) winner = -1;

            // check rows
            for (int i = 0; i < 3; i++) 
            {
                if (ticTacToArray[i, 0] == ticTacToArray[i, 1] && ticTacToArray[i, 1] == ticTacToArray[i, 2] && ticTacToArray[i, 0] != 0) 
                {
                    winner = ticTacToArray[i, 0];
                }
            }
            // check columns
            for (int i = 0; i < 3; i++)
            {
                if (ticTacToArray[0, i] == ticTacToArray[1, i] && ticTacToArray[1, i] == ticTacToArray[2, i] && ticTacToArray[0, i] != 0)
                {
                    winner = ticTacToArray[0, i];
                }
            }

            // downwards diagonal
            if (ticTacToArray[0, 0] == ticTacToArray[1, 1] && ticTacToArray[1, 1] == ticTacToArray[2, 2] && ticTacToArray[0, 0] != 0) 
            {
                winner = ticTacToArray[0, 0];
            }
            // upwards diagonal
            if (ticTacToArray[2, 0] == ticTacToArray[1, 1] && ticTacToArray[1, 1] == ticTacToArray[0, 2] && ticTacToArray[1, 1] != 0)
            {
                winner = ticTacToArray[1, 1];
            }

            

            return winner;
        }

        static void ProcessInput()
        {
            // if this isn't here, input will block the game before drawing for the first time
            if (turn == -1) return;

            // reset input
            p1_x_input = 0;
            p1_y_input = 0;
            p2_x_input = 0;
            p2_y_input = 0;

            char[] allowedKeysThisTurn; // different keys allowed on p1 vs. p2 turn

            // choose which keybindings to use
            if (turn % 2 == 0) allowedKeysThisTurn = playerOneKeys;
            else allowedKeysThisTurn = playerTwoKeys;

            // get the current player's input
            char input = ' ';
            while (!allowedKeysThisTurn.Contains(input))
            {
                input = Char.ToUpper(Console.ReadKey(true).KeyChar);
                //Console.WriteLine(input);
            }

            // check all input keys 
            if (input == 'Q') 
            {
                p1_x_input = 0;
                p1_y_input = 0;
            }
            if (input == 'A') 
            {
                p1_x_input = 0;
                p1_y_input = 1;
            }
            if (input == 'E')
            {
                p1_x_input = 2;
                p1_y_input = 0;
            }
            if (input == 'D')
            {
                p1_x_input = 2;
                p1_y_input = 1;
            }
            if (input == 'W')
            {
                p1_y_input = 0;
                p1_x_input = 1;
            }
            if (input == 'S')
            {
                p1_y_input = 1;
                p1_x_input = 1;
            }
            if (input == 'Z')
            {
                p1_x_input = 0;
                p1_y_input = 2;
            }
            if (input == 'X')
            {
                p1_x_input = 1;
                p1_y_input = 2;
            }
            if (input == 'C')
            {
                p1_x_input = 2;
                p1_y_input = 2;
            }

            //player two
            if (input == 'U')
            {
                p2_x_input = 0;
                p2_y_input = 0;
            }
            if (input == 'J')
            {
                p2_x_input = 0;
                p2_y_input = 1;
            }
            if (input == 'O')
            {
                p2_x_input = 2;
                p2_y_input = 0;
            }
            if (input == 'L')
            {
                p2_x_input = 2;
                p2_y_input = 1;
            }
            if (input == 'I')
            {
                p2_y_input = 0;
                p2_x_input = 1;
            }
            if (input == 'K')
            {
                p2_y_input = 1;
                p2_x_input = 1;
            }
            if (input == 'M')
            {
                p2_x_input = 0;
                p2_y_input = 2;
            }
            if (input == ',')
            {
                p2_x_input = 1;
                p2_y_input = 2;
            }
            if (input == '.')
            {
                p2_x_input = 2;
                p2_y_input = 2;
            }

        }

        static void Update()
        {
            if (turn % 2 == 0)
            {
                if (ticTacToArray[p1_y_input, p1_x_input] == 2)
                {
                    if (playerOneSteal)
                    {
                        playerOneSteal = false;
                        ticTacToArray[p1_y_input, p1_x_input] = 1;
                    }
                }
                else 
                {
                    ticTacToArray[p1_y_input, p1_x_input] = 1;
                }
                   
            }
            else 
            {
                if (ticTacToArray[p2_y_input, p2_x_input] == 1)
                {
                    if (playerTwoSteal)
                    {
                        playerTwoSteal = false;
                        ticTacToArray[p2_y_input, p2_x_input] = 2;
                    }
                }
                else
                {
                    ticTacToArray[p2_y_input, p2_x_input] = 2;
                }
            }

            turn += 1;

        }

        static void Draw()
        {
            // draw the background (playfield)
            Console.SetCursorPosition(0, 0);
            Console.Write(playField);

            // draw the tic tac toe inputs
            for (int row = 0; row < 3; row++) 
            {
                for (int col = 0; col < 3; col++) 
                {
                    Console.SetCursorPosition(col+10, row+1);
                    if (ticTacToArray[row, col] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        
                        Console.Write('X');
                    }
                    else if (ticTacToArray[row, col] == 2) 
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write('O');
                    }
                }
            }

            // draw the Turn Indicator
            Console.SetCursorPosition(3, 5);
            Console.ForegroundColor = playerColors[turn % 2];

            Console.Write($"PLAYER {turn % 2 + 1}'S TURN!");


            Console.ForegroundColor = ConsoleColor.DarkGray;
            if (turn % 2 == 0)
            {
                Console.WriteLine("\n          QWE");
                Console.WriteLine("          ASD");
                Console.WriteLine("          ZXC");
                if (playerOneSteal)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You have one steal available!");
                }
                else 
                {
                    Console.WriteLine("                                   ");
                }
            }
            else
            {
                Console.WriteLine("\n          UIO");
                Console.WriteLine("          JKL");
                Console.WriteLine("          M,.");
                if (playerTwoSteal)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("You have one steal available!");
                }
                else
                {
                    Console.WriteLine("                                   ");
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
