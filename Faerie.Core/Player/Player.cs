using CmlLib.Core.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faerie.Core.Player
{
    internal class Player
    {
        public static string? AccessToken { get; set; }
        public static string? Username { get; set; }
        public static string? Uuid { get; set; }
        public static string? Xuid { get; set; }

        public Player(MSession session)
        {
            AccessToken = session.AccessToken;
            Username = session.Username;
            Uuid = session.UUID;
            Xuid = session.Xuid;
        }

        public string? GetAccessToken() => AccessToken;
        public string? GetUsername() => Username;
        public string? GetUUID() => Uuid;
        public string GetSkinURL()
        {
            return $"https://crafatar.com/skins/uuid/{Uuid}";
        }

        public string GetSkinFaceURL()
        {
            return $"https://crafatar.com/avatars/{Uuid}";
        }

        public string GetSkinFaceANSI()
        {



            return String.Empty;
        }
    }
}
