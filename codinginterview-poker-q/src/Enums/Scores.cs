namespace codinginterview_poker_q.src.Enums;

public enum Scores  // key point: the order of the enum values matters for the score comparison
{
    HigherCard,
    OnePair,
    TwoPair,
    ThreeOfAKind,
    Straight,
    Flush,
    FullHouse,
    FourOfAKind,
    StraightFlush,
    RoyalFlush
}

public static class ScoresExtensions
{
    public static string GetString(this Scores score)
    {
        return score switch
        {
            Scores.HigherCard => "High Card",
            Scores.OnePair => "One Pair",
            Scores.TwoPair => "Two Pair",
            Scores.ThreeOfAKind => "Three of a Kind",
            Scores.Straight => "Straight",
            Scores.Flush => "Flush",
            Scores.FullHouse => "Full House",
            Scores.FourOfAKind => "Four of a Kind",
            Scores.StraightFlush => "Straight Flush",
            Scores.RoyalFlush => "Royal Flush",
            _ => throw new ArgumentException("Score value not valid."),
        };
    }
}
