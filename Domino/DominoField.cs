using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino
{
    class DominoField
    {
        public string[,] Field;
        public DominoField()
        {
            Field = new string[8, 10]       // Кости распологаются по периметру 
            {
                { "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     " },
                { "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     " },
                { "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     " },
                { "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     " },
                { "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     " },
                { "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     " },
                { "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     " },
                { "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     ", "     " }
            };
        }

        public void DisplayField()   // Отобразить игровое поле
        {
            Console.Clear();
            string underscore = new string('_', 120);
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int i = 0; i < 8; i++)            
            {
                Console.WriteLine();
                Console.Write("\t\t");
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(Field[i, j]);
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(underscore);
            Console.ForegroundColor = ConsoleColor.Yellow;
        }


        int i = 0, j = 4;    // Позиция по умолчанию для первой костяшки
        public void SetDomino(string domino)         
        {
            Field[i, j] = domino;
        }


        public int horiz_left = 0;      // Смещение в матрице против часовой стрелки горизонтально
        public int vertic_left = 0;     // против часовой стрелки вертикально
        public int horiz_right = 0;     // по часовой стрелке горизонтально
        public int vertic_right = 0;    // по часовой стрелке вертикально
        public void SetDomino(string domino, char side)      // Все значения и условия вычислены математически отдельно
        {
            // Если кость ставится влево и далее против часовой стрелки
            if (side == 'L')
            {
                if (horiz_left < 4)
                {
                    Field[0, j - horiz_left - 1] = domino;
                    horiz_left++;
                }
                else if (horiz_left == 4 && vertic_left < 7)
                {
                    Field[vertic_left + 1, 0] = domino;
                    vertic_left++;
                }
                else if (vertic_left == 7 && horiz_left < 13)
                {
                    Field[7, horiz_left - j + 1] = domino;
                    horiz_left++;
                }
                else if (horiz_left == 13)
                {
                    Field[vertic_left - 1, 9] = domino;
                    vertic_left--;
                }
            }
            // Если кость ставится вправо и далее по часовой стрелке
            else if (side == 'R')
            {
                if (horiz_right < 5)
                {
                    Field[0, j + horiz_right + 1] = domino;
                    horiz_right++;
                }
                else if (horiz_right == 5 && vertic_right < 7)
                {
                    Field[vertic_right + 1, 9] = domino;
                    vertic_right++;
                }
                else if (vertic_right == 7 && horiz_right > -4)
                {
                    Field[7, horiz_right + j - 1] = domino;
                    horiz_right--;
                }
                else if (horiz_right == -4)
                {
                    Field[vertic_right - 1, 0] = domino;
                    vertic_right--;
                }
            }
        }
    }
}
