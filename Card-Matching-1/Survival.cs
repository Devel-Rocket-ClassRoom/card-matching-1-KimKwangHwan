using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public class Survival : CardMatchingGame
{
    private int limit = 3;
    private int survive_count;

    public override void Init(Level level, CardSkin skin)
    {
        base.Init(level, skin);
        survive_count = 0;
    }

    public override bool CheckMatching()
    {
        bool isCorrect = false;
        if (matching_cards[0] == matching_cards[1] && (!ReferenceEquals(matching_cards[0], matching_cards[1])))
        {
            correct_count++;
            matching_cards[0].ChangeState(State.Matched);
            matching_cards[1].ChangeState(State.Matched);
            isCorrect = true;
            survive_count = 0;
        }
        else
        {
            matching_cards[0].ChangeState(State.None);
            matching_cards[1].ChangeState(State.None);
            survive_count++;
        }
        try_count++;
        return isCorrect;
    }
    public override void CheckClear(out GameState game_state)
    {
        if (survive_count >= limit)
        {
            Console.WriteLine("\n=== 게임 오버! ===");
            Console.WriteLine("연속으로 3번 틀렸습니다.");
            Console.WriteLine($"찾은 쌍: {correct_count}/{deck.Length / 2}");
            game_state = GameState.Fail;
            return;
        }
        else if (correct_count >= deck.Length / 2)
        {
            Console.WriteLine("\n=== 게임 클리어! ===");
            PrintCount();
            game_state = GameState.Clear;
            return;
        }
        game_state = GameState.Gaming;
    }

    public override void PrintCount()
    {
        Console.WriteLine($"연속 실패: {survive_count}/{limit} | 찾은 쌍: {correct_count}/{deck.Length / 2}\n");
    }
}