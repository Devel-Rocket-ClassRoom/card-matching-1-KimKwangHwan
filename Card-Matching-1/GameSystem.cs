using System;
using System.Collections.Generic;
using System.Text;

public enum GameState
{
    Gaming,
    Fail,
    Clear
}
public enum GameMode
{
    Classic = 1,
    TimeAttack,
    Survival
}
public class GameSystem
{
    CardMatchingGame cmg;
    GameState game_state;
    public void Start()
    {
        Console.WriteLine("=== 카드 짝 맞추기 게임 ===\n");
        
        while(true)
        {
            Console.WriteLine("게임 모드를 선택하세요:");
            Console.WriteLine("1. 클래식");
            Console.WriteLine("2. 타임어택");
            Console.WriteLine("3. 서바이벌");
            Console.Write("선택: ");

            int mode;
            if (Int32.TryParse(Console.ReadLine(), out mode))
            {
                if (mode < 1 || mode > 3) continue;
            }
            else continue;

            switch ((GameMode)mode)
            {
                case GameMode.Classic:
                    cmg = new CardMatchingGame();
                    break;
                case GameMode.TimeAttack:
                    cmg = new TimeAttack();
                    break;
                case GameMode.Survival:
                    cmg = new Survival();
                    break;
            }

            
            game_state = GameState.Gaming;
            Console.WriteLine("\n난이도를 선택하세요:");
            Console.WriteLine("1. 쉬움 (2x4)");
            Console.WriteLine("2. 보통 (4x4)");
            Console.WriteLine("3. 어려움 (4x6)");
            Console.Write("선택: ");
            int level;
            if (Int32.TryParse(Console.ReadLine(), out level))
            {
                if (level < 1 || level > 3) continue;
            }
            else continue;

            Console.WriteLine("\n카드 스킨을 선택하세요:");
            Console.WriteLine("1. 숫자 (기본)");
            Console.WriteLine("2. 알파벳 (컬러)");
            Console.WriteLine("3. 기호 (컬러)");
            Console.Write("선택: ");
            int skin;

            if (Int32.TryParse((Console.ReadLine()), out skin))
            {
                if (level < 1 || level > 3) continue;
            }
            else continue;

            cmg.Init((Level)level, (CardSkin)skin);
            cmg.Preview();
            while (game_state == GameState.Gaming)
            {
                Console.Clear();
                cmg.Retry();
                cmg.PrintCardArray();
                cmg.PrintCount();
                Console.WriteLine("첫 번째 카드를 선택하세요 (행 열): ");
                string[] input = Console.ReadLine().Split(' ');
                int row;
                int col;
                if (Int32.TryParse(input[0], out row) && Int32.TryParse(input[1], out col))
                {
                    if (!cmg.SelectCard(row, col))
                    {
                        Console.WriteLine("\n범위 밖의 카드를 고르셨습니다. 다시 시도하세요!\n");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("다시 입력하세요.");
                    continue;
                }
                cmg.PrintCardArray();
                Console.WriteLine("두 번째 카드를 선택하세요 (행 열): ");
                input = Console.ReadLine().Split(' ');
                int row2, col2;
                if (Int32.TryParse(input[0], out row2) && Int32.TryParse(input[1], out col2))
                {
                    if (row == row2 && col == col2)
                    {
                        Console.WriteLine("\n같은 카드를 고르셨습니다. 다시 시도하세요!\n");
                        continue;
                    }
                    if (!cmg.SelectCard(row2, col2))
                    {
                        Console.WriteLine("\n범위 밖의 카드를 고르셨습니다. 다시 시도하세요!\n");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("다시 입력하세요.");
                    continue;
                }
                cmg.PrintCardArray();
                cmg.PrintCount();
                if (cmg.CheckMatching())
                {
                    Console.WriteLine("\n짝을 찾았습니다!");
                }
                else
                {
                    Console.WriteLine("\n짝이 맞지 않습니다!");
                }
                cmg.CheckClear(out game_state);
            }

            //if (game_state == GameState.Fail)
            //{
            //    Console.WriteLine("\n시도 횟수를 모두 사용했습니다.");
            //    cmg.PrintFindCount();
            //}
            //else if (game_state == GameState.Clear)
            //{
            //    Console.WriteLine("\n=== 게임 클리어! ===");
            //    cmg.PrintTotalTryCount();
            //}

            Console.Write("\n새 게임을 하시겠습니까? (Y/N): ");

            string YN = Console.ReadLine();

            if (YN == "N" || YN == "n")
            {
                break;
            }
        }
    }
}