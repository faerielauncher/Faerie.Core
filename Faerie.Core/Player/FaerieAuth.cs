using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using Faerie.Core.Logger;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.Msal;

namespace Faerie.Core.Player
{

    internal class FaerieAuth(FaerieAuth.Method method, string appId, XboxGameAccount? account = null)
    {
        private Method AuthMethod { get; } = method;
        private string AppId { get; } = appId;
        private XboxGameAccount? Account { get; } = account;
        private MSession? session;
        
        private readonly JELoginHandler loginHandler = new JELoginHandlerBuilder()
            .WithLogger(FaerieLogger.logger)
            .Build();


        public enum Method
        {
            SILENT,
            SILENT_MSAL,
            INTERACTIVE,
            DEVICECODE,
            OFFLINE,
        }

        public async Task Signin()
        {
            NestedAuthenticator? authenticator;
            var app = await MsalClientHelper.BuildApplicationWithCache(AppId);

            switch (AuthMethod)
            {
                case Method.SILENT:
                    if(Account is not null)
                    {
                        session = await loginHandler.AuthenticateInteractively(Account);
                    }
                    break;
                case Method.SILENT_MSAL:
                    authenticator = loginHandler.CreateAuthenticatorWithDefaultAccount();
                    authenticator.AddMsalOAuth(app, msal => msal.Silent());
                    authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
                    authenticator.AddJEAuthenticator();
                    session = await authenticator.ExecuteForLauncherAsync();
                    break;

                case Method.INTERACTIVE:
                    authenticator = loginHandler.CreateAuthenticatorWithDefaultAccount(default);
                    authenticator.AddMsalOAuth(app, msal => msal.Interactive());
                    authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
                    authenticator.AddForceJEAuthenticator();
                    session = await authenticator.ExecuteForLauncherAsync();
                    break;

                case Method.DEVICECODE:
                    authenticator = loginHandler.CreateAuthenticatorWithDefaultAccount(default);
                    authenticator.AddMsalOAuth(app, msal => msal.DeviceCode(code =>
                    {
                        Console.WriteLine(code.Message);
                        return Task.CompletedTask;
                    }));
                    authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
                    authenticator.AddJEAuthenticator();
                    session = await authenticator.ExecuteForLauncherAsync();
                    break;

            }

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
