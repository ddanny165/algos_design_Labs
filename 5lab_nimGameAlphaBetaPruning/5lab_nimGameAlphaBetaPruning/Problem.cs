using System;
using System.Collections.Generic;

namespace _5lab_nimGameAlphaBetaPruning
{

    public static class Problem
    {
        public static int MiniMax(Node CurrentNode, int DepthToReach, int Alpha, int Beta, bool MaximizingPlayer, string Difficulty)
        {
            if (DepthToReach == 0 || CheckIfGameIsOver(CurrentNode)) 
            {
                NextStateHolder.NextState.Add(CurrentNode);
                return CurrentNode.StaticEvaluation;
            }

            if (MaximizingPlayer)
            {
                int MaxEval = Constants.NEG_INFINITY;
                List<Node> Successors = Expand(CurrentNode, Difficulty);

                foreach (var Successor in Successors)
                {
                    int Eval = MiniMax(Successor, DepthToReach - 1, Alpha, Beta, false, Difficulty);
                    MaxEval = CompareAndRetBiggest(MaxEval, Eval);

                    Alpha = CompareAndRetBiggest(Alpha, Eval);
                    
                    if (Beta <= Alpha) 
                    {
                        break;
                    }
                }

                NextStateHolder.NextState.Add(CurrentNode);
                return MaxEval;
            }
            else
            {
                int MinEval = Constants.POS_INFINITY;
                List<Node> Successors = Expand(CurrentNode, Difficulty);

                foreach (var Successor in Successors)
                {
                    int Eval = MiniMax(Successor, DepthToReach - 1, Alpha, Beta, true, Difficulty);
                    MinEval = CompareAndRetSmallest(MinEval, Eval);

                    Beta = CompareAndRetSmallest(Beta, Eval);

                    if (Beta <= Alpha) 
                    {
                        break;
                    }
                }

                NextStateHolder.NextState.Add(CurrentNode);
                return MinEval;
            }
        }

        private static List<Node> Expand(Node NodeToExpand, string Difficulty) 
        {
            int StaticEvaluation;
            List<Node> Successors = new();
            List<int[]> NewStates = GetNewStates(NodeToExpand.State);

            foreach (var NewState in NewStates) 
            {
                if (Difficulty == "easy")
                {
                    StaticEvaluation = EvaluateStateForEasyDiff(NewState);
                }
                else 
                {
                    StaticEvaluation = EvaluateState(NewState);
                }

                Node NewNode = new Node(NewState, NodeToExpand, NodeToExpand.Depth + 1, StaticEvaluation);
                Successors.Add(NewNode);
            }

            return Successors;
        }

        private static List<int[]> GetNewStates(int[] CurrentState) 
        {
            List<int[]> NewStates = new();
            int[] NewState = new int[4];

            // for first heap
            if (CurrentState[0] == 1) 
            {
                NewState = new List<int>(CurrentState).ToArray();
                NewState[0] = 0;
                NewStates.Add(NewState);
            }

            // for second heap
            switch (CurrentState[1]) 
            {
                case 3:
                    for (int i = 0; i < 3; i++)
                    {
                        NewState = new List<int>(CurrentState).ToArray();
                        NewState[1] = i;
                        NewStates.Add(NewState);

                    }
                    break;

                case 2:
                    for (int i = 0; i < 2; i++)
                    {
                        NewState = new List<int>(CurrentState).ToArray();
                        NewState[1] = i;
                        NewStates.Add(NewState);

                    }
                    break;

                case 1:
                    NewState = new List<int>(CurrentState).ToArray();
                    NewState[1] = 0;
                    NewStates.Add(NewState);
                    break;

                default:
                    break;

            }

            // for third heap
            switch (CurrentState[2]) 
            {
                case 5:
                    for (int i = 0; i < 5; i++) 
                    {
                        NewState = new List<int>(CurrentState).ToArray();
                        NewState[2] = i;
                        NewStates.Add(NewState);

                    }
                    break;

                case 4:
                    for (int i = 0; i < 4; i++)
                    {
                        NewState = new List<int>(CurrentState).ToArray();
                        NewState[2] = i;
                        NewStates.Add(NewState);

                    }
                    break;

                case 3:
                    for (int i = 0; i < 3; i++)
                    {
                        NewState = new List<int>(CurrentState).ToArray();
                        NewState[2] = i;
                        NewStates.Add(NewState);

                    }
                    break;

                case 2:
                    for (int i = 0; i < 2; i++)
                    {
                        NewState = new List<int>(CurrentState).ToArray();
                        NewState[2] = i;
                        NewStates.Add(NewState);

                    }
                    break;

                case 1:
                    NewState = new List<int>(CurrentState).ToArray();
                    NewState[2] = 0;
                    NewStates.Add(NewState);
                    break;

                default:
                    break;
            }

            // for fourth heap
            switch (CurrentState[3]) 
            {
                case 7:
                    for (int i = 0; i < 7; i++)
                    {
                        NewState = new List<int>(CurrentState).ToArray();
                        NewState[3] = i;
                        NewStates.Add(NewState);

                    }
                    break;

                case 6:
                    for (int i = 0; i < 6; i++)
                    {
                        NewState = new List<int>(CurrentState).ToArray();
                        NewState[3] = i;
                        NewStates.Add(NewState);

                    }
                    break;

                case 5:
                    for (int i = 0; i < 5; i++)
                    {
                        NewState = new List<int>(CurrentState).ToArray();
                        NewState[3] = i;
                        NewStates.Add(NewState);

                    }
                    break;

                case 4:
                    for (int i = 0; i < 4; i++)
                    {
                        NewState = new List<int>(CurrentState).ToArray();
                        NewState[3] = i;
                        NewStates.Add(NewState);

                    }
                    break;

                case 3:
                    for (int i = 0; i < 3; i++)
                    {
                        NewState = new List<int>(CurrentState).ToArray();
                        NewState[3] = i;
                        NewStates.Add(NewState);

                    }
                    break;

                case 2:
                    for (int i = 0; i < 2; i++)
                    {
                        NewState = new List<int>(CurrentState).ToArray();
                        NewState[3] = i;
                        NewStates.Add(NewState);

                    }
                    break;

                case 1:
                    NewState = new List<int>(CurrentState).ToArray();
                    NewState[3] = 0;
                    NewStates.Add(NewState);
                    break;

                default:
                    break;

            }


            return NewStates;
        }

        private static int EvaluateStateForEasyDiff(int[] State) 
        {
            int Eval = 0;

            for (int i = 0; i < State.Length; i++)
            {
                Eval += State[i];
            }

            return Eval;
        }

        public static int EvaluateState(int[] State)
        {
            int Eval = 0;

            for (int i = 0; i < State.Length; i++) 
            {
                Eval += State[i];
            
            }

            return Eval * -1;
        }

        private static int CompareAndRetBiggest(int FirstNum, int SecondNum)
        {
            if (FirstNum > SecondNum)
            {
                return FirstNum;
            }
            else
            {
                return SecondNum;
            }
        }

        private static int CompareAndRetSmallest(int FirstNum, int SecondNum)
        {
            if (FirstNum > SecondNum)
            {
                return SecondNum;
            }
            else
            {
                return FirstNum;
            }
        }

        private static bool CheckIfGameIsOver(Node CurrentNode)
        {
            bool IsOver = false;

            if (CurrentNode.State[0] == 0 && CurrentNode.State[1] == 0 &&
                CurrentNode.State[2] == 0 && CurrentNode.State[3] == 0) 
            {
                IsOver = true;
            }

            return IsOver;
        }
    }
}
