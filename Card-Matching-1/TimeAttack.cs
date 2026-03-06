using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public class TimeAttack : CardMatchingGame
{
    private int time_limit;
    private int elapsedSeconds;
    private Timer timer = null;
    private bool time_out;

    private void OnTick(object state)
    {
        elapsedSeconds++;

        if (elapsedSeconds >= time_limit)
        {
            timer.Dispose();
            time_out = true;
        }
    }

    public override void Init(Level level, CardSkin skin)
    {
        base.Init(level, skin);
        if (this.level == Level.Easy)
        {
            time_limit = 60;
        }
        else if (this.level == Level.Normal)
        {
            time_limit = 90;
        }
        else if (this.level == Level.Hard)
        {
            time_limit = 120;
        }
        elapsedSeconds = 0;
        time_out = false;
    }

    public override void CheckClear(out GameState game_state)
    {
        if (time_out)
        {
            Console.WriteLine("\n=== 게임 오버! ===");
            Console.WriteLine("제한 시간을 초과했습니다!");
            Console.WriteLine($"찾은 쌍: {correct_count}/{deck.Length / 2}");
            game_state = GameState.Fail;
            return;
        }
        else if (correct_count >= deck.Length / 2)
        {
            timer.Dispose();
            Console.WriteLine("\n=== 게임 클리어! ===");
            PrintCount();
            game_state = GameState.Clear;
            return;
        }
        game_state = GameState.Gaming;
    }

    public override void PrintCount()
    {
        Console.WriteLine($"경과 시간: {elapsedSeconds}초 / {time_limit}초 | 찾은 쌍: {correct_count}/{deck.Length / 2}\n");
    }

    public override void Preview()
    {
        base.Preview();
        timer = new Timer(OnTick, null, 1000, 1000);
    }
}