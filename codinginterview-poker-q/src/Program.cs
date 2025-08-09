using codinginterview_poker_q.src.Enums;
using codinginterview_poker_q.src.Structs;

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

List<List<Card>> DealHand(List<Card> deck)  // key point: have to remove the cards from the deck after dealing
{
    var hands = new List<List<Card>>
    {
        new List<Card>(),
        new List<Card>(),
        new List<Card>(),
        new List<Card>()
    };

    for (int cardIndex = 0; cardIndex < 5; cardIndex++)
    {
        for (int player = 0; player < 4; player++)
        {
            hands[player].Add(deck[0]);
            deck.RemoveAt(0);
        }
    }

    return hands;
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
    var score = ScoreHand(hand[0]);

    Console.WriteLine($"Your hand: [{String.Join(", ", hand.ToString())}], Score: {score}");
}

var deck = CreateDeck();

var hands = DealHand(deck);

var scoreHands = hands.Select(hand => ScoreHand(hand)).ToList(); //list of scores for each hand

var maxScore = scoreHands.Max(); //get the maximum score

var winners = scoreHands
    .Select((score, index) => new { score, index })
    .Where(x => x.score == maxScore)
    .Select(x => x.index)
    .ToList(); // list of winner indexes

if (winners.Count == 1)
{
    Console.WriteLine($"Player {winners[0] + 1} wins. Hand: [{String.Join(", ", hands[winners[0]])}], Score: {maxScore.GetString()}");
}
else
{
    Console.WriteLine($"Tie between players: {String.Join(", ", winners.Select(x => x + 1))}");
    winners.ForEach(winner =>
        Console.WriteLine($"Player {winner + 1} hand: [{String.Join(", ", hands[winner])}], Score: {maxScore.GetString()}")
    );
}