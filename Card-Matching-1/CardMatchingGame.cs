using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public enum Level
{
    Easy = 1,
    Normal,
    Hard
}

public class CardMatchingGame
{
    protected Card[] deck;
    protected int correct_count;
    protected int try_limit;
    protected int try_count;
    protected Card[] matching_cards;
    protected int count = 0;
    protected int row, col;
    protected Level level;
    protected int game_start_count;
    protected int preview_seconds;
    protected CardSkin card_skin;
    public CardMatchingGame()
    {
        game_start_count = 0;
        //Init(level);
    }

    virtual public void Init(Level level, CardSkin skin)
    {
        if (game_start_count == 0 || this.level != level || card_skin != skin)
        {
            matching_cards = new Card[2];
            switch (level)
            {
                case Level.Easy:
                    try_limit = 10;
                    row = 2;
                    col = 4;
                    preview_seconds = 5;
                    deck = new Card[row * col];
                    break;
                case Level.Normal:
                    try_limit = 20;
                    row = 4;
                    col = 4;
                    preview_seconds = 3;
                    deck = new Card[row * col];
                    break;
                case Level.Hard:
                    try_limit = 30;
                    row = 4;
                    col = 6;
                    preview_seconds = 2;
                    deck = new Card[row * col];
                    break;
            }
            for (int i = 0; i < deck.Length; i++)
            {
                deck[i] = new Card(i / 2 + 1, skin);
            }
        }
        else
        {
            for (int i = 0; i < deck.Length; i++)
            {
                deck[i].ChangeState(State.None);
            }
        }
        correct_count = 0;
        try_count = 0;
        count = 0;
        Shuffle();
        game_start_count++;
        this.level = level;
        card_skin = skin;
    }

    public void Shuffle()
    {
        Console.Clear();
        Console.WriteLine("카드를 섞는 중...");
        for (int i = 0; i < deck.Length; i++)
        {
            Random rnd = new Random();
            int index = rnd.Next(0, deck.Length);
            Card temp = deck[index];
            deck[index] = deck[i];
            deck[i] = temp;
        }
        Thread.Sleep(500);
    }

    public void PrintCardArray()
    {
        Console.Clear();
        Console.WriteLine();
        Console.Write("   ");
        for (int i = 0; i < col; i++)
        {
            Console.Write($" {(i + 1)}열");
        }
        Console.WriteLine();
        for (int i = 0; i < row; i++)
        {
            Console.Write($"{i + 1}행");
            for (int j = 0; j < col; j++)
            {
                Console.Write($"{deck[i * col + j],4}");
                Console.ResetColor();
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public bool SelectCard(int x, int y)
    {
        if (x < 1 || y < 1 || x > row || y > col)
        {
            return false;
        }
        int index = (x - 1) * this.col + (y - 1);
        deck[index].ChangeState(State.Matching);
        matching_cards[count++] = deck[index];
        return true;
    }

    public virtual bool CheckMatching()
    {
        bool isCorrect = false;
        if (matching_cards[0] == matching_cards[1] && (!ReferenceEquals(matching_cards[0], matching_cards[1])))
        {
            correct_count++;
            matching_cards[0].ChangeState(State.Matched);
            matching_cards[1].ChangeState(State.Matched);
            isCorrect = true;
        }
        else
        {
            matching_cards[0].ChangeState(State.None);
            matching_cards[1].ChangeState(State.None);
        }
        try_count++;
        return isCorrect;
    }

    public void Retry()
    {
        count = 0;
    }

    public virtual void PrintCount()
    {
        Console.WriteLine($"\n시도 횟수: {try_count} | 찾은 쌍: {correct_count}/{deck.Length / 2}"); ;
    }

    public virtual void CheckClear(out GameState game_state)
    {
        if (try_count >= try_limit)
        {
            Console.WriteLine("\n=== 게임 오버! ===");
            Console.WriteLine("\n시도 횟수를 모두 사용했습니다.");
            Console.WriteLine($"총 시도 횟수: {try_count}\n");
            game_state = GameState.Fail;
            return;
        }
        else if (correct_count >= deck.Length / 2)
        {
            Console.WriteLine("\n=== 게임 클리어! ===");
            Console.WriteLine($"찾은 쌍: {correct_count}/{deck.Length / 2}\n");
            game_state = GameState.Clear;
            return;
        }
        game_state = GameState.Gaming;
    }
    public virtual void Preview()
    {
        for (int i = 0; i < deck.Length; i++)
        {
            deck[i].ChangeState(State.Matching);
        }
        PrintCardArray();
        Console.WriteLine($"\n잘 기억하세요! ({preview_seconds}초 후 뒤집힙니다)");
        Thread.Sleep(preview_seconds * 1000);
        for (int i = 0; i < deck.Length; i++)
        {
            deck[i].ChangeState(State.None);
        }
    }
}