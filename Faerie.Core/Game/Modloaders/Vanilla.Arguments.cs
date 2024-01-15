using Faerie.Core.Data;
using Faerie.Core.DataStore;
using Faerie.Core.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faerie.Core.Game.Modloaders
{
    internal partial class Vanilla
    {
        FaerieArgumentFactory BuildArguments(FaerieArgumentFactory factory)
        {
            var version = JsonDeserialize<VersionTemplate>(Path.Combine(CachePath.GetPath(), $"{MinecraftVersion}.json"));
            var assetsDir = new FaerieDirectory(FaerieData.PATH, "assets");

            if (version is null)
            {
                throw new Exception($"Couldn't find {MinecraftVersion}.json");
            }

            factory.GetKeyAndSetValue("--username", Username);
            factory.GetKeyAndSetValue("--version", MinecraftVersion);
            factory.GetKeyAndSetValue("--gameDir", "");
            factory.GetKeyAndSetValue("--assetsDir", assetsDir.GetPath());
            factory.GetKeyAndSetValue("--assetIndex", version.AssetIndex.Id);
            factory.GetKeyAndSetValue("--uuid", Uuid);
            factory.GetKeyAndSetValue("--accessToken", AccessToken);
            factory.GetKeyAndSetValue("--xuid", Xuid);
            factory.GetKeyAndSetValue("--userType", "msa");
            factory.GetKeyAndSetValue("--versionType", "Faerie");

            return factory;
        }
    }
}
