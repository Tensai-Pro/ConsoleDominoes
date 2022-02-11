using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino
{
    class GameProcess
    {
        int value_left = 0;             // Текущее левое доступное значение 
        int value_right = 0;            // Текущая правое доступное значение
        char side;                      // Сторона, в которую ставится кость
        byte vertPosition_left = 0;     // Определяет четную (0)/нечетную (1) позицию по вертикали (движение против часовой стрелки)
        byte vertPosition_right = 0;    // Определяет четную (0)/нечетную (1) позицию по вертикали (движение по часовой стрелке) 

        public void FirstMove(byte turn, Players player, DominoField field)
        {
            field.DisplayField();

            // Действия для игрока
            if (turn == 1)
            {
                Console.WriteLine("\n{0, 11}\n", "Ваш ход");
                for (int i = 1; i <= player.Hand.Count; i++)            // Отображение всех костей игрока
                {
                    Console.WriteLine("{0}) {1}", i, player.Hand[i - 1]);
                }
                Choose();

                void Choose()
                {
                    Console.Write("\nВыберите кость: ");

                    string n = Console.ReadLine();                  // Проверка на корректность вводимого значения
                    int num;
                    bool isNum = int.TryParse(n, out num);

                    if (isNum && num > 0 && num <= player.Hand.Count)
                    {
                        Console.Write("Вы выбрали костяшку ");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(player.Hand[num - 1]);

                        string[] values = player.Hand[num - 1].Split(new char[] { '<', ':', '>' }, StringSplitOptions.RemoveEmptyEntries);
                        value_left = int.Parse(values[0]);
                        value_right = int.Parse(values[1]);

                        field.SetDomino(player.Hand[num - 1]);
                        player.Hand.RemoveAt(num - 1);
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Пожалуйста, введите номер выбранной кости.");
                        Choose();
                    }       
                }
            }
            // Действия для компьютера
            else
            {
                Console.WriteLine("\n{0, 18}\n", "Ход компьютера");
                /*
                for (int i = 1; i <= player.Hand.Count; i++)            // Отображение всех костей игрока
                {
                    Console.WriteLine("{0}) {1}", i, player.Hand[i - 1]);
                }
                */
                Random rnd = new Random();
                int random = rnd.Next(0, player.Hand.Count);
                Console.Write("\nИИ выбрал костяшку ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(player.Hand[random]);

                string[] values = player.Hand[random].Split(new char[] { '<', ':', '>' }, StringSplitOptions.RemoveEmptyEntries);
                value_left = int.Parse(values[0]);
                value_right = int.Parse(values[1]);

                field.SetDomino(player.Hand[random]);
                player.Hand.RemoveAt(random);
                Console.ReadKey();
            }
        }

        public void Move(ref byte turn, Players player, Stack<string> pile, DominoField field)
        {
            field.DisplayField();

            // Действия для игрока
            if (turn == 1)                  
            {
                Console.WriteLine("\n{0, 11}\n", "Ваш ход");
                for (int i = 1; i <= player.Hand.Count; i++)              // Отображение всех костей игрока
                {
                    Console.WriteLine("{0}) {1}", i, player.Hand[i - 1]);
                }

                if (CheckAvailability(player.Hand))
                {
                    Choose(player, field); 
                    turn++;
                }
                else
                {
                    if (pile.Count > 0)
                    {
                        Console.WriteLine("\nУ вас нет доступных костей.\nНажмите, чтобы взять кость из резерва.");
                        Console.ReadKey();
                        player.Hand.Add(pile.Pop());

                        while (pile.Count > 0 && CheckApprop(player.Hand[player.Hand.Count - 1]) == false)
                        {
                            field.DisplayField();

                            Console.WriteLine("\n{0, 11}\n", "Ваш ход");
                            for (int i = 1; i <= player.Hand.Count; i++)
                            {
                                Console.WriteLine("{0}) {1}", i, player.Hand[i - 1]);
                            }

                            Console.WriteLine("\nУ вас нет доступных костей.\nНажмите, чтобы взять кость из резерва.");
                            Console.ReadKey();
                            player.Hand.Add(pile.Pop());
                        }

                        if (CheckApprop(player.Hand[player.Hand.Count - 1]))
                        {
                            Move(ref turn, player, pile, field);
                        }
                        else
                        {
                            field.DisplayField();

                            Console.WriteLine("\n{0, 11}\n", "Ваш ход");
                            for (int i = 1; i <= player.Hand.Count; i++)
                            {
                                Console.WriteLine("{0}) {1}", i, player.Hand[i - 1]);
                            }

                            Console.WriteLine("\nУ вас нет доступных костей. Резерв пуст.\nНажмите, чтобы продолжить.");
                            Console.ReadKey();
                            turn++;
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nУ вас нет доступных костей. Резерв пуст.\nНажмите, чтобы продолжить.");
                        Console.ReadKey();
                        turn++;
                    }
                }
            }
            // Действия для компьютера
            else
            {
                Console.WriteLine("\n{0, 18}\n", "Ход компьютера");
                /*
                for (int i = 1; i <= player.Hand.Count; i++)              // Отображение всех костей игрока
                {
                    Console.WriteLine("{0}) {1}", i, player.Hand[i - 1]);
                }
                */

                if (CheckAvailability(player.Hand))
                {
                    for (int i = 0; i < player.Hand.Count; i++)
                    {
                        if (CheckApprop(player.Hand[i]))
                        {
                            Console.Write("\nИИ выбрал костяшку ");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine(player.Hand[i]);

                            ChangeValues(player.Hand[i], field);                       // Меняем текущие позиции и отправляем кость в метод установки на поле
                            player.Hand.RemoveAt(i);                            // Удаляем используемую кость
                            Console.ReadKey();
                            turn--;
                            return;
                        }
                    }
                }
                else
                {
                    if (pile.Count > 0)
                    {
                        Console.WriteLine("\nУ компьютера нет доступных костей.\nОн берет из резерва.");
                        Console.ReadKey();
                        player.Hand.Add(pile.Pop());

                        while (pile.Count > 0 && CheckApprop(player.Hand[player.Hand.Count - 1]) == false)
                        {
                            field.DisplayField();
                            Console.WriteLine("\n{0, 18}\n", "Ход компьютера");
                            /*
                            for (int i = 1; i <= player.Hand.Count; i++)
                            {
                                Console.WriteLine("{0}) {1}", i, player.Hand[i - 1]);
                            }
                            */
                            Console.WriteLine("\nУ компьютера нет доступных костей.\nОн берет из резерва.");
                            Console.ReadKey();
                            player.Hand.Add(pile.Pop());
                        }

                        if (CheckApprop(player.Hand[player.Hand.Count - 1]))
                        {
                            Move(ref turn, player, pile, field);
                        }
                        else
                        {
                            field.DisplayField();

                            Console.WriteLine("\n{0, 18}\n", "Ход компьютера");
                            /*
                            for (int i = 1; i <= player.Hand.Count; i++)
                            {
                                Console.WriteLine("{0}) {1}", i, player.Hand[i - 1]);
                            }
                            */
                            Console.WriteLine("\nУ компьютера нет доступных костей. Резерв пуст.\nНажмите, чтобы продолжить.");
                            Console.ReadKey();
                            turn--;
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nУ компьютера нет доступных костей. Резерв пуст.\nНажмите, чтобы продолжить.");
                        Console.ReadKey();
                        turn--;
                    }
                }
            }
        }


        void Choose(Players player, DominoField field)
        {
            Console.Write("\nВыберите кость: ");

            string n = Console.ReadLine();                  // Проверка на корректность вводимого значения
            int num;
            bool isNum = int.TryParse(n, out num);

            if (isNum && num > 0 && num <= player.Hand.Count)
            {
                if (CheckApprop(player.Hand[num - 1]))
                {
                    Console.Write("Вы выбрали костяшку ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(player.Hand[num - 1]);
                    ChangeValues(player.Hand[num - 1], field);                       // Меняем текущие позиции и отправляем кость в метод установки на поле
                    player.Hand.RemoveAt(num - 1);                            // Удаляем используемую кость
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Выбранная кость не подходит. Попробуйте еще раз.");
                    Choose(player, field);
                }
            }
            else
            {
                Console.WriteLine("Пожалуйста, введите номер выбранной кости.");
                Choose(player, field);
            }
        }

        
        string FlipDomino(string domino)            // Перевернуть костяшку
        {
            char[] domArr = new char[5];                
            for (int i = 0; i < domino.Length; i++)
            {
                domArr[i] = domino[i];
            }
            char swap = domArr[3];
            domArr[3] = domArr[1];
            domArr[1] = swap;
            return new string(domArr);
        }

        void ChangeValues(string domino, DominoField field)
        {
            string[] values = domino.Split(new char[] { '<', ':', '>' }, StringSplitOptions.RemoveEmptyEntries);     // Хранение первого и второго значения доминошки

            // Если левый элемент кости равен текущей левой позиции
            if (int.Parse(values[0]) == value_left)
            {
                // Движение по верхней горизонтали
                if (field.horiz_left < 4)
                {
                    value_left = int.Parse(values[1]);
                    side = 'L';

                    domino = FlipDomino(domino);        // Переворачиваем костяшку

                    field.SetDomino(domino, side);
                }
                // Движение по вертикали
                else if ((field.horiz_left == 4 && field.vertic_left < 7) || field.horiz_left == 13)
                {
                    if (vertPosition_left == 0)     // позиция четная
                    {
                        value_left = int.Parse(values[1]);
                        side = 'L';

                        field.SetDomino(domino, side);

                        vertPosition_left++;
                    }
                    else     // позиция нечетная
                    {
                        value_left = int.Parse(values[1]);
                        side = 'L';

                        domino = FlipDomino(domino);        // Переворачиваем костяшку

                        field.SetDomino(domino, side);

                        vertPosition_left--;
                    }
                }
                // Движение по нижней горизонтали
                else if (field.vertic_left == 7 && field.horiz_left < 13)
                {
                    value_left = int.Parse(values[1]);
                    side = 'L';

                    field.SetDomino(domino, side);
                }
            }
            // Если левый элемент кости равен текущей правой позиции
            else if (int.Parse(values[0]) == value_right)
            {
                // Движение по верхней горизонтали
                if (field.horiz_right < 5)
                {
                    value_right = int.Parse(values[1]);
                    side = 'R';

                    field.SetDomino(domino, side);
                }
                // Движение по вертикали
                else if ((field.horiz_right == 5 && field.vertic_right < 7) || field.horiz_right == -4)
                {
                    if (vertPosition_right == 0)     // позиция четная
                    {
                        value_right = int.Parse(values[1]);
                        side = 'R';

                        domino = FlipDomino(domino);        // Переворачиваем костяшку

                        field.SetDomino(domino, side);

                        vertPosition_right++;
                    }
                    else     // позиция нечетная
                    {
                        value_right = int.Parse(values[1]);
                        side = 'R';

                        field.SetDomino(domino, side);

                        vertPosition_right--;
                    }
                }
                // Движение по нижней горизонтали
                else if (field.vertic_right == 7 && field.horiz_right > -4)
                {
                    value_right = int.Parse(values[1]);
                    side = 'R';

                    domino = FlipDomino(domino);        // Переворачиваем костяшку

                    field.SetDomino(domino, side);
                }
            }
            // Если правый элемент кости равен текущей левой позиции
            else if (int.Parse(values[1]) == value_left)
            {
                // Движение по верхней горизонтали
                if (field.horiz_left < 4)
                {
                    value_left = int.Parse(values[0]);
                    side = 'L';

                    field.SetDomino(domino, side);
                }
                //Движение по вертикали
                else if ((field.horiz_left == 4 && field.vertic_left < 7) || field.horiz_left == 13)
                {
                    if (vertPosition_left == 0)     // позиция четная
                    {
                        value_left = int.Parse(values[0]);
                        side = 'L';

                        domino = FlipDomino(domino);        // Переворачиваем костяшку

                        field.SetDomino(domino, side);

                        vertPosition_left++;
                    }
                    else     // позиция нечетная
                    {
                        value_left = int.Parse(values[0]);
                        side = 'L';

                        field.SetDomino(domino, side);

                        vertPosition_left--;
                    }
                }
                // Движение по нижней горизонтали
                else if (field.vertic_left == 7 && field.horiz_left < 13)
                {
                    value_left = int.Parse(values[0]);
                    side = 'L';

                    domino = FlipDomino(domino);        // Переворачиваем костяшку

                    field.SetDomino(domino, side);
                }
            }
            // Если правый элемент кости равен текущей правой позиции
            else if (int.Parse(values[1]) == value_right)
            {
                // Движение по верхней горизонтали
                if (field.horiz_right < 5)
                {
                    value_right = int.Parse(values[0]);
                    side = 'R';

                    domino = FlipDomino(domino);        // Переворачиваем костяшку

                    field.SetDomino(domino, side);
                }
                // Движение по вертикали
                else if ((field.horiz_right == 5 && field.vertic_right < 7) || field.horiz_right == -4)
                {
                    if (vertPosition_right == 0)     // позиция четная
                    {
                        value_right = int.Parse(values[0]);
                        side = 'R';

                        field.SetDomino(domino, side);

                        vertPosition_right++;
                    }
                    else     // позиция нечетная
                    {
                        value_right = int.Parse(values[0]);
                        side = 'R';

                        domino = FlipDomino(domino);        // Переворачиваем костяшку

                        field.SetDomino(domino, side);

                        vertPosition_right--;
                    }
                }
                // Движение по нижней горизонтали
                else if (field.vertic_right == 7 && field.horiz_right > -4)
                {
                    value_right = int.Parse(values[0]);
                    side = 'R';

                    field.SetDomino(domino, side);
                }
            }
        }


        public bool CheckAvailability(List<string> hand)
        {
            bool isAvailable = false;

            if (hand.Count > 0)
            {
                for (int i = hand.Count - 1; i >= 0 && isAvailable == false; i--)
                {
                    string[] values = hand[i].Split(new char[] { '<', ':', '>' }, StringSplitOptions.RemoveEmptyEntries);
                    if (int.Parse(values[0]) == value_left || int.Parse(values[0]) == value_right || int.Parse(values[1]) == value_left || int.Parse(values[1]) == value_right)
                    {
                        isAvailable = true;
                    }
                }
            }
            
            return isAvailable;
        }

        bool CheckApprop(string domino)
        {
            bool isAppropriate = false;

            string[] values = domino.Split(new char[] { '<', ':', '>' }, StringSplitOptions.RemoveEmptyEntries);
            if (int.Parse(values[0]) == value_left || int.Parse(values[0]) == value_right || int.Parse(values[1]) == value_left || int.Parse(values[1]) == value_right)
            {
                isAppropriate = true;
            }

            return isAppropriate;
        }

    }
}
