using Faerie.Core.DataStore;
using Faerie.Core.Player;
using Microsoft.Extensions.Logging;

new FaerieData()
    .VerifyStructure();

// temp code
FaerieAuth auth = new(FaerieAuth.Method.DEVICECODE, "");

await auth.Signin();
Player? player = auth.GetPlayer();

if (player is null)
{
    logger.Log(LogLevel.Error, "Couldn't fetch player data!");
    return;
}


