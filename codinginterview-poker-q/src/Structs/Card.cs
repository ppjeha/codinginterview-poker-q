using codinginterview_poker_q.src.Enums;

namespace codinginterview_poker_q.src.Structs;

public readonly struct Card
{
    public Ranks Rank { get; }
    public Suits Suit { get; }

    public Card(Ranks rank, Suits suit)
    {
        Rank = rank;
        Suit = suit;
    }

    public override string ToString()
    {
        return $"{Rank.GetString()}{Suit.GetString()}";
    }
}
