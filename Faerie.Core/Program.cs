using CmlLib.Core.Auth.Microsoft;
using Faerie.Core.Data;
using Faerie.Core.DataStore;
using Faerie.Core.Game;
using Faerie.Core.Game.Modloaders;
using Faerie.Core.Java;
using Faerie.Core.Logger;
using Faerie.Core.Player;

new FaerieData()
    .VerifyStructure();

await new FaerieJavaFactory()
    .AddRuntime(8)
    .AddRuntime(11)
    .AddRuntime(16)
    .AddRuntime(17)
    .Build();

FaerieDirectoryWatcher.Start(new FaerieDirectory(FaerieData.PATH, "instances"));

JELoginHandler loginHandler = new JELoginHandlerBuilder()
    .WithLogger(FaerieLogger.logger)
    .Build();

if (loginHandler is null)
{
    throw new Exception("Couldn't initialize login handler");
}

// temp code
FaerieAuth auth = new(loginHandler, "499c8d36-be2a-4231-9ebd-ef291b7bb64c");

var msal = await auth.Prepare();
await auth.Signin(msal.authenticator);
var player = auth.GetPlayer();

if (player is null)
{
    throw new Exception("Couldn't fetch player data!");
}

Console.WriteLine(player.GetUsername());

var modloader = new Vanilla();
modloader.SetMinecraftVersion("1.20.4");

await new FaerieGameFactory()
    .SetModloader(modloader)
    .Play();

