using CmlLib.Core.Auth.Microsoft;
using XboxAuthNet.Game.Msal;

var app = await MsalClientHelper.BuildApplicationWithCache("yeeee");
var loginHandler = JELoginHandlerBuilder.BuildDefault();

var authenticator = loginHandler.CreateAuthenticatorWithNewAccount(default);
authenticator.AddMsalOAuth(app, msal => msal.Interactive());
authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
authenticator.AddJEAuthenticator();
var session = await authenticator.ExecuteForLauncherAsync();