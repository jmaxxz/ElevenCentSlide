using System;

namespace ElevenCentSlide
{
    public interface IPlayer
    {
        /// <summary>
        /// When called player will be given unlimited time to reach a solution
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        Solution Solve(Game game);

        /// <summary>
        /// When called player will be given no longer than <see cref="limit"/>
        /// to solve the problem. If player is still solving when limit is reached
        /// call will be terminated, and it will be assumed player did not know
        /// if the problem was solvable, and did not have a solution.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Solution Solve(Game game, TimeSpan limit);
    }
}
