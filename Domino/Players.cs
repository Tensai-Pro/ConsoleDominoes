using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino
{
    class Players
    {
        public int maxValue = 0;                // Максимальное значение костяшки в руке
        public bool isDouble = false;                 // Наличие дубля (кроме пустой кости)
        public List<string> Hand = new List<string>();  // Рука игрока 

        public Players(Stack<string> pile)
        {
            for (int i = 0; i < 7; i++)
            {
                Hand.Add(pile.Pop());

                string[] values = Hand[i].Split(new char[] { '<', ':', '>' }, StringSplitOptions.RemoveEmptyEntries);    // Массив со значениями хвоста и головы костяшки   
                if (values[0] == values[1] && values[0] != "0")
                {
                    isDouble = true;
                }
            }
            FindMax();
        }

        void FindMax()      // Находит максимальное значение среди костей 
        {
            // Работа с дублями
            if (isDouble == true)                                            
            {
                for (int i = 0; i < Hand.Count; i++)
                {
                    string[] values = Hand[i].Split(new char[] { '<', ':', '>' }, StringSplitOptions.RemoveEmptyEntries);

                    if (values[0] == values[1])
                    {
                        if (byte.Parse(values[0]) + byte.Parse(values[1]) > maxValue)
                        {
                            maxValue = byte.Parse(values[0]) + byte.Parse(values[1]);
                        }
                    }
                }
            }
            // Работа при отсутствии дублей
            else
            {
                for (int i = 0; i < Hand.Count; i++)
                {
                    string[] values = Hand[i].Split(new char[] { '<', ':', '>' }, StringSplitOptions.RemoveEmptyEntries);

                    if (byte.Parse(values[0]) + byte.Parse(values[1]) > maxValue)
                    {
                        maxValue = byte.Parse(values[0]) + byte.Parse(values[1]);
                    }
                }
            }
        }

        public static int FindMax(Players player)       // Статическая перегрузка метода, используемая при равных макс. значениях у игроков                             
        {
            int _maxValue = 0;

            for (int i = 0; i < player.Hand.Count; i++)
            {
                string[] values = player.Hand[i].Split(new char[] { '<', ':', '>' }, StringSplitOptions.RemoveEmptyEntries);

                if (byte.Parse(values[0]) + byte.Parse(values[1]) != player.maxValue)
                {
                    if (byte.Parse(values[0]) + byte.Parse(values[1]) > _maxValue)
                    {
                        _maxValue = byte.Parse(values[0]) + byte.Parse(values[1]);
                    }
                }
            }

            return _maxValue;
        }
    }
}
