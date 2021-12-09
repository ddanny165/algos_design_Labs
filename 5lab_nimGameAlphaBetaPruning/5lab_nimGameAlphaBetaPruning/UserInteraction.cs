using System;

namespace _5lab_nimGameAlphaBetaPruning
{
    public static class UserInteraction
    {
        public static void InteractWithUser()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Welcome to the NIM game!\nCreated by Danylo Moskaliuk as 5th lab for Algorithms Design.");
            Console.WriteLine("\n\nRules: \n\n1. Your goal is to avoid taking the last item.\n2. There are four heaps, " +
                "you can take any amount of items \n   (1 is minimum) from only a single heap per move.\n\nAs you see, it's pretty simple!" +
                "\nGood luck!");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n\nType in '1' to start playing: ");
            int UserInput = 0;

            try
            {
                UserInput = int.Parse(Console.ReadLine());
            }
            catch (FormatException) 
            {
                Console.WriteLine("\n\nTry again!\n");
                InteractWithUser();
            }

            if (UserInput != 1) 
            {
                Console.WriteLine("\n\nTry again!\n");
                InteractWithUser();
            }

            Console.Write("\nDefine the difficulty [easy|medium|hard]: ");
            string UserLevelInput = Console.ReadLine();

            if (UserLevelInput != "easy" && UserLevelInput != "medium" && UserLevelInput != "hard") 
            {
                Console.WriteLine("\n\nYou must define a valid name of a difficulty!\n\n");
                InteractWithUser();
            }
            Console.ResetColor();

            Algorithms.NimGame(UserLevelInput);
        }

    }
}
