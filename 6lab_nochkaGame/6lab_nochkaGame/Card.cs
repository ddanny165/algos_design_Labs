namespace _6lab_nochkaGame
{
    public class Card
    {
        public string CardValue { get; private set; }
        public string CardSuit { get; set; }

        public Card(string CardValue)
        {
            this.CardValue = CardValue;
            this.CardSuit = null;
        }

        public Card(string CardValue, string CardSuit)
        {
            this.CardValue = CardValue;
            this.CardSuit = CardSuit;
        }
    }
}
