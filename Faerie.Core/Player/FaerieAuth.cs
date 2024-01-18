using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using Faerie.Core.Logger;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.Msal;

namespace Faerie.Core.Player
{

    public class FaerieAuth(JELoginHandler loginHandler, string appId, XboxGameAccount? account = null)
    {
        private string AppId { get; } = appId;
        private XboxGameAccount? Account { get; } = account;
        private MSession? session;
        
        public async Task<(NestedAuthenticator authenticator, string authCode, DateTimeOffset expiration)> Prepare()
        {
            var app = await MsalClientHelper.BuildApplicationWithCache(AppId);
            string deviceCode = "";
            DateTimeOffset expiration = default;
            var authenticator = loginHandler.CreateAuthenticatorWithDefaultAccount(default);
            authenticator.AddMsalOAuth(app, msal => msal.DeviceCode(code =>
            {
                Console.WriteLine(code.Message);
                deviceCode = code.DeviceCode;
                expiration = code.ExpiresOn;
                return Task.CompletedTask;
            }));
            authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
            authenticator.AddJEAuthenticator();
            return (authenticator, deviceCode, expiration);
        }

        public async Task<NestedAuthenticator> PrepareSilent()
        {
            var app = await MsalClientHelper.BuildApplicationWithCache(AppId);
            NestedAuthenticator authenticator = loginHandler.CreateAuthenticatorWithDefaultAccount();
            authenticator.AddMsalOAuth(app, msal => msal.Silent());
            authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
            authenticator.AddJEAuthenticator();
            return authenticator;
        }

        public async Task Signin(NestedAuthenticator authenticator)
        {
            session = await authenticator.ExecuteForLauncherAsync();

        }



        public async Task Signout()
        {
            if(Account is not null)
            {
                await loginHandler.Signout(Account);
            }
        }

        public Player? GetPlayer()
        {
            if(session is not null)
            {
                return new Player(session);
            }

            return null;
        }
    }
}
