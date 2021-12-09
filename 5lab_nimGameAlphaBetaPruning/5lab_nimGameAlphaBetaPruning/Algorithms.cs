using System;
using System.Collections.Generic;

namespace _5lab_nimGameAlphaBetaPruning
{
    public static class Algorithms
    {
        public static void NimGame(string Difficulty)
        {
            int[] CurrentState;
            bool IsComputersMove;
            bool GameIsFinished = false;

            Console.WriteLine("\nThere are four heaps below:");
            List<int> firstHeap = new() { 1 };
            List<int> secondHeap = new() { 1, 1, 1 };
            List<int> thirdHeap = new() { 1, 1, 1, 1, 1 };
            List<int> fourthHeap = new() { 1, 1, 1, 1, 1, 1, 1 };

            int DepthToReach = GetDepthValueByDifficulty(Difficulty);

            while (!GameIsFinished) 
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n\nIt's your turn!");
                Console.WriteLine("\nCurrent state of the game: ");

                PrintOneHeap(firstHeap);
                PrintOneHeap(secondHeap);
                PrintOneHeap(thirdHeap);
                PrintOneHeap(fourthHeap);
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Green;

                // Player's move
                int rowToWorkWith = 0;
                while (rowToWorkWith != 1 && rowToWorkWith != 2 && rowToWorkWith != 3 && rowToWorkWith != 4) 
                {
                    rowToWorkWith = ChooseARow(firstHeap.Count, secondHeap.Count, thirdHeap.Count, fourthHeap.Count);
                }

                var heapToWorkWith = GetListToWorkWith(rowToWorkWith, firstHeap, secondHeap, thirdHeap, fourthHeap);

                int numOfElemToDelete = DefineNumberOfElementsToDelete(heapToWorkWith.Count);
                DeleteElementsOfHeap(heapToWorkWith, numOfElemToDelete);
                Console.ResetColor();

                IsComputersMove = false;
                GameIsFinished = CheckIfGameIsOver(firstHeap.Count, secondHeap.Count, thirdHeap.Count, fourthHeap.Count, IsComputersMove);

                if (GameIsFinished) 
                {
                    break;
                }

                // Computer's move
                CurrentState = InitializeCurrentState(firstHeap.Count, secondHeap.Count, thirdHeap.Count, fourthHeap.Count);
                int StaticEvaluation = Problem.EvaluateState(CurrentState);
                Node CurrentStateNode = new(CurrentState, null, 0, StaticEvaluation);

                int FoundStaticEvaluation = Problem.MiniMax(CurrentStateNode, DepthToReach, Constants.NEG_INFINITY, Constants.POS_INFINITY, false, Difficulty);

                Node NextState = GetNextState(NextStateHolder.NextState, FoundStaticEvaluation);
                ExecuteComputerMove(NextState, firstHeap, secondHeap, thirdHeap, fourthHeap);
                NextStateHolder.NextState.Clear();

                IsComputersMove = true;
                GameIsFinished = CheckIfGameIsOver(firstHeap.Count, secondHeap.Count, thirdHeap.Count, fourthHeap.Count, IsComputersMove);
            }
        }

        private static void PrintOneHeap(List<int> heap) 
        {
            foreach (var item in heap) 
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
        }

        private static bool CheckIfGameIsOver(int firstHeapCount, int secondHeapCount, int thirdHeapCount, int fourthHeapCount, bool IsComputersMove) 
        {
            bool IsGameOver = false;

            if (firstHeapCount == 0 && secondHeapCount == 0 && thirdHeapCount == 0 && fourthHeapCount == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                IsGameOver = true;
                Console.WriteLine("\nGame over!");

                if (IsComputersMove)
                {
                    Console.WriteLine("Congratulations! You won.");
                }
                else 
                {
                    Console.WriteLine("You lost! Try playing better next time :D");
                }
                Console.ResetColor();
            }

            return IsGameOver;
        }

        private static int GetDepthValueByDifficulty(string Difficulty) 
        {
            int DepthToGo;

            switch (Difficulty) 
            {
                case "easy":
                    DepthToGo = 3;
                    break;
                case "medium":
                    DepthToGo = 6;
                    break;
                case "hard":
                    DepthToGo = 10;
                    break;
                default:
                    throw new ArgumentException($"Wrong difficulty input {Difficulty}.");
            }

            return DepthToGo;
        }

        public static Node GetNextState(List<Node> PossibleFutureStates, int FoundStaticEvaluation) 
        {
            Node NodeToReturn = null;
            int MaxStateDepth = FindMaxDepth(PossibleFutureStates);

            foreach(var Node in PossibleFutureStates)
            {
                if (Node.Depth == MaxStateDepth && Node.StaticEvaluation == FoundStaticEvaluation) 
                {
                    NodeToReturn = Node;
                }
            }

            return GetInitialParentNode(NodeToReturn);
        }

        public static int FindMaxDepth(List<Node> PossibleFutureStates) 
        {
            int MaxDepth = 0;

            foreach (var State in PossibleFutureStates) 
            {
                if (State.Depth > MaxDepth) 
                {
                    MaxDepth = State.Depth;
                }
            }

            return MaxDepth;
        }

