namespace codinginterview_poker_q.src.Enums;

public enum Ranks // key point: A is higher, checks are for low-A
{
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    J,
    Q,
    K,
    A   
}

public static class RanksExtensions
{
    public static string GetString(this Ranks rank)
    {
        return rank switch
        {
            Ranks.A => "A",
            Ranks.Two => "2",
            Ranks.Three => "3",
            Ranks.Four => "4",
            Ranks.Five => "5",
            Ranks.Six => "6",
            Ranks.Seven => "7",
            Ranks.Eight => "8",
            Ranks.Nine => "9",
            Ranks.Ten => "10",
            Ranks.J => "J",
            Ranks.Q => "Q",
            Ranks.K => "K",
            _ => throw new ArgumentException("Rank value not valid."),
        };
    }

    public static Ranks FromString(string rank)
    {
        return rank switch
        {
            "A" => Ranks.A,
            "2" => Ranks.Two,
            "3" => Ranks.Three,
            "4" => Ranks.Four,
            "5" => Ranks.Five,
            "6" => Ranks.Six,
            "7" => Ranks.Seven,
            "8" => Ranks.Eight,
            "9" => Ranks.Nine,
            "10" => Ranks.Ten,
            "J" => Ranks.J,
            "Q" => Ranks.Q,
            "K" => Ranks.K,
            _ => throw new ArgumentException("Rank representation not valid."),
        };
    }
}
