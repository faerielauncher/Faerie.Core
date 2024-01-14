using Faerie.Core.Logger;
using Faerie.Core.Player;
using Microsoft.Extensions.Logging;
using static Faerie.Core.Logger.FaerieLogger;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Auth auth = new (Auth.Method.DEVICECODE, "");

        await auth.Signin();
        Player? player = auth.GetPlayer();

        if (player is null)
        {
            logger.Log(LogLevel.Error, "Couldn't authenticate");
            return;
        }

        Console.WriteLine(player.GetAccessToken());
    }
}