using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ElevenCentSlide
{
    public class Game
    {
        public static Game CommonGame { get; }
        public ImmutableArray<Coin> TargetPattern { get; }
        public ImmutableArray<Coin> Board { get; }
        public bool Solved { get; }

        static Game()
        {
            CommonGame = new Game(new[] {Coin.Penny, Coin.Penny, Coin.Penny, Coin.Dime, Coin.Dime},
                Enumerable.Repeat(Coin.None, 10)
                .Concat(new[] { Coin.Penny, Coin.Dime, Coin.Penny, Coin.Dime, Coin.Penny })
                .Concat(Enumerable.Repeat(Coin.None, 10))
            );
        }

        public Game(IEnumerable<Coin> targetPattern, IEnumerable<Coin> startingBoard)
        {
            if (targetPattern.Count() < 2)
                throw new ArgumentException("Target pattern too short", nameof(targetPattern));

            TargetPattern = targetPattern.ToImmutableArray();
            Board = startingBoard.ToImmutableArray();
            Solved = GetPositionOfSolution() >= 0;
        }

        public bool IsValidMove(int startingPos, int endingPos)
        {
            if (startingPos > Board.Length - 2 || startingPos < 0)
                return false; // Starting position is off the board
            if (endingPos > Board.Length - 2 || endingPos < 0)
                return false; // Ending position is off the board
            if((int)Board[startingPos] + (int)Board[startingPos+1] != 11)
                return false; // Must move 11 cents each time
            if (Board[endingPos] != Coin.None || Board[endingPos + 1] != Coin.None)
                return false; // End position must not be filled

            return true;
        }

        public Game Move(int startingPos, int endingPos)
        {
            if (!IsValidMove(startingPos, endingPos))
                throw new ArgumentOutOfRangeException("Invalid move");

            var minPos = Math.Min(startingPos, endingPos);
            var maxPos = Math.Max(startingPos, endingPos);

            var newBoard = Board.Take(minPos)
                .Concat(Board.Skip(maxPos).Take(2))
                .Concat(Board.Skip(minPos + 2).Take(maxPos-(minPos+2)))
                .Concat(Board.Skip(minPos).Take(2))
                .Concat(Board.Skip(maxPos+2));

            return new Game(TargetPattern, newBoard);
        }

        private int GetPositionOfSolution()
        {
            // [kmp search](https://en.wikipedia.org/wiki/Knuth%E2%80%93Morris%E2%80%93Pratt_algorithm)

            int[] prefixTable = new int[TargetPattern.Length];
            prefixTable[0] = -1;
            prefixTable[1] = 0;

            var pos = 2;
            var cnd = 0;

            while(pos < TargetPattern.Length)
            {
                if (TargetPattern[pos - 1] == TargetPattern[cnd])
                {
                    prefixTable[pos++] = ++cnd;
                }
                else if(cnd > 0)
                {
                    cnd = prefixTable[cnd];
                }
                else
                {
                    prefixTable[pos] = 0;
                    pos++;
                }
            }

            var i = 0;
            var m = 0;
            while(m+i < Board.Length)
            {
                if(TargetPattern[i] == Board[m + i])
                {
                    if(i++ == TargetPattern.Length-1)
                        return m;
                }
                else
                {
                    if(prefixTable[i] > -1)
                    {
                        m = m + i - prefixTable[i];
                        i = prefixTable[i];
                    }
                    else
                    {
                        m++;
                        i = 0;
                    }
                }
            }

            return -1;
        }
    }
}
