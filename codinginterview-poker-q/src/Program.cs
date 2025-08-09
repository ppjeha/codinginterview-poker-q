using codinginterview_poker_q.src.Enums;
using codinginterview_poker_q.src.Struct;

List<Card> CreateDeck()
{
    var deck = (from suit in Enum.GetValues<Suits>()
                from rank in Enum.GetValues<Ranks>()
                select new Card(rank, suit)).ToList();
    
    deck = ShuffleDeck(deck);

    return deck;
}

List<Card> ShuffleDeck(List<Card> deck) //could use other shuffling algorithms, like Fisher-Yates
{
    var random = new Random();
    return deck.OrderBy(x => random.Next()).ToList();
}

List<Card> DealHand(List<Card> deck)  // key point: have to remove the cards from the deck after dealing
{
    int pokerHand = 5;
    var hand = deck.Take(pokerHand).ToList();
    deck.RemoveRange(0, pokerHand);
    return hand;
}

Scores ScoreHand(List<Card> hand)
{
    Scores score = Scores.HigherCard;

    // create an ascending list of the hand's ranks
    var handRanks = hand.Select(x => x.Rank).OrderBy(x => x).ToList();

    // check for the border cases since A is the last in my enum
    var isLowAceStraight = handRanks.SequenceEqual([Ranks.A, Ranks.Two, Ranks.Three, Ranks.Four, Ranks.Five]);
    var isRoyalStraight = handRanks.SequenceEqual([Ranks.Ten, Ranks.J, Ranks.Q, Ranks.K, Ranks.A]);

    bool isSequential =
        handRanks.SequenceEqual(Enumerable.Range((int)handRanks.Min(), handRanks.Count).Select(x => (Ranks)x))
        || isLowAceStraight
        || isRoyalStraight;

    // flush cases
    if (hand.All(x => x.Suit == Suits.Hearts) ||
        hand.All(x => x.Suit == Suits.Spades) ||
        hand.All(x => x.Suit == Suits.Clubs) ||
        hand.All(x => x.Suit == Suits.Diamonds))
    {
        score = Scores.Flush;

        if (isSequential)
        {
            score = Scores.StraightFlush;

            if (isRoyalStraight)
            {
                score = Scores.RoyalFlush;
            }
        }
    }

    // four of a kind
    if (handRanks.GroupBy(x => x).Any(g => g.Count() == 4))
        score = Scores.FourOfAKind;

    // three of a kind
    if (handRanks.GroupBy(x => x).Any(g => g.Count() == 3))
        score = Scores.ThreeOfAKind;

    // one pair
    if (handRanks.GroupBy(x => x).Any(g => g.Count() == 2))
    {
        score = Scores.OnePair;

        // two pair
        if (handRanks.GroupBy(x => x).Count(g => g.Count() == 2) == 2)
            score = Scores.TwoPair;

        // three of a kind
        if (handRanks.GroupBy(x => x).Any(g => g.Count() == 3))
            score = Scores.FullHouse;
    }

    return score;
}

void ExampleRun()
{
    var deck = CreateDeck();
    var hand = DealHand(deck);
    var score = ScoreHand(hand);

    Console.WriteLine($"Your hand: [{String.Join(", ", hand.ToString())}], Score: {score}");
}

var deck = CreateDeck();
var player1 = DealHand(deck);
var score1 = ScoreHand(player1);
var player2 = DealHand(deck);
var score2 = ScoreHand(player2);

Console.WriteLine($"Player 1 hand: [{String.Join(", ", player1)}], Score: {ScoresExtensions.GetString(score1)}");
Console.WriteLine($"Player 2 hand: [{String.Join(", ", player2)}], Score: {ScoresExtensions.GetString(score2)}");

Console.WriteLine(score1 > score2 ? "Player 1 Won." : (score1 < score2 ? "Player 2 Won." : "It's a tie."));


