using Faerie.Core.Http;
using Faerie.Core.Templates;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Xml.Linq;

namespace Faerie.Core.Game.Modloaders
{
    /// <summary>
    /// https://piston-meta.mojang.com/mc/game/version_manifest.json 
    /// </summary>
    internal partial class Vanilla : ModLoader
    {
        JsonSerializerOptions serializeOptions = new JsonSerializerOptions
        {
            Converters =
                {
                    new ArgumentRuleConverter(),
                }
        };

        // i hate this code
        public override FaerieArgumentFactory Arguments()
        {
            FaerieArgumentFactory factory = new FaerieArgumentFactory();
            var version = JsonDeserialize<VersionTemplate>(Path.Combine(CachePath.GetPath(), $"{MinecraftVersion}.json"));

            if (version is null)
            {
                throw new Exception($"Couldn't find {MinecraftVersion}.json");
            }

            if(version.MinecraftArguments is not null)
            {
                version.MinecraftArguments = $"-Djava.library.path= -cp {version.MinecraftArguments}";
                foreach (var item in version.MinecraftArguments.Split(" "))
                {
                    string value = item;
                    var faerieArgument = new FaerieArgument();

                    if (value.Contains("${"))
                    {
                        int index = value.IndexOf("$");
                        value = value.Substring(0, index);
                    }
                    if (value.Contains("="))
                    {
                        int index = value.IndexOf("=");
                        value = value.Substring(0, index);
                        faerieArgument.SetSplitter("=");
                    }
                    if (!string.IsNullOrEmpty(value))
                    {
                        faerieArgument.SetKey(value);
                        factory.AddArgument(faerieArgument);
                    }
                }

                return BuildArguments(factory);
            }

            if(version.Arguments is null)
            {
                throw new Exception("Couldn't fetch arguments.");
            }

            List<object> merged = new List<object>();

            // ignore rules because they r optional in game
            merged.AddRange(version.Arguments.Jvm);

            foreach (var item in version.Arguments.Game)
            {
                var element = JsonSerializer.Deserialize<object>((JsonElement)item, serializeOptions);
                if (element is string)
                {
                    merged.Add(item);
                }
            }


            for (int i = 0; i < merged.Count; i++)
            {
                var element = JsonSerializer.Deserialize<object>((JsonElement)merged[i], serializeOptions);
                if (element is string)
                {
                    var value = (string)element;
                    var faerieArgument = new FaerieArgument();

                    if (value.Contains("${"))
                    {
                        int index = value.IndexOf("$");
                        value = value.Substring(0, index);
                    }
                    if (value.Contains("="))
                    {
                        int index = value.IndexOf("=");
                        value = value.Substring(0, index);
                        faerieArgument.SetSplitter("=");
                    }

                    if (!string.IsNullOrEmpty(value))
                    {
                        faerieArgument.SetKey(value);
                        factory.AddArgument(faerieArgument);
                    }

                }

                if (element is RulesTemplate)
                {
                    var value = (RulesTemplate)element;
                    var rules = value.Rules;

                    if (rules is not null)
                    {
                        foreach (var rule in rules)
                        {
                            var subvalue = value.Value;
                            if (subvalue is null)
                            {
                                continue;
                            }

                            var values = JsonSerializer.Deserialize<object>((JsonElement)subvalue, serializeOptions);
                            bool addRule = false;


                            if (rule.Os is null)
                            {
                                Console.WriteLine("is null");
                                addRule = true;
                            }
                            else
                            {
                                if (rule.Os.ContainsKey("name"))
                                {
                                    if (rule.Os["name"] == GetPlatform().Replace("mac", "osx"))
                                    {
                                        addRule = true;
                                    }
                                }

                                if (rule.Os.ContainsKey("arch"))
                                {
                                    if (rule.Os["arch"] == GetArchitecture())
                                    {
                                        addRule = true;
                                    }
                                }
                            }

                            if (addRule)
                            {
                                if (values is string)
                                {
                                    var val = (string)values;
                                    var faerieArgument = new FaerieArgument();
                                    if (val.Contains("${"))
                                    {
                                        int index = val.IndexOf("$");
                                        val = val.Substring(0, index);
                                    }
                                    if (val.Contains("="))
                                    {
                                        int index = val.IndexOf("=");
                                        val = val.Substring(0, index);
                                        faerieArgument.SetSplitter("=");
                                    }

                                    if (!string.IsNullOrEmpty(val))
                                    {
                                        faerieArgument.SetKey(val);
                                        factory.AddArgument(faerieArgument);
                                    }

                                }

                                if (values is List<string>)
                                {
                                    foreach (string val in (List<string>)values)
                                    {
                                        string _value = val;
                                        var faerieArgument = new FaerieArgument();

                                        if (_value.Contains("${"))
                                        {
                                            int index = _value.IndexOf("$");
                                            _value = _value.Substring(0, index);
                                        }
                                        if (_value.Contains("="))
                                        {
                                            int index = _value.IndexOf("=");
                                            _value = _value.Substring(0, index);
                                            faerieArgument.SetSplitter("=");
                                        }

                                        if (!string.IsNullOrEmpty(_value))
                                        {
                                            faerieArgument.SetKey(_value);
                                            factory.AddArgument(faerieArgument);
                                        }

                                    }
                                }

                            }
                        }
                    }

                }

            }

            return BuildArguments(factory);
        }

        public async override Task<bool> Download()
        {
            string manifestFile = Path.Combine(CachePath.GetPath(), "version_manifest.json");

            if (!File.Exists(manifestFile))
            {
                await new FaerieHttpFactory("https://piston-meta.mojang.com/mc/game/version_manifest.json")
                    .CreateRequestDownload(CachePath);
            }
            else
            {
                logger.LogInformation("Manifest exists! Skipping download.");
            }

            var manifest = JsonDeserialize<VersionManifestTemplate>(manifestFile);

            var url = manifest?.Versions?
                .First(s => s.Id == MinecraftVersion)
                .Url;

            if (url is null)
            {
                return false;
            }

            await new FaerieHttpFactory(url)
                .CreateRequestDownload(CachePath);

            var version = JsonDeserialize<VersionTemplate>(Path.Combine(CachePath.GetPath(), $"{MinecraftVersion}.json"));

            if (await DownloadLibraries(version))
            {
                logger.LogInformation("Libraries finished!");
            } 
            else
            {
                logger.LogWarning("Libraries finished with errors.");
            }

            if (await DownloadAssets(version))
            {
                logger.LogInformation("Assets finished!");
            }
            else
            {
                logger.LogWarning("Assets finished with errors.");
            }

            if (await DownloadClient(version))
            {
                logger.LogInformation("Clients finished!");
            }
            else
            {
                logger.LogWarning($"Clients finished with errors.");
            }

            return true;

        }
    }
}
