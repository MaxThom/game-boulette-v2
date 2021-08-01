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

        public static (Player, bool?) FindCorrespondingPlayer(Guid playerId, GameRoom game)
        {
            bool? isTeamOne = null;
            var player = game.TeamOne.Players.Where(x => x.Id == playerId).FirstOrDefault();
            if (player == null)
            {
                player = game.TeamTwo.Players.Where(x => x.Id == playerId).FirstOrDefault();
                if (player != null)
                    isTeamOne = false;
            }
            else
                isTeamOne = true;
            
            return (player, isTeamOne);
        }

        public static (Player, GameRoom, bool?) FindCorrespondingPlayerAndGame(string connectionId, Dictionary<string, GameRoom> games)
        {
            Player player = null;
            GameRoom g = null;
            bool? isTeamOne = null;
            foreach (var game in games.Select(x => x.Value))
            {
                player = game.TeamOne.Players.Where(x => x.ConnectionId == connectionId).FirstOrDefault();
                if (player == null)
                {
                    player = game.TeamTwo.Players.Where(x => x.ConnectionId == connectionId).FirstOrDefault();
                    if (player != null)
                        isTeamOne = false;
                }
                else
                    isTeamOne = true;                    

                if (player != null)
                    return (player, game, isTeamOne);
            }

            return (player, g, isTeamOne);
        }
    }
}
