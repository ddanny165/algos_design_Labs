namespace _6lab_nochkaGame
{
    public class CardPlaceholder
    {
        public Card[] cards;

        public CardPlaceholder(Card PlaceHolderCard)
        {
            cards = new Card[2];
            cards[0] = PlaceHolderCard;
            cards[1] = null;
        }

        public CardPlaceholder(Card PlaceHolderCard, Card CardToTake) 
        {
            cards = new Card[2];
            cards[0] = PlaceHolderCard;
            cards[1] = CardToTake;
        }
    }
}
