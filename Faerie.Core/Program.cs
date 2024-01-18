using Faerie.Core.Data;
using Faerie.Core.DataStore;
using Faerie.Core.Game;
using Faerie.Core.Game.Modloaders;
using Faerie.Core.Java;
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

// temp code
FaerieAuth auth = new(FaerieAuth.Method.DEVICECODE, "499c8d36-be2a-4231-9ebd-ef291b7bb64c");

await auth.Signin();
Player? player = auth.GetPlayer();

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

