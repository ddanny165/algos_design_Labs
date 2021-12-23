using System;
using System.Collections.Generic;

namespace _6lab_nochkaGame
{
    public static class UserInteraction
    {
        public static void InteractWithUser() 
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Welcome to the Nochka game!\nCreated by Danylo Moskaliuk as 6th lab for Algorithms Design.");
            Console.WriteLine("\n\nYour goal is to be the first one to get rid of all cards.\nGood luck!");
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
                Console.Clear();
                Console.WriteLine("\n\nTry again!\n");
                InteractWithUser();
            }

            if (UserInput != 1)
            {
                Console.Clear();
                Console.WriteLine("\n\nTry again!\n");
                InteractWithUser();
            }
            Console.ResetColor();

            NochkaGame();
        }

        private static void NochkaGame() 
        {
            bool IsComputersMove;
            int[,] GameState;
            bool IsGameFinished = false;
            Dictionary<int, (string, string, bool)> AllCards = new()
            {
                { 1, ("6", "Hearts", false) },
                { 2, ("7", "Hearts", false) },
                { 3, ("8", "Hearts", false) },
                { 4, ("9", "Hearts", false) },
                { 5, ("10", "Hearts", false) },
                { 6, ("J", "Hearts", false) },
                { 7, ("Q", "Hearts", false) },
                { 8, ("K", "Hearts", false) },
                { 9, ("A", "Hearts", false) },
                { 10, ("6", "Diamonds", false) },
                { 11, ("7", "Diamonds", false) },
                { 12, ("8", "Diamonds", false) },
                { 13, ("9", "Diamonds", false) },
                { 14, ("10", "Diamonds", false) },
                { 15, ("J", "Diamonds", false) },
                { 16, ("Q", "Diamonds", false) },
                { 17, ("K", "Diamonds", false) },
                { 18, ("A", "Diamonds", false) },
                { 19, ("6", "Spades", false) },
                { 20, ("7", "Spades", false) },
                { 21, ("8", "Spades", false) },
                { 22, ("9", "Spades", false) },
                { 23, ("10", "Spades", false) },
                { 24, ("J", "Spades", false) },
                { 25, ("Q", "Spades", false) },
                { 26, ("K", "Spades", false) },
                { 27, ("A", "Spades", false) },
                { 28, ("6", "Clubs", false) },
                { 29, ("7", "Clubs", false) },
                { 30, ("8", "Clubs", false) },
                { 31, ("9", "Clubs", false) },
                { 32, ("10", "Clubs", false) },
                { 33, ("J", "Clubs", false) },
                { 34, ("Q", "Clubs", false) },
                { 35, ("K", "Clubs", false) },
                { 36, ("A", "Clubs", false) }
            };
       
            CardPlaceholder[] firstRow = InitializeRow(AllCards);
            CardPlaceholder[] secondRow = InitializeRow(AllCards);
            CardPlaceholder[] thirdRow = InitializeRow(AllCards);
            CardPlaceholder[] fourthRow = InitializeRow(AllCards);

            List<Card> PlayersHand = GenerateInitialHand(AllCards);
            List<Card> BotsHand = GenerateInitialHand(AllCards);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\nThere are four rows:\n");
            Console.ResetColor();

            while (!IsGameFinished) 
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                OutputRow(1, firstRow);
                OutputRow(2, secondRow);
                OutputRow(3, thirdRow);
                OutputRow(4, fourthRow);
                OutputAlreadyLaidCards(StateHolders.PutCards);
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Green;
                OutputPlayersHand(PlayersHand);
                GameState = GetGameState(firstRow, secondRow, thirdRow, fourthRow);

                //Player's move
                ExecutePlayersMove(PlayersHand, GameState, firstRow, secondRow, thirdRow, fourthRow);
                IsComputersMove = false;
                IsGameFinished = CheckIfGameIsOver(PlayersHand, BotsHand);

                if (IsGameFinished && !IsComputersMove)
                {
                    Console.WriteLine("You won! Congrats!");
                }

                // Computer's move
                GameState = GetGameState(firstRow, secondRow, thirdRow, fourthRow);
                var CurrentState = Algorithms.ConvertMatrixToState(GameState, StateHolders.RowSuit);

                int StaticEvaluation = Algorithms.EvaluateState(CurrentState, PlayersHand, BotsHand);
                Node CurrentStateNode = new(CurrentState, null, 0, StaticEvaluation);
                int FoundStaticEvaluation = Algorithms.MiniMax(CurrentStateNode, 5, false, PlayersHand, BotsHand);

                Node NextState = GetNextState(StateHolders.NextStates, FoundStaticEvaluation);
                ExecuteBotsMove(NextState, GameState, BotsHand, firstRow, secondRow, thirdRow, fourthRow);
                StateHolders.NextStates.Clear();
                IsComputersMove = true;
                IsGameFinished = CheckIfGameIsOver(PlayersHand, BotsHand);

                if (IsGameFinished && IsComputersMove) 
                {
                    Console.WriteLine("Computer won! Try again next time.");
                    break;
                }
               
                // just to check
                int count = BotsHand.Count;
                for (int i = 0; i < count; i++) 
                {
                    if (BotsHand[i] == null) 
                    {
                        BotsHand.Remove(BotsHand[i]);
                        count -= 1;
                        i -= 1;
                    }
                }
                Console.ResetColor();
            }
        }

        private static void OutputAlreadyLaidCards(List<Card> LaidCards) 
        {
            int ListLength = LaidCards.Count;
            Console.WriteLine("\nAlready laid cards: ");
            for (int i = 0; i < ListLength; i++) 
            {
                Console.Write($"|{LaidCards[i].CardValue}, {LaidCards[i].CardSuit}| ");
            }
            Console.WriteLine();
        }

        public static void ExecuteBotsMove(Node NextState, int[,] PreviousGameState, 
            List<Card> BotsHand, CardPlaceholder[] firstRow, CardPlaceholder[] secondRow, CardPlaceholder[] thirdRow, CardPlaceholder[] fourthRow) 
        {
            Console.WriteLine("Executing bot's move...");

            int[,] NewGameState = Algorithms.ConvertToMatrixGameState(NextState.State);
            CardPlaceholder[] rowToWorkWith;

            (int, int) newPosition = (-1, -1);

            for (int i = 0; i < 4; i++) 
            {
                for (int j = 0; j < 9; j++) 
                {
                    if (NewGameState[i, j] != PreviousGameState[i, j]) 
                    {
                        newPosition = (i, j);
                        break;
                    }
                }
            }

            bool IsCardFound = false;
            Card CardToRemove = null;

            if (newPosition.Item1 == -1 && newPosition.Item2 == -1)
            {
                Random ran = new Random();
                foreach (var Card in BotsHand)
                {
                    if (Card.CardValue == "6" || Card.CardValue == "A")
                    {
                        CardToRemove = Card;
                        int rowNum = 0;

                        for (int i = 1; i < 5; i++) 
                        {
                            if (StateHolders.RowSuit[i] == Card.CardSuit) 
                            {
                                rowNum = i - 1;
                            }
                        }

                        if (StateHolders.RowSuit[rowNum + 1] != Card.CardSuit) 
                        {
                            while (StateHolders.RowSuit[rowNum + 1] != Card.CardSuit && StateHolders.RowSuit[rowNum + 1] != null) 
                            {
                                rowNum = ran.Next(0, 3);
                            }
                        }

                        switch (rowNum + 1)
                        {
                            case 1:
                                rowToWorkWith = firstRow;
                                break;
                            case 2:
                                rowToWorkWith = secondRow;
                                break;
                            case 3:
                                rowToWorkWith = thirdRow;
                                break;
                            case 4:
                                rowToWorkWith = fourthRow;
                                break;
                            default:
                                throw new ArgumentException("Incorrect row input number.");
                        }

                        int tempRowNum = rowNum;

                        if (Card.CardValue == "6")
                        {
                            rowNum = 0;
                        }

                        if (Card.CardValue == "A")
                        {
                            rowNum = 8;
                        }

                        rowToWorkWith[rowNum].cards[0] = CardToRemove;

                        rowNum = tempRowNum;
                        // заполняем все плейсхолдеры одной и той же мастью
                        if (rowToWorkWith[2].cards[0].CardSuit == null && CheckIfSuitNotDefinedYet(StateHolders.RowSuit, CardToRemove.CardSuit))
                        {
                            foreach (var CardPlace in rowToWorkWith)
                            {
                                CardPlace.cards[0].CardSuit = CardToRemove.CardSuit;
                            }
                            StateHolders.RowSuit[rowNum + 1] = CardToRemove.CardSuit;
                        }

                        BotsHand.Remove(CardToRemove);
                        Console.WriteLine($"Number of cards in bot's hand {BotsHand.Count}");
                        break;
                    }
                }
            }
            else 
            {
                foreach (var Card in BotsHand)
                {
                    // Card != null
                    if (Card.CardValue == GetCardValueByCounter(newPosition.Item2))
                    {
                        CardToRemove = Card;
                        IsCardFound = true;

                        switch (newPosition.Item1 + 1)
                        {
                            case 1:
                                rowToWorkWith = firstRow;
                                break;
                            case 2:
                                rowToWorkWith = secondRow;
                                break;
                            case 3:
                                rowToWorkWith = thirdRow;
                                break;
                            case 4:
                                rowToWorkWith = fourthRow;
                                break;
                            default:
                                throw new ArgumentException("Incorrect row input number.");
                        }

                        if (rowToWorkWith[newPosition.Item2].cards[0].CardSuit == CardToRemove.CardSuit ||
                            (rowToWorkWith[2].cards[0].CardSuit == null && CheckIfSuitNotDefinedYet(StateHolders.RowSuit, CardToRemove.CardSuit)))
                        {
                            rowToWorkWith[newPosition.Item2].cards[0] = CardToRemove;
                        }
                        else
                        {
                            int rowNum = 0;

                            for (int i = 1; i < 5; i++)
                            {
                                if (StateHolders.RowSuit[i] == CardToRemove.CardSuit)
                                {
                                    rowNum = i - 1;
                                }

                                switch (rowNum + 1)
                                {
                                    case 1:
                                        rowToWorkWith = firstRow;
                                        break;
                                    case 2:
                                        rowToWorkWith = secondRow;
                                        break;
                                    case 3:
                                        rowToWorkWith = thirdRow;
                                        break;
                                    case 4:
                                        rowToWorkWith = fourthRow;
                                        break;
                                    default:
                                        throw new ArgumentException("Incorrect row input number.");
                                }
                            }
                        }

                        // заполняем все плейсхолдеры одной и той же мастью
                        foreach (var CardPlace in rowToWorkWith)
                        {
                            CardPlace.cards[0].CardSuit = CardToRemove.CardSuit;
                        }
                        StateHolders.RowSuit[newPosition.Item1 + 1] = CardToRemove.CardSuit;

                        if (rowToWorkWith[newPosition.Item2].cards[1] != null)
                        {
                            BotsHand.Add(rowToWorkWith[newPosition.Item2].cards[1]);
                            rowToWorkWith[newPosition.Item2].cards[1] = null;
                        }

                        break;
                    }
                    BotsHand.Remove(CardToRemove);
                }
            }

            if (CardToRemove != null)
            {
                Console.WriteLine($"Bot has put: {CardToRemove.CardSuit}, {CardToRemove.CardValue}");
                StateHolders.PutCards.Add(CardToRemove);
                BotsHand.Remove(CardToRemove);
            }
            else 
            {
                switch (newPosition.Item1 + 1)
                {
                    case 1:
                        rowToWorkWith = firstRow;
                        break;
                    case 2:
                        rowToWorkWith = secondRow;
                        break;
                    case 3:
                        rowToWorkWith = thirdRow;
                        break;
                    case 4:
                        rowToWorkWith = fourthRow;
                        break;
                    default:
                        throw new ArgumentException("Incorrect row input number.");
                }

                if (rowToWorkWith[newPosition.Item2].cards[1] != null)
                {
                    Console.WriteLine($"Bot took the card: {rowToWorkWith[newPosition.Item2].cards[1].CardSuit}, {rowToWorkWith[newPosition.Item2].cards[1].CardValue}\n");
                    BotsHand.Add(rowToWorkWith[newPosition.Item2].cards[1]);
                    rowToWorkWith[newPosition.Item2].cards[1] = null;
                }
                else 
                {
                    Random ran = new Random();
                    int row = ran.Next(0, 3);
                    int column = ran.Next(0, 8);

                    switch (row + 1)
                    {
                        case 1:
                            rowToWorkWith = firstRow;
                            break;
                        case 2:
                            rowToWorkWith = secondRow;
                            break;
                        case 3:
                            rowToWorkWith = thirdRow;
                            break;
                        case 4:
                            rowToWorkWith = fourthRow;
                            break;
                        default:
                            throw new ArgumentException("Incorrect row input number.");
                    }

                    int iterationsCounter = 0;
                    while (rowToWorkWith[column].cards[1] == null || !CheckIfaValidMove(row, column, NewGameState)) 
                    {
                        row = 0;
                        column = 0;
                        bool foundValue = false;

                        int rowToStart = ran.Next(0, 3);
                        int columnToStart = ran.Next(0, 8);

                        for (int i = rowToStart; i < 4; i++) 
                        {
                            for (int j = columnToStart; j < 9; j++) 
                            {
                                if (NewGameState[i, j] == 1) 
                                {
                                    row = i;

                                    if (j == 0) 
                                    {
                                        column = 1;
                                        foundValue = true;
                                        break;

                                    }

                                    if (j == 8)
                                    {
                                        column = 7;
                                        foundValue = true;
                                        break;
                                    }

                                    column = ran.Next(2, 6);
                                }
                            }
                            if (foundValue) 
                            {
                                break;
                            }
                        }

                        if (iterationsCounter > 1500 && iterationsCounter < 3000) 
                        {
                            row = ran.Next(2, 3);
                        }

                        if (iterationsCounter > 3000) 
                        {
                            row = ran.Next(0, 1);
                        }

                        if (iterationsCounter > 5000) 
                        {
                            row = ran.Next(1, 2);
                            column = ran.Next(2, 5);
                        }

                        switch (row + 1)
                        {
                            case 1:
                                rowToWorkWith = firstRow;
                                break;
                            case 2:
                                rowToWorkWith = secondRow;
                                break;
                            case 3:
                                rowToWorkWith = thirdRow;
                                break;
                            case 4:
                                rowToWorkWith = fourthRow;
                                break;
                            default:
                                throw new ArgumentException("Incorrect row input number.");
                        }

                        iterationsCounter++;
                    }

                    Console.WriteLine($"Bot took the card: {rowToWorkWith[column].cards[1].CardSuit}, {rowToWorkWith[column].cards[1].CardValue}\n");
                    BotsHand.Add(rowToWorkWith[column].cards[1]);
                    rowToWorkWith[column].cards[1] = null;
                }
            }
        }

        public static bool CheckIfSuitNotDefinedYet(Dictionary<int, string> RowSuit, string Suit)
        {
            bool NotDefined = true;

            for (int i = 1; i < 5; i++) 
            {
                if (RowSuit[i] == Suit) 
                {
                    NotDefined = false;
                }
            
            }

            return NotDefined;
        }

        public static Node GetNextState(List<Node> PossibleFutureStates, int FoundStaticEvaluation)
        {
            Node NodeToReturn = null;
            int MaxStateDepth = FindMaxDepth(PossibleFutureStates);

            if (FoundStaticEvaluation == int.MaxValue)
            {
                foreach (var Node in PossibleFutureStates)
                {
                    if (Node.Depth == MaxStateDepth)
                    {
                        NodeToReturn = Node;
                    }
                }
                return NodeToReturn;
            }
            else 
            {
                foreach (var Node in PossibleFutureStates)
                {
                    if (Node.Depth == MaxStateDepth && Node.StaticEvaluation == FoundStaticEvaluation)
                    {
                        NodeToReturn = Node;
                    }
                }
                return GetInitialParentNode(NodeToReturn);
            }
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

        public static bool CheckIfGameIsOver(List<Card> PlayersHand, List<Card> BotsHand) 
        {
            bool IsGameOver = false;

            if (PlayersHand.Count == 0 || BotsHand.Count == 0)
            {
                IsGameOver = true;
            }

            return IsGameOver;
        }

        private static void ExecutePlayersMove(List<Card> PlayersHand, int[,] GameState, CardPlaceholder[] firstRow, CardPlaceholder[] secondRow, CardPlaceholder[] thirdRow, CardPlaceholder[] fourthRow) 
        {
            Console.WriteLine("\n\nIt's your turn!\nIf value is true, it means there is a card to take.\n");
            Console.ForegroundColor = ConsoleColor.Red;

            Console.Write("Define a row: ");
            int UserRowInput = 0;
            CardPlaceholder[] rowToWorkWith;

            try
            {
                UserRowInput = int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("\n\nTry again!\n");
                ExecutePlayersMove(PlayersHand, GameState, firstRow, secondRow, thirdRow, fourthRow);
            }
            if (UserRowInput != 1 && UserRowInput != 2 && UserRowInput != 3 && UserRowInput != 4)
            {
                Console.WriteLine("\n\nTry again!\n");
                ExecutePlayersMove(PlayersHand, GameState, firstRow, secondRow, thirdRow, fourthRow);
            }

            switch (UserRowInput)
            {
                case 1:
                    rowToWorkWith = firstRow;
                    break;
                case 2:
                    rowToWorkWith = secondRow;
                    break;
                case 3:
                    rowToWorkWith = thirdRow;
                    break;
                case 4:
                    rowToWorkWith = fourthRow;
                    break;
                default:
                    throw new ArgumentException("Incorrect row input number.");
            }

            string UserValueInput;
            Console.Write("Define a card value to go: ");
            UserValueInput = Console.ReadLine();

            if (CheckIfaValidMove(UserRowInput - 1, GetCounterByCardValue(UserValueInput), GameState))
            {
                // for cases with 6 and A
                bool CheckIfThereIsACardAvailable = false;
                if (UserValueInput == "6" || UserValueInput == "A")
                {
                    // проверяем есть ли у игрока доступная карта типа 6 или А
                    int rowNum = GetCounterByCardValue(UserValueInput);
                    foreach (var Card in PlayersHand)
                    {
                        if (Card.CardValue == UserValueInput && (rowToWorkWith[rowNum].cards[0].CardSuit == null || rowToWorkWith[rowNum].cards[0].CardSuit == Card.CardSuit))
                        {
                            CheckIfThereIsACardAvailable = true;
                        }
                    }

                    // если такая имеется, даем возможность выбрать игроку
                    if (CheckIfThereIsACardAvailable)
                    {
                        Console.Write("\nChoose a card number from your hand you want to use:");
                        int CardNum = int.Parse(Console.ReadLine());
                        var ChosenCard = PlayersHand[CardNum];

                        // проверяем, была ли выбрана подходящая карта
                        if (ChosenCard.CardValue == UserValueInput && (rowToWorkWith[rowNum].cards[0].CardSuit == null || rowToWorkWith[rowNum].cards[0].CardSuit == ChosenCard.CardSuit)
                            && !CheckIfSuitAlreadyUsedForAnother(StateHolders.RowSuit, ChosenCard.CardSuit, UserRowInput)) //UserRowInput
                        {
                            rowToWorkWith[rowNum].cards[0] = ChosenCard;

                            PlayersHand.Remove(ChosenCard);
                            StateHolders.PutCards.Add(ChosenCard);
                            // заполняем все плейсхолдеры одной и той же мастью
                            foreach (var Card in rowToWorkWith)
                            {
                                Card.cards[0].CardSuit = ChosenCard.CardSuit;
                            }

                            StateHolders.RowSuit[UserRowInput] = ChosenCard.CardSuit;
                        }
                        else // если подходящая карта не была выбрана
                        {
                            Console.WriteLine("\n\nIncorrect card value. Try again!\n");
                            ExecutePlayersMove(PlayersHand, GameState, firstRow, secondRow, thirdRow, fourthRow);
                        }
                    }
                    else //если карты нет, то перезапускаем метод
                    {
                        Console.WriteLine("\n\nIncorrect card value. Try again!\n");
                        ExecutePlayersMove(PlayersHand, GameState, firstRow, secondRow, thirdRow, fourthRow);
                    }

                }
                else
                {
                    if (UserValueInput != "7" && UserValueInput != "8" && UserValueInput != "9" && UserValueInput != "10"
                    && UserValueInput != "J" && UserValueInput != "Q" && UserValueInput != "K")
                    {
                        Console.WriteLine("\n\nIncorrect card value. Try again!\n");
                        ExecutePlayersMove(PlayersHand, GameState, firstRow, secondRow, thirdRow, fourthRow);
                    }

                    ExecuteMove(rowToWorkWith, UserRowInput, UserValueInput, PlayersHand); //UserRowInput - 1
                }

            }
            else 
            {
                Console.WriteLine("Your move is not valid. Try again.");
                ExecutePlayersMove(PlayersHand, GameState, firstRow, secondRow, thirdRow, fourthRow);
            }
            

            Console.ResetColor();
        }

        public static void ExecuteMove(CardPlaceholder[] ChosenRow, int ChosenRowNum, string ChosenCardValue, List<Card> PlayersHand) 
        {
            int rowNum = GetCounterByCardValue(ChosenCardValue);
            bool CheckIfThereIsACardAvailable = false;

            // check if there is an available card for user to put
            foreach (var Card in PlayersHand) 
            {    // совпадает ли масть и значение карты, чтобы положить
                if (Card.CardValue == ChosenCardValue && (ChosenRow[rowNum].cards[0].CardSuit == null || ChosenRow[rowNum].cards[0].CardSuit == Card.CardSuit)) 
                {
                    CheckIfThereIsACardAvailable = true;
                }
            }

            // если есть подходящая карта
            if (CheckIfThereIsACardAvailable)
            {
                // выбираем карту
                Console.Write("\nChoose a card number from your hand you want to use:");
                int CardNum = int.Parse(Console.ReadLine());

                var CardToPut = PlayersHand[CardNum];

                // если карта подходит, то выполняем действия
                if (CardToPut.CardValue == ChosenCardValue && (ChosenRow[rowNum].cards[0].CardSuit == null || ChosenRow[rowNum].cards[0].CardSuit == CardToPut.CardSuit))
                {
                    
                    ChosenRow[rowNum].cards[0] = CardToPut;

                    // заполняем все плейсхолдеры одной и той же мастью
                    foreach (var Card in ChosenRow) 
                    {
                        Card.cards[0].CardSuit = CardToPut.CardSuit;
                    }

                    StateHolders.RowSuit[ChosenRowNum] = CardToPut.CardSuit;

                    PlayersHand.Remove(CardToPut);
                    StateHolders.PutCards.Add(CardToPut);

                    if (ChosenRow[rowNum].cards[1] != null) 
                    {
                        Console.WriteLine($"You've taken: {ChosenRow[rowNum].cards[1].CardSuit}, {ChosenRow[rowNum].cards[1].CardValue}.\n\n");
                        PlayersHand.Add(ChosenRow[rowNum].cards[1]);
                        ChosenRow[rowNum].cards[1] = null;
                    }
                }
                else //если взяли не ту карту
                {
                    Console.WriteLine("Try again! You've chosen incorrect card!");
                    ExecuteMove(ChosenRow, ChosenRowNum, ChosenCardValue, PlayersHand);
                }
            }
            else // если подходящей карты нет - просто берем то, куда мы пошли - при этом место остается незаполненным
            {
                Console.WriteLine("\nYou didn't have the suitable card to put.");
                if (ChosenRow[rowNum].cards[1] != null)
                {
                    Console.WriteLine($"You've taken: {ChosenRow[rowNum].cards[1].CardSuit}, {ChosenRow[rowNum].cards[1].CardValue}.\n\n");
                    PlayersHand.Add(ChosenRow[rowNum].cards[1]);
                    ChosenRow[rowNum].cards[1] = null;
                }
            }
        }

        public static bool CheckIfaValidMove(int DefinedRow, int DefinedColumn, int[,] GameState) 
        {
            bool IsAValidMove = false;

            int rows = GameState.GetUpperBound(0) + 1;
            int columns = GameState.Length / rows;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (GameState[i, j] == 1)
                    {
                        if (i > 0 && i < 3 && j > 0 && j < 8) 
                        {
                            if ((i + 1 == DefinedRow) && (j == DefinedColumn))
                                IsAValidMove = true;
                            if ((i - 1 == DefinedRow) && (j == DefinedColumn))
                                IsAValidMove = true;
                            if ((i == DefinedRow) && (j + 1 == DefinedColumn))
                                IsAValidMove = true;
                            if ((i == DefinedRow) && (j - 1 == DefinedColumn))
                                IsAValidMove = true;
                        }

                        if (i == 0 && j == 0) 
                        {
                            if ((i + 1 == DefinedRow) && (j == DefinedColumn))
                                IsAValidMove = true;
                            if ((i == DefinedRow) && (j + 1 == DefinedColumn))
                                IsAValidMove = true;
                        }

                        if (i == 3 && j == 8) 
                        {
                            if ((i - 1 == DefinedRow) && (j == DefinedColumn))
                                IsAValidMove = true;
                            if ((i == DefinedRow) && (j - 1 == DefinedColumn))
                                IsAValidMove = true;
                        }

                        if (i == 0 && j == 8)
                        {
                            if ((i + 1 == DefinedRow) && (j == DefinedColumn))
                                IsAValidMove = true;
                            if ((i == DefinedRow) && (j - 1 == DefinedColumn))
                                IsAValidMove = true;
                        }

                        if (i > 0 && i <= 3 && j == 0) 
                        {
                            if ((i - 1 == DefinedRow) && (j == DefinedColumn))
                                IsAValidMove = true;
                            if ((i == DefinedRow) && (j + 1 == DefinedColumn))
                                IsAValidMove = true;
                        }

                        if (i == 0 && j > 0 && j <= 8)
                        {
                            if ((i== DefinedRow) && (j - 1 == DefinedColumn))
                                IsAValidMove = true;
                            if ((i + 1 == DefinedRow) && (j == DefinedColumn))
                                IsAValidMove = true;
                        }

                        // here
                        if (i >= 0 && i <= 3 && (j == 8 || j == 0)) 
                        {
                            if ((i == DefinedRow) && (j == DefinedColumn))
                                IsAValidMove = true;
                        }

                        if (i == 3 && j == 0)
                        {
                            if ((i - 1 == DefinedRow) && (j == DefinedColumn))
                                IsAValidMove = true;
                            if ((i == DefinedRow) && (j + 1 == DefinedColumn))
                                IsAValidMove = true;
                        }

                        if (i == 0 && j > 0 && j < 8) 
                        {
                            if ((i + 1 == DefinedRow) && (j == DefinedColumn))
                                IsAValidMove = true;
                            if ((i == DefinedRow) && (j + 1 == DefinedColumn))
                                IsAValidMove = true;
                            if ((i == DefinedRow) && (j - 1 == DefinedColumn))
                                IsAValidMove = true;
                        }

                        if (i > 0 && i < 3 && j == 0)
                        {
                            if ((i + 1 == DefinedRow) && (j == DefinedColumn))
                                IsAValidMove = true;
                            if ((i - 1 == DefinedRow) && (j== DefinedColumn))
                                IsAValidMove = true;
                            if ((i == DefinedRow) && (j + 1 == DefinedColumn))
                                IsAValidMove = true;
                        }

                        if (i == 3 && j > 0 && j < 8)
                        {
                            if ((i - 1 == DefinedRow) && (j == DefinedColumn))
                                IsAValidMove = true;
                            if ((i == DefinedRow) && (j - 1 == DefinedColumn))
                                IsAValidMove = true;
                            if ((i == DefinedRow) && (j + 1 == DefinedColumn))
                                IsAValidMove = true;
                        }

                        if (i > 0 && i < 3 && j == 8)
                        {
                            if ((i == DefinedRow) && (j - 1 == DefinedColumn))
                                IsAValidMove = true;
                            if ((i - 1 == DefinedRow) && (j == DefinedRow))
                                IsAValidMove = true;
                            if ((i + 1 == DefinedRow) && (j == DefinedRow))
                                IsAValidMove = true;
                        }
                    }
                }
            }

            return IsAValidMove;
        }

        private static bool CheckIfSuitAlreadyUsedForAnother(Dictionary<int, string> RowSuit, string SuitToCheck, int currentRowNumber) 
        {
            bool AlreadyUsed = false;

            // if rowsuit > 4 -> clear

            for (int i = 1; i <= 4; i++) 
            {
                if (RowSuit[i] == SuitToCheck && i != currentRowNumber) 
                {
                    AlreadyUsed = true;
                    break;
                }
            }

            return AlreadyUsed;
        }

        public static int GetCounterByCardValue(string CardValue) 
        {
            int rowNum = -1;

            switch (CardValue)
            {
                case "6":
                    rowNum = 0;
                    break;
                case "7":
                    rowNum = 1;
                    break;
                case "8":
                    rowNum = 2;
                    break;
                case "9":
                    rowNum = 3;
                    break;
                case "10":
                    rowNum = 4;
                    break;
                case "J":
                    rowNum = 5;
                    break;
                case "Q":
                    rowNum = 6;
                    break;
                case "K":
                    rowNum = 7;
                    break;
                case "A":
                    rowNum = 8;
                    break;
                default:
                    throw new ArgumentException("Incorrect card value.");
            }

            return rowNum;
        }

        public static string GetCardValueByCounter(int Counter)
        {
            string CardValue = "";

            switch (Counter) 
            {
                case 0:
                    CardValue = "6";
                    break;
                case 1:
                    CardValue = "7";
                    break;
                case 2:
                    CardValue = "8";
                    break;
                case 3:
                    CardValue = "9";
                    break;
                case 4:
                    CardValue = "10";
                    break;
                case 5:
                    CardValue = "J";
                    break;
                case 6:
                    CardValue = "Q";
                    break;
                case 7:
                    CardValue = "K";
                    break;
                case 8:
                    CardValue = "A";
                    break;
                default:
                    throw new ArgumentException("Incorrect card value.");
            }

            return CardValue;
        }

        private static int[,] GetGameState(CardPlaceholder[] firstRow, CardPlaceholder[] secondRow, CardPlaceholder[] thirdRow, CardPlaceholder[] fourthRow)
        {
            int[,] GameState = new int[4, 9];
            CardPlaceholder[] currentRow = firstRow;

            int rows = GameState.GetUpperBound(0) + 1;
            int columns = GameState.Length / rows;

            for (int i = 0; i < rows; i++) 
            {
                switch (i) 
                {
                    case 0:
                        break;
                    case 1:
                        currentRow = secondRow;
                        break;
                    case 2:
                        currentRow = thirdRow;
                        break;
                    case 3:
                        currentRow = fourthRow;
                        break;
                }

                for (int j = 0; j < columns; j++) 
                {
                    if (currentRow[j].cards[1] == null)
                    {
                        GameState[i, j] = 1;
                    }
                }
            }

            return GameState;
        }

        private static void OutputPlayersHand(List<Card> PlayersHand) 
        {
            Console.WriteLine("\n\nYour card's hand: ");
            for (int i = 0; i < PlayersHand.Count; i++) 
            {
                Console.Write($"{i}. |{PlayersHand[i].CardSuit} {PlayersHand[i].CardValue}| ");
            }
            Console.WriteLine();
        }

        private static void OutputRow(int rowNumber, CardPlaceholder[] rowToOutput) 
        {
            Console.Write($"{rowNumber}. ");
            bool IsCardAvailable = true;

            for (int i = 0; i < rowToOutput.Length; i++) 
            {
                if (rowToOutput[i].cards[1] == null)
                {
                    IsCardAvailable = false;
                }
                else 
                {
                    IsCardAvailable = true;
                }

                if (rowToOutput[i].cards[0].CardSuit == null) 
                {
                    Console.Write($"|NoSuit {rowToOutput[i].cards[0].CardValue}, {IsCardAvailable}| ");
                    continue;
                }
                Console.Write($"|{rowToOutput[i].cards[0].CardSuit} {rowToOutput[i].cards[0].CardValue}, {IsCardAvailable}| ");
            }
            Console.WriteLine();
        }

        private static CardPlaceholder[] InitializeRow(Dictionary<int, (string, string, bool)> AllCards) 
        {
            CardPlaceholder[] RowToInit = new CardPlaceholder[9];

            // initializing from 7 to king (skipping 6 and aces)
            for (int i = 0; i < RowToInit.Length; i++)
            {
                if (i == 0 || i == 8) 
                {
                    Card InitiaPlaceholderCard = new Card(Constants.possibleCardValues[i]);
                    RowToInit[i] = new CardPlaceholder(InitiaPlaceholderCard);
                    continue;
                }

                Card PlaceholderCard = new Card(Constants.possibleCardValues[i]);
                Card ShuffledCard = GetRandomCardNotUsedYet(AllCards);

                RowToInit[i] = new CardPlaceholder(PlaceholderCard, ShuffledCard);
            }

            return RowToInit;
        }

        private static Card GetRandomCardNotUsedYet(Dictionary<int, (string, string, bool)> AllCards) 
        {
            Card CardToGet = null;
            bool CardFound = false;

            Random rnd = new Random();
            int NumOfCardToTake = 0;
       
            while (!CardFound) 
            {
                NumOfCardToTake = rnd.Next(1, 36);
                var tupleOfValues = AllCards[NumOfCardToTake];

                if (!tupleOfValues.Item3) 
                {
                    CardToGet = new Card(tupleOfValues.Item1, tupleOfValues.Item2);
                    AllCards[NumOfCardToTake] = (tupleOfValues.Item1, tupleOfValues.Item2, true);
                    
                    CardFound = true;
                }
            }

            return CardToGet;
        }

        private static Card GetLeftCardNotUsedYet(Dictionary<int, (string, string, bool)> AllCards)
        {
            List<int> NotUsedValues = new(8);

            for (int i = 1; i < 37; i++)
            {
                if (!AllCards[i].Item3)
                {
                    NotUsedValues.Add(i);
                }
            }

            Random rnd = new Random();
            Card CardToGet = null;

            int RandomNum = rnd.Next(0, NotUsedValues.Count - 1);

            var tupleOfValues = AllCards[NotUsedValues[RandomNum]];
            CardToGet = new Card(tupleOfValues.Item1, tupleOfValues.Item2);
            AllCards[NotUsedValues[RandomNum]] = (tupleOfValues.Item1, tupleOfValues.Item2, true);

            return CardToGet;
        }

        private static List<Card> GenerateInitialHand(Dictionary<int, (string, string, bool)> AllCards) 
        {
            List<Card> InitialHand = new(4);

            for (int i = 0; i < InitialHand.Capacity; i++) 
            {
                Card CardToHand = GetLeftCardNotUsedYet(AllCards);
                InitialHand.Add(CardToHand);
            }

            return InitialHand;
        }
    }
}
