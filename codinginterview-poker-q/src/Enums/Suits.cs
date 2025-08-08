namespace codinginterview_poker_q.src.Enums;

public enum Suits
{
    Hearts,
    Diamonds,
    Clubs,
    Spades,
}

public static class SuitsExtensions
{
    public static string GetString(this Suits suit)
    {
        return suit switch
        {
            Suits.Hearts => "H",
            Suits.Diamonds => "D",
            Suits.Clubs => "C",
            Suits.Spades => "S",
            _ => throw new ArgumentException("Suit value not valid."),
        };
    }

    public static Suits FromString(string suit)
    {
        return suit switch
        {
            "H" => Suits.Hearts,
            "D" => Suits.Diamonds,
            "C" => Suits.Clubs,
            "S" => Suits.Spades,
            _ => throw new ArgumentException("Suit representation not valid."),
        };
    }
}
