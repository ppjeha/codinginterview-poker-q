using codinginterview_poker_q.src.Enums;

List<string> CreateDeck()
{
    var deck = (from suit in Enum.GetValues<Suits>()
                from rank in Enum.GetValues<Ranks>()
                select $"{rank.GetString()}{suit.GetString()}").ToList();
    
    deck = ShuffleDeck(deck);

    return deck;
}

List<string> ShuffleDeck(List<string> deck) //could use other shuffling algorithms, like Fisher-Yates
{
    var random = new Random();
    return deck.OrderBy(x => random.Next()).ToList();
}

List<string> DealHand(List<string> deck)  // key point: have to remove the cards from the deck after dealing
{
    int pokerHand = 5;
    var hand = deck.Take(pokerHand).ToList();
    deck.RemoveRange(0, pokerHand);
    return hand;
}

Scores ScoreHand(List<string> hand)
{
    Scores score = Scores.HigherCard;

    // create an ascending list of the hand's ranks
    var handRanks = hand.Select(x => RanksExtensions.FromString(x[..^1])).OrderBy(x => x).ToList();

    // check for the border cases since A is the last in my enum
    var isLowAceStraight = handRanks.SequenceEqual([Ranks.A, Ranks.Two, Ranks.Three, Ranks.Four, Ranks.Five]);
    var isRoyalStraight = handRanks.SequenceEqual([Ranks.Ten, Ranks.J, Ranks.Q, Ranks.K, Ranks.A]);

    bool isSequential =
        handRanks.SequenceEqual(Enumerable.Range((int)handRanks.Min(), handRanks.Count).Select(x => (Ranks)x))
        || isLowAceStraight
        || isRoyalStraight;

    // flush cases
    if (hand.All(x => x.Contains(Suits.Hearts.GetString())) ||
        hand.All(x => x.Contains(Suits.Spades.GetString())) ||
        hand.All(x => x.Contains(Suits.Clubs.GetString())) ||
        hand.All(x => x.Contains(Suits.Diamonds.GetString())))
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

    // four of a Kind
    if (handRanks.GroupBy(x => x).Any(g => g.Count() == 4))
        score = Scores.FourOfAKind;

    // three of a Kind
    if (handRanks.GroupBy(x => x).Any(g => g.Count() == 3))
        score = Scores.ThreeOfAKind;

    // one Pair
    if (handRanks.GroupBy(x => x).Any(g => g.Count() == 2))
    {
        score = Scores.OnePair;

        // two Pair
        if (handRanks.GroupBy(x => x).Count(g => g.Count() == 2) == 2)
            score = Scores.TwoPair;

        // three of a Kind
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

    Console.WriteLine($"Your hand: [{String.Join(", ", hand)}], Score: {score}");
}

var deck = CreateDeck();
var player1 = DealHand(deck);
var score1 = ScoreHand(player1);
var player2 = DealHand(deck);
var score2 = ScoreHand(player2);

Console.WriteLine($"Player 1 hand: [{String.Join(", ", player1)}], Score: {ScoresExtensions.GetString(score1)}");
Console.WriteLine($"Player 2 hand: [{String.Join(", ", player2)}], Score: {ScoresExtensions.GetString(score2)}");

Console.WriteLine(score1 > score2 ? "Player 1 Won." : (score1 < score2 ? "Player 2 Won." : "It's a tie."));


