using System;
using System.Collections.Generic;
using System.Text;

public enum State
{
    None = 0,
    Matching,
    Matched
}

public enum CardSkin
{
    Num = 1,
    Alphabet,
    Mark
}

public class Card
{
    private static ConsoleColor[] color_table;
    private static char[] alphabet_table;
    private static char[] mark_table;

    private State state;
    private int value;
    private char shape;

    private ConsoleColor color;

    static Card()
    {
        color_table = new ConsoleColor[]
        {
            ConsoleColor.Red,
            ConsoleColor.Green,
            ConsoleColor.Blue,
            ConsoleColor.Yellow,
            ConsoleColor.Cyan,
            ConsoleColor.Magenta,
            ConsoleColor.DarkCyan,
            ConsoleColor.DarkMagenta,
            ConsoleColor.DarkRed,
            ConsoleColor.DarkGreen,
            ConsoleColor.DarkBlue,
            ConsoleColor.DarkYellow
        };
        alphabet_table = new char[]
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L'
        };
        mark_table = new char[]
        {
            '★', // 별
            '●', // 원
            '■', // 사각형
            '▲', // 삼각형
            '♠', // 스페이드
            '♥', // 하트
            '♦', // 다이아
            '♣', // 클로버
            '▼', // 역삼각형
            '○', // 빈 원
            '♡', // 빈 하트
            '☆', // 빈 별
        };
    }

    public Card(int value, CardSkin skin = CardSkin.Mark)
    {
        state = State.None;
        this.value = value;

        if (skin == CardSkin.Num)
        {
            color = ConsoleColor.White;
            this.shape = (char)('0' + this.value);
        }
        else if (skin == CardSkin.Alphabet)
        {
            color = color_table[this.value - 1];
            this.shape = alphabet_table[this.value - 1];
        }
        else if (skin == CardSkin.Mark)
        {
            color = color_table[this.value - 1];
            this.shape = mark_table[this.value - 1];
        }
    }

    public void ChangeState(State state)
    {
        this.state = state;
    }

    public State GetState() { return this.state; }
    public static bool operator ==(Card lhs, Card rhs)
    {
        if (lhs is null && rhs is null) return true;
        if (lhs is null || rhs is null) return false;
        return lhs.value == rhs.value;
    }
    public static bool operator !=(Card lhs, Card rhs)
    {
        return !(lhs == rhs);
    }
    public override bool Equals(object obj)
    {
        if (obj is Card card)
        {
            return this == card;
        }
        return false;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(state, value, shape);
    }

    public override string ToString()
    {
        string result;
        if (state == State.None)
        {
            result = "**";
        }
        else if (state == State.Matching)
        {
            Console.ForegroundColor = color;
            result = $"[{shape}]";
        }
        else
        {
            Console.ForegroundColor = color;
            result = $"{shape}";
        }

        return result;
    }
}