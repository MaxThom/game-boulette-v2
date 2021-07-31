using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoulette.Shared.Utilities
{
    public static class GameUtility
    {
        private static string availableCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJQLMNOPQRSTUVWXYZ0123456789!=@$%&*";
        private static Random random = new Random(); //new Guid().GetHashCode()
        private const int GameCodeSize = 8;

        public static string GenerateGameCode()
        {
            var code = new StringBuilder();
            int i = 0;
            while (i < GameCodeSize)
            {
                code = code.Append(availableCharacters[random.Next(availableCharacters.Length)]);
                i++;
            }
            
            return code.ToString();
        }

        public static bool IsGameCodeValid(string code)
        {
            if (code.Length != GameCodeSize)
                return false;

            for (int i = 0; i < code.Length; i++)
            {
                if (availableCharacters.IndexOf(code[i]) == -1)
                    return false;
            }

            return true;
        }

        public static Player FindCorrespondingPlayer(Guid playerId, GameRoom game)
        {
            var player = game.TeamOne.Players.Where(x => x.Id == playerId).FirstOrDefault();
            if (player == null)
                player = game.TeamTwo.Players.Where(x => x.Id == playerId).FirstOrDefault();
            
            return player;
        }
    }
}
