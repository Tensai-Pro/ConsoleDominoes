using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Domino
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(120, 35);
            StartGame();
        }

        static void StartGame()
        {
            while(true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Clear();
                for (int i = 0; i < 60; i++)
                {
                    Console.Write("-");
                    Console.Write("_");
                }
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                string asterisk = new string('*', 120);
                Console.WriteLine("\n\n\n\n{0}", asterisk);
                Console.SetCursorPosition(53, 7);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("DOMINO");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("\n{0}", asterisk);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("{0, 65}", "Начать новую игру?");
                Console.WriteLine("\n{0, 60}", "Yes / No");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(0, 22);
                for (int i = 0; i < 60; i++)
                {
                    Console.Write("-");
                    Console.Write("_");
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(56, 14);
                ConsoleKeyInfo pressedKey = Console.ReadKey();
                if (pressedKey.Key == ConsoleKey.Y)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("\n\n{0, 69}", "Загружается новая игра...");
                    Thread.Sleep(1700);
                    RunGame();
                }
                else if (pressedKey.Key == ConsoleKey.N)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("\n\n{0, 65}", "Выходим из игры...");
                    Thread.Sleep(1700);
                    return;
                }
            }
        }

        static void RunGame()
        {
            Stack<string> pile = DominoSet.Shuffle();             // Перемешиваем "кучу"

            DominoField field = new DominoField();                // Генерируем новое поле

            Players you = new Players(pile);                      // Создаем руку игрока
            Players computer = new Players(pile);                 // Создаем руку компьютера

            GameProcess newGame = new GameProcess();              // Генерируем новую игру

            byte turn = Turn.WhoIsFirst(you, computer);       // Определяем, кто ходит первый
            if (turn == 1)                                    // Делаем первый ход
            {
                Console.WriteLine("\n\n{0, 63}", "Первый ход ваш");
                Console.WriteLine("{0, 69}", "Нажмите, чтобы продолжить.");
                Console.ReadKey();
                newGame.FirstMove(turn, you, field);
                turn++;
            }
            else
            {
                Console.WriteLine("\n\n{0, 66}", "Первый ход компьютера");
                Console.WriteLine("{0, 69}", "Нажмите, чтобы продолжить.");
                Console.ReadKey();
                newGame.FirstMove(turn, computer, field);
                turn--;
            }

            while (you.Hand.Count > 0 && computer.Hand.Count > 0 && (pile.Count > 0 || newGame.CheckAvailability(you.Hand) || newGame.CheckAvailability(computer.Hand)))    // Весь процесс игры
            {
                if (turn == 1)
                {
                    newGame.Move(ref turn, you, pile, field);
                }
                else
                {
                    newGame.Move(ref turn, computer, pile, field);
                }
            }

            EndGame(you, computer, field);
        }

        static void EndGame(Players player1, Players player2, DominoField field) 
        {
            string asterisk = new string('*', 120);

            if (player1.Hand.Count == 0)
            {
                field.DisplayField();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(asterisk);

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("{0, 61}", "GAME OVER\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("{0, 62}", "Вы победили!");
                Console.WriteLine("{0, 67}", "ИИ потерпел поражение\n");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("{0, 71}", "Нажмите, чтобы продолжить...\n");

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(asterisk);

                Console.ReadKey();
            }
            else if (player2.Hand.Count == 0)
            {
                field.DisplayField();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(asterisk);

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("{0, 61}", "GAME OVER\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("{0, 62}", "Вы проиграли!");
                Console.WriteLine("{0, 63}", "ИИ одолел вас\n");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("{0, 71}", "Нажмите, чтобы продолжить...\n");

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(asterisk);

                Console.ReadKey();
            }
            else
            {
                field.DisplayField();
                Console.WriteLine("Ни у кого нет доступных костей.\n");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(asterisk);

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("{0, 61}", "GAME OVER\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("{0, 59}", "Ничья!");
                Console.WriteLine("{0, 73}", "Никто не смог выложить свои кости\n");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("{0, 71}", "Нажмите, чтобы продолжить...\n");

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(asterisk);

                Console.ReadKey();
            }
        }
    }
}
