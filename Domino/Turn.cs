using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino
{
    static class Turn
    {
        static byte turn = 0;
        static public byte WhoIsFirst(Players player1, Players player2)
        {
            if (player1.isDouble == true && player2.isDouble == true)
            {
                if (player1.maxValue > player2.maxValue)
                {
                    turn = 1;
                }
                else
                {
                    turn = 2;
                }
            }
            else if (player1.isDouble == true || player2.isDouble == true)
            {
                if (player1.isDouble == true)
                {
                    turn = 1;
                }
                else
                {
                    turn = 2;
                }
            }
            else
            {
                if (player1.maxValue > player2.maxValue)
                {
                    turn = 1;
                }
                else if (player1.maxValue < player2.maxValue)
                {
                    turn = 2;
                }
                else if (player1.maxValue == player2.maxValue)
                {
                    while (player1.maxValue == player2.maxValue)
                    {
                        player1.maxValue = Players.FindMax(player1);
                        player2.maxValue = Players.FindMax(player2);
                    }

                    if (player1.maxValue > player2.maxValue)
                    {
                        turn = 1;
                    }
                    else
                    {
                        turn = 2;
                    }
                }
            }
            return turn;
        }
    }
}
