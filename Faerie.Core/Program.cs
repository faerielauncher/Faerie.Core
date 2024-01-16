using Faerie.Core.DataStore;
using Faerie.Core.Game;
using Faerie.Core.Game.Modloaders;
using Faerie.Core.Java;
using Faerie.Core.Player;
using Microsoft.Extensions.Logging;

new FaerieData()
    .VerifyStructure();

await new FaerieJavaFactory()
    .AddRuntime(8)
    .AddRuntime(11)
    .AddRuntime(16)
    .AddRuntime(17)
    .Build();

// temp code
FaerieAuth auth = new(FaerieAuth.Method.DEVICECODE, "");

await auth.Signin();
Player? player = auth.GetPlayer();

if (player is null)
{
    throw new Exception("Couldn't fetch player data!");
}

Console.WriteLine(player.GetUsername());

var modloader = new Vanilla();
modloader.SetMinecraftVersion("1.12.2");

await new FaerieGameFactory()
    .SetModloader(modloader)
    .Play();