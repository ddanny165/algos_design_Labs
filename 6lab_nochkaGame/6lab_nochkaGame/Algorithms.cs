using System;
using System.Collections.Generic;


namespace _6lab_nochkaGame
{
    public static class Algorithms
    {
        public static int MiniMax(Node CurrentNode, int DepthToReach, bool MaximizingPlayer, List<Card> PlayersHand, List<Card> BotsHand) 
        {
            if (DepthToReach == 0 || UserInteraction.CheckIfGameIsOver(PlayersHand, BotsHand)) 
            {
                StateHolders.NextStates.Add(CurrentNode);
                return CurrentNode.StaticEvaluation;
            }

            if (MaximizingPlayer)
            {
                int MaxEval = Constants.NEG_INFINITY;
                List<Node> Successors = Expand(CurrentNode, PlayersHand, BotsHand, StateHolders.RowSuit);

                foreach (var Successor in Successors)
                {
                    int Eval = MiniMax(Successor, DepthToReach - 1, false, PlayersHand, BotsHand);
                    MaxEval = Math.Max(MaxEval, Eval);
                }

                StateHolders.NextStates.Add(CurrentNode);
                return MaxEval;
            }
            else 
            {
                int MinEval = Constants.POS_INFINITY;
                List<Node> Successors = Expand(CurrentNode, PlayersHand, BotsHand, StateHolders.RowSuit);

                foreach (var Successor in Successors)
                {
                    int Eval = MiniMax(Successor, DepthToReach - 1, false, PlayersHand, BotsHand);
                    MinEval = Math.Min(MinEval, Eval);
                }

                StateHolders.NextStates.Add(CurrentNode);
                return MinEval;
            }
        }

        private static List<Node> Expand(Node NodeToExpand, List<Card> PlayersHand, List<Card> BotsHand, Dictionary<int, string> RowSuit) 
        {
            int StaticEvaluation;
            List<Node> Successors = new();

            List<List<(string, int[])>> NewStates = GetNewStates(ConvertToMatrixGameState(NodeToExpand.State), BotsHand, RowSuit);

            foreach (var NewState in NewStates) 
            {
                StaticEvaluation = EvaluateState(NewState, PlayersHand, BotsHand);

                Node NewNode = new Node(NewState, NodeToExpand, NodeToExpand.Depth + 1, StaticEvaluation);
                Successors.Add(NewNode);
            }

            return Successors;
        }

        public static int EvaluateState(List<(string, int[])> State, List<Card> PlayersHand, List<Card> BotsHand) 
        {
            int Eval = 0;
            // current game state suits
            string[] StateSuits = new string[4];
            for (int i = 0; i < StateSuits.Length; i++) 
            {
                StateSuits[i] = State[i].Item1;
            }

            // gamestate at matrix (taken cards)
            int[,] GameStateToEvaluate = ConvertToMatrixGameState(State);

            for (int i = 0; i < 4; i++) 
            {
                for (int j = 0; j < 9; j++) 
                {
                    if (GameStateToEvaluate[i, j] == 1 && j != 0 && j != 8) 
                    {
                        Eval += EvaluateCard(UserInteraction.GetCardValueByCounter(j));
                    }
                
                }
            }

            return Eval;
        }

        private static int EvaluateCard(string value) 
        {
            switch (value) 
            {
                case "6":
                    return 5;
                case "7":
                    return 4;
                case "8":
                    return 3;
                case "9":
                    return 2;
                case "10":
                    return 1;
                case "J":
                    return 2;
                case "Q":
                    return 3;
                case "K":
                    return 4;
                case "A":
                    return 5;
                default:
                    return 0;
            }
        
        }

        public static int[,] ConvertToMatrixGameState(List<(String, int[])> State) 
        {
            int[,] CurrentNodeGameState = new int[4, 9];

            for (int i = 0; i < 4; i++) 
            {
                for (int j = 0; j < 9; j++) 
                {
                    CurrentNodeGameState[i, j] = State[i].Item2[j]; 
                }
            }

            return CurrentNodeGameState;
        }

        private static List<List<(string, int[])>> GetNewStates(int[,] GameState, List<Card> BotsHand, Dictionary<int, string> RowSuit) 
        {
            List<(int, int)> newPositions = GetNewPositions(GameState, BotsHand, RowSuit);

            List<List<(string, int[])>> NewStates = new();

            for (int i = 0; i < newPositions.Count; i++) 
            {
                int[,] newMatrixState = (int[,])GameState.Clone();
                newMatrixState[newPositions[i].Item1, newPositions[i].Item2] = 1;

                NewStates.Add(ConvertMatrixToState(newMatrixState, RowSuit));
            }

            return NewStates;
        }

        public static List<(string, int[])> ConvertMatrixToState(int[,] GameState, Dictionary<int, string> RowSuit) 
        {
            List<(string, int[])> state = new();
            int[] row = new int[9];


            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    row[j] = GameState[i, j];
                }

                state.Add((RowSuit[i + 1], row));
            }

            return state;
        }

        private static List<(int, int)> GetNewPositions(int[,] GameState, List<Card> BotsHand, Dictionary<int, string> RowSuit) 
        {
            List<(int, int)> NewPositions = new();

            List<(int, int)> currentPositions = new();
            bool NoSuitsDefined = true;

            for (int i = 1; i < 5; i++) 
            {
                if (RowSuit[i] != null)
                {
                    NoSuitsDefined = false;
                    break;
                }
            }

            string currentSuit;
            int row;
            int column;

            for (int i = 1; i < 5; i++) 
            {
                currentSuit = RowSuit[i];
                foreach (var Card in BotsHand)
                {
                    if (Card != null && (Card.CardSuit == currentSuit || NoSuitsDefined))
                    {
                        column = UserInteraction.GetCounterByCardValue(Card.CardValue);
                        row = i - 1;
                        currentPositions.Add((row, column));
                    }
                }
            }

            for (int i = 0; i < currentPositions.Count; i++) 
            {
                if (UserInteraction.CheckIfaValidMove(currentPositions[i].Item1, currentPositions[i].Item2, GameState) &&
                    CheckIfBotHasASuitableCard(BotsHand, StateHolders.RowSuit[currentPositions[i].Item1 + 1], UserInteraction.GetCardValueByCounter(currentPositions[i].Item2))) //GETCARD VALUE BY COUNTER
                {
                    NewPositions.Add((currentPositions[i].Item1, currentPositions[i].Item2));
                }
            }

            if (NewPositions == null) 
            {
                Console.WriteLine("New Positions null");
            }
            
            return NewPositions;
        }

        private static bool CheckIfBotHasASuitableCard(List<Card> BotsHand, string Suit, string Value) 
        {
            bool BotHasASuitable = false;

            foreach (var Card in BotsHand)
            {
                if ((Card.CardSuit == Suit || Suit == null) && Card.CardValue == Value) 
                {
                    BotHasASuitable = true;
                    break;
                }
            }

            return BotHasASuitable;
        }
    }
}
