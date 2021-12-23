using System.Collections.Generic;

namespace _6lab_nochkaGame
{
    public static class StateHolders
    {
        // rowNum and its suit
        public static Dictionary<int, string> RowSuit = new() 
        {
            {1, null },
            {2, null },
            {3, null },
            {4, null }
        };

        public static List<Node> NextStates = new();

        public static List<Card> PlacedOnBoardCards = new();

        public static List<Card> PutCards = new();

        public static int sixRowCounter = 0;
        public static int ARowCounter = 0;
    }
}
