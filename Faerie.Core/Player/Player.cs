using CmlLib.Core.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faerie.Core.Player
{
    internal class Player(MSession session)
    {
        private string? accessToken { get; set; } = session.AccessToken;
        private string? username { get; set; } = session.Username;

        private string? uuid = session.UUID;

        public string? GetAccessToken() => accessToken;
        public string? GetUsername() => username;
        public string? GetUUID() => uuid;
        public string GetSkinURL()
        {
            return $"https://crafatar.com/skins/uuid/{uuid}";
        }

        public string GetSkinFaceURL()
        {
            return $"https://crafatar.com/avatars/{uuid}";
        }

        public string GetSkinFaceANSI()
        {



            return String.Empty;
        }
    }
}
