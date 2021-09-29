using System;

namespace _1lab_8queens_ids
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] initialBoard = new int[Board.boardRows, Board.boardColumns];
            Board.setManualBoardState(initialBoard);

            Console.WriteLine("Initial board state: ");
            Board.showBoard(initialBoard);

            int conflictsByState = Problem.conflictsCount(initialBoard);
            Console.WriteLine($"Number of conflicts is {conflictsByState}.");
            Console.WriteLine();

            Tree tree = new Tree();
            tree.root = new Node(initialBoard, null, 0);

            Console.WriteLine("Recursive best first search:");
            var result2 = SearchAlgorithms.RecursiveBestFirstSearch(tree.root, int.MaxValue);

            if (result2 != null)
            {
                Board.showBoard(result2.State);

                Console.WriteLine($"\nDepth of a goal state is: {result2.Depth}.");
                Console.WriteLine($"Generated states: {TaskCounters.generatedStatesCounterRBFS}\n" +
                    $"Num of iterations: {TaskCounters.iterationsCounterRBFS}");
            }
            else
            {
                Console.WriteLine("Solution is not found");
            }

            Console.WriteLine("\n\n");

            Console.WriteLine("Iterative deepening search:");
            var result = SearchAlgorithms.IterativeDeepeningSearch(tree.root);

            if (result != null)
            {
                Board.showBoard(result.State);

                Console.WriteLine($"\nDepth of a goal state is: {result.Depth}.");
                Console.WriteLine($"Generated states: {TaskCounters.generatedStatesCounterIDS}\n" +
                    $"Num of iterations: {TaskCounters.iterationsCounterIDS}");
            }
            else
            {
                Console.WriteLine("Solution is not found");
            }

            #region some tests
            /*
            Tree tree = new Tree();
            tree.root = new Node(initialBoard, null, 0);

            var result = SearchAlgorithms.IterativeDeepeningSearch(tree.root);

            if (result != null)
            {
                Board.showBoard(result.State);
                Console.WriteLine($"Depth is: {result.Depth}.");
            }
            else
            {
                Console.WriteLine("Solution is not found");
            }*/

            //Проверка состояния работает корректно
            /*
            int[,] goalBoard = new int[Board.boardRows, Board.boardColumns];
            Board.setGoalBoardState(goalBoard);
            Board.showBoard(goalBoard);
            Console.WriteLine(Problem.isAGoal(goalBoard));*/

            /*
            //IF A GOAL CHECK
            bool isGoal = Problem.isAGoal(firstBoard);
            Console.WriteLine(isGoal);

            //GENERATING NEW STATES FROM A GIVEN BOARD
            List<int[,]> newStates = Board.getNewStates(firstBoard);

            //Tree implementation
            Tree tree = new Tree();

            tree.root = new Node(firstBoard, null , 0);
            Board.showBoard(tree.root.State);
            tree.root.children  = Expand(tree.root);

            int count = 0;
            foreach (var item in tree.root.children)
            {
                Board.showBoard(item.State);
                Console.WriteLine();
                Console.WriteLine(Problem.isAGoal(item.State));
                Console.WriteLine(item.Depth);
                Console.WriteLine();
                count++;
            }
            Console.WriteLine();
            Console.WriteLine(count);
            
            tree.root.AddChild(new Node(newStates[0], tree.root, 1));
            tree.root.AddChild(new Node(newStates[1], tree.root, 1));
            Board.showBoard(tree.root.children[0].State);
            Board.showBoard(tree.root.children[1].State);
            
            List<int[,]> newStates2 = Board.getNewStates(newStates[0]);

            tree.root.children[0].AddChild(new Node(newStates2[0], tree.root.children[0], 2));
            Console.WriteLine();
            Board.showBoard(tree.root.children[0].children[0].State);
            Console.WriteLine($"Depth is {tree.root.children[0].children[0].Depth}");*/
            #endregion 
        }
    }
}
