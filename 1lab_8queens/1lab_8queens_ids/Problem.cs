namespace _1lab_8queens_ids
{
    static class Problem
    {
        //finding queens by columns
        public static int[] findQueensByColumns(int[,] board)
        {
            int[] queenCoordinates = new int[16];
            int arrCounter = 0;

            for (int column = 0; column < 8; column++)
            {
                for (int row = 0; row < 8; row++)
                {
                    if (board[row, column] == 1)
                    {
                        //координата по рядку
                        queenCoordinates[arrCounter] = row;
                        arrCounter++;

                        //координата по столбцу
                        queenCoordinates[arrCounter] = column;
                        arrCounter++;
                    }
                }

            }
            
            return queenCoordinates;
        }

        // finding first queen
        public static int[] findQueens(int[,] board)
        {
            int[] queenCoordinates = new int[16];
            int arrCounter = 0;

            for (int i = 0; i < Board.boardRows; i++)
            {
                for (int j = 0; j < Board.boardColumns; j++)
                {
                    if (board[i, j] == 1)
                    {
                        //координата по рядку 
                        queenCoordinates[arrCounter] = i;
                        arrCounter++;

                        //координата по столбцу
                        queenCoordinates[arrCounter] = j;
                        arrCounter++;
                    }
                }
            }
            return queenCoordinates;
        }

        //is a goal state check
        public static bool isAGoal(int[,] state)
        {
            // массив из координатов, 0 - координата ряда, 1 - координата столбца и так далее (8 вершин, 16 элементов у массива)
            int[] queensCoordinates = Problem.findQueens(state);

            int i = 0;
            while (i < 16)
            {
                bool isGoal = Problem.isAGoalQueen(state, queensCoordinates[i], queensCoordinates[i + 1]);
                
                if (isGoal == false)
                {
                    return false;
                }

                i += 2;
            }

            return true;
        }

        //is a goal queen check
        public static bool isAGoalQueen(int[,] currentState, int row, int column)
        {
            int rows = currentState.GetUpperBound(0) + 1;
            int columns = currentState.GetUpperBound(1) + 1;

            // проверка на то, не перекрывают ли ферзи друг друга по вертикали справа от взятого ферзя
            // не особо полезный, так как у нас 1 ферзь на вертикаль, но пускай будет
            for (int i = row + 1; i < rows; i++)
            {
                if (currentState[i, column] == 1)
                {
                    return false;
                }
            }

            // проверка на то, не перекрывают ли ферзи друг друга по горизонтали вправо от ферзи
            for (int j = column + 1; j < columns; j++)
            {
                if (currentState[row , j] == 1)
                {
                    return false;
                }
            }

            // проверка на то, не перекрывают ли ферзи друг друга по горизонтали влево от фрези
            for (int j = column - 1; j >= 0; j--)
            {
                if (currentState[row, j] == 1)
                {
                    return false;
                }
            }

            // проверка на то, не перекрывают ли ферзи друг друга по диагонали
            int counter = 1;

            while (counter < 9)
            {
                // левая верхняя
                if ( (row - counter > -1) && (column - counter > -1) && 
                    (currentState[row - counter, column - counter] == 1) )
                {
                    return false;
                }

                // правая верхняя
                if ((row - counter > -1) && (column + counter < 8) &&
                    (currentState[row - counter, column + counter] == 1))
                {
                    return false;
                }

                // левая нижняя
                if ( (row + counter < 8) && (column - counter > -1) &&
                    (currentState[row + counter, column - counter] == 1) )
                {
                    return false;
                }
                 
                // правая нижняя
                if ( (row + counter < 8) && (column + counter < 8) &&
                    (currentState[row + counter, column + counter] == 1) )
                {
                    return false;
                }

                counter++;
            }

            return true;
        }

        //conflicts count by state
        public static int conflictsCount(int[,] state)
        {
            // массив из координатов, 0 - координата ряда, 1 - координата столбца и так далее (8 вершин, 16 элементов у массива)
            int[] queensCoordinates = Problem.findQueensByColumns(state);

            int conflictsCount = 0; 

            int i = 0;
            while (i < 16)
            {
                int conflictsCountByQueen = Problem.conflictsCountByQueen(state, queensCoordinates[i], queensCoordinates[i + 1]);

                conflictsCount += conflictsCountByQueen;

                i += 2;
            }

            // делим на 2, так как это сумма всей конфликтов по одному ферзю (тот же самый конфликт может считатся за два)
            // так как мы смотрим конфликты по одной ферзи отдельно
            return conflictsCount / 2; 
        }

        public static int conflictsCountByQueen(int[,] currentState, int row, int column)
        {
            int rows = currentState.GetUpperBound(0) + 1;
            int columns = currentState.GetUpperBound(1) + 1;

            int conflictsByQueenCounter = 0;

            // проверка на то, не перекрывают ли ферзи друг друга по вертикали справа от взятого ферзя
            // не особо полезный, так как у нас 1 ферзь на вертикаль, но пускай будет
            for (int i = row + 1; i < rows; i++)
            {
                if (currentState[i, column] == 1)
                {
                    conflictsByQueenCounter++;
                }
            }

            // проверка на то, не перекрывают ли ферзи друг друга по горизонтали вправо от ферзи
            for (int j = column + 1; j < columns; j++)
            {
                if (currentState[row, j] == 1)
                {
                    conflictsByQueenCounter++;
                }
            }

            // проверка на то, не перекрывают ли ферзи друг друга по горизонтали влево от фрези
            for (int j = column - 1; j >= 0; j--)
            {
                if (currentState[row, j] == 1)
                {
                    conflictsByQueenCounter++;
                }
            }

            // проверка на то, не перекрывают ли ферзи друг друга по диагонали
            int counter = 1;

            while (counter < 9)
            {
                // левая верхняя
                if ((row - counter > -1) && (column - counter > -1) &&
                    (currentState[row - counter, column - counter] == 1))
                {
                    conflictsByQueenCounter++;
                }

                // правая верхняя
                if ((row - counter > -1) && (column + counter < 8) &&
                    (currentState[row - counter, column + counter] == 1))
                {
                    conflictsByQueenCounter++;
                }

                // левая нижняя
                if ((row + counter < 8) && (column - counter > -1) &&
                    (currentState[row + counter, column - counter] == 1))
                {
                    conflictsByQueenCounter++;
                }

                // правая нижняя
                if ((row + counter < 8) && (column + counter < 8) &&
                    (currentState[row + counter, column + counter] == 1))
                {
                    conflictsByQueenCounter++;
                }

                counter++;
            }

            return conflictsByQueenCounter;
        }
        
    }
}
