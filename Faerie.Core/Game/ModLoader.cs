using Faerie.Core.Data;
using Faerie.Core.DataStore;
using Faerie.Core.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faerie.Core.Game
{
    public abstract class ModLoader
    {
        protected string? MinecraftVersion;
        protected string? LoaderVersion;
        protected string? JavaPath;
        protected readonly FaerieDirectory CachePath = new FaerieDirectory(FaerieData.PATH, "cache");

        public void SetMinecraftVersion(string version)
        {
            MinecraftVersion = version;
        }
        public void SetLoaderVersion(string? version)
        {
            LoaderVersion = version;
        }
        public Task ConfigureJava(string path = "")
        {
            if (!string.IsNullOrEmpty(path))
            {
                JavaPath = path;
                return Task.CompletedTask;
            }

            var version = JsonDeserialize<VersionTemplate>(Path.Combine(CachePath.GetPath(), $"{MinecraftVersion}.json"));
            if(version is null)
            {
                throw new Exception($"Couldn't load {MinecraftVersion}.json, please try re-downloading the instance.");
            }

            int majorVersion = version.JavaVersion.majorVersion;

            JavaPath = new FaerieDirectory(Path.Combine(FaerieData.PATH, "runtime", majorVersion.ToString()), "bin").GetPath();
            return Task.CompletedTask;

        }

        public string? GetJavaExecutable()
        {
            return Path.Combine(JavaPath!, "java.exe");
        }
        public abstract Task<bool> Download();
        public abstract FaerieArgumentFactory Arguments();
    }
}