        // Don't look ar this one xD
        public static Node GetInitialParentNode(Node ChildNode) 
        {
            switch (ChildNode.Depth) 
            {
                case 1:
                    return ChildNode;
                case 2:
                    return ChildNode.ParentNode;
                case 3:
                    return ChildNode.ParentNode.ParentNode;
                case 4:
                    return ChildNode.ParentNode.ParentNode.ParentNode;
                case 5:
                    return ChildNode.ParentNode.ParentNode.ParentNode.ParentNode;
                case 6:
                    return ChildNode.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode;
                case 7:
                    return ChildNode.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode;
                case 8:
                    return ChildNode.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode;
                case 9:
                    return ChildNode.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode;
                case 10:
                    return ChildNode.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode;
                default:
                    throw new ArgumentException("Maximum depth for MINIMAX algorithm is 10.");
            }
        }

        public static void ExecuteComputerMove(Node NextState, List<int> firstHeap, List<int> secondHeap, List<int> thirdHeap, List<int> fourthHeap) 
        {
            Console.WriteLine("\n\nExecuting computer's move...");

            //for the first heap
            int NumOfItemsToDelete = firstHeap.Count - NextState.State[0];
            DeleteElementsOfHeap(firstHeap, NumOfItemsToDelete);

            //for the second heap
            NumOfItemsToDelete = secondHeap.Count - NextState.State[1];
            DeleteElementsOfHeap(secondHeap, NumOfItemsToDelete);

            //for the third heap
            NumOfItemsToDelete = thirdHeap.Count - NextState.State[2];
            DeleteElementsOfHeap(thirdHeap, NumOfItemsToDelete);

            //for the fourth heap
            NumOfItemsToDelete = fourthHeap.Count - NextState.State[3];
            DeleteElementsOfHeap(fourthHeap, NumOfItemsToDelete);
        }

        private static int ChooseARow(int numOfElemAtFirst, int numOfElemAtSecond, int numOfElemAtThird, int numOfElemAtFourth) 
        {
            Console.Write("\nChoose a row [1|2|3|4]: ");
            int UserInputRow = int.Parse(Console.ReadLine());

            switch (UserInputRow) 
            {
                case 1:
                    UserInputRow = numOfElemAtFirst > 0 ? UserInputRow : 0;
                    if (UserInputRow == 1)
                    {
                        return UserInputRow;
                    }
                    else
                    {
                        break;
                    }
                case 2:
                    UserInputRow = numOfElemAtSecond > 0 ? UserInputRow : 0;
                    if (UserInputRow == 2)
                    {
                        return UserInputRow;
                    }
                    else
                    {
                        break;
                    }
                case 3:
                    UserInputRow = numOfElemAtThird > 0 ? UserInputRow : 0;
                    if (UserInputRow == 3)
                    {
                        return UserInputRow;
                    }
                    else 
                    {
                        break;
                    }

                case 4:
                    UserInputRow = numOfElemAtFourth > 0 ? UserInputRow : 0;
                    if (UserInputRow == 4)
                    {
                        return UserInputRow;
                    }
                    else
                    {
                        break;
                    }
            }

            if (UserInputRow < 0 || UserInputRow == 0 || UserInputRow > 4)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Try to define a row again!");
                Console.ResetColor();
            }

            return UserInputRow;
        }

        private static List<int> GetListToWorkWith(int ChosenRow, List<int> firstHeap, List<int> secondHeap, List<int> thirdHeap, List<int> fourthHeap)
        {
            List<int> chosenHeap = new();

            switch (ChosenRow) 
            {
                case 1:
                    chosenHeap = firstHeap;
                    break;
                case 2:
                    chosenHeap = secondHeap;
                    break;
                case 3:
                    chosenHeap = thirdHeap;
                    break;
                case 4:
                    chosenHeap = fourthHeap;
                    break;
                default:
                    throw new ArgumentException($"{ChosenRow} does not correspond to the actual row's number.");
            }

            return chosenHeap;
        }

        private static int[] InitializeCurrentState(int numOfElemAtFirst, int numOfElemAtSecond, int numOfElemAtThird, int numOfElemAtFourth)
        {
            int[] CurrentState = new int[4];

            CurrentState[0] = numOfElemAtFirst;
            CurrentState[1] = numOfElemAtSecond;
            CurrentState[2] = numOfElemAtThird;
            CurrentState[3] = numOfElemAtFourth;

            return CurrentState;
        }

        private static int DefineNumberOfElementsToDelete(int heapElemCount)
        {
            Console.Write("\nDefine a number of elements you want to take (delete): ");
            int UserInputNumberOfItemsToDelete = int.Parse(Console.ReadLine());

            if (UserInputNumberOfItemsToDelete > heapElemCount)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"There aren't that many items!\nYou've taken all the possible items instead of {UserInputNumberOfItemsToDelete}.");
                Console.ResetColor();
                UserInputNumberOfItemsToDelete = heapElemCount;
            }

            if (UserInputNumberOfItemsToDelete <= 0) 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"You must take at least one item!\nYou've taken one item instead of {UserInputNumberOfItemsToDelete}.");
                Console.ResetColor();
                UserInputNumberOfItemsToDelete = 1;
            }

            return UserInputNumberOfItemsToDelete;
        }

        public static void DeleteElementsOfHeap(List<int> heap, int numOfElemToDelete) 
        {
            if (heap.Count == numOfElemToDelete)
            {
                heap.Clear();
            }
            else 
            {
                for (int i = 0; i < numOfElemToDelete; i++) 
                {
                    heap.Remove(1);
                }
            }
        }
    }
}
