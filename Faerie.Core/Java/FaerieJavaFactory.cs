using Faerie.Core.Data;
using Faerie.Core.DataStore;
using Faerie.Core.Http;
using Faerie.Core.Templates;
using Microsoft.Extensions.Logging;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace Faerie.Core.Java
{
    internal class FaerieJavaFactory
    {
        private readonly List<int> jre = new List<int>();
        private FaerieDirectory? dir = new(FaerieData.PATH, "runtime");
        public FaerieJavaFactory AddRuntime(int jre)
        {
            this.jre.Add(jre);

            return this;
        }
        public FaerieJavaFactory SetDirectory(FaerieDirectory dir)
        {
            this.dir = dir;
            return this;
        }

        private bool Exists(string version)
        {
            return new FaerieDirectory(Path.Combine(FaerieData.PATH, "runtime"), version).Exists();
        }

        public async Task Build()
        {
            // https://api.adoptium.net/v3/binary/latest/11/ga/windows/x64/jre/hotspot/normal/eclipse?project=jdk
            // https://api.adoptium.net/v3/binary/latest/<version>/ga/<os>/<architecture>/jre/hotspot/normal/eclipse?project=jdk

            AvailableReleasesTemplate? releases = await new FaerieHttpFactory("https://api.adoptium.net/v3/info/available_releases")
                .CreateRequestAsJson<AvailableReleasesTemplate>();

            if (releases is null)
            {
                logger.LogWarning("Couldn't fetch Java Releases, skipping download.");
                return;
            }

            foreach (var item in jre)
            {
                FaerieDirectory dir = new FaerieDirectory(FaerieData.PATH, "runtime");
                string release = "ga";

                if (!releases.Available_lts_releases.Contains(item))
                {
                    release = "ea";
                }

                using (FaerieDirectory jreDir = new FaerieDirectory(dir.GetPath(), item.ToString()))
                {
                    if (jreDir.IsEmpty())
                    {
                        jreDir.Delete();
                    }

                    if (!Exists(item.ToString()))
                    {

                        if (!jreDir.Exists())
                        {
                            var result = await new FaerieHttpFactory($"https://api.adoptium.net/v3/binary/latest/{item}/{release}/{GetPlatform()}/{GetArchitecture()}/jre/hotspot/normal/eclipse?project=jdk")
                            .CreateRequestDownload(dir);

                            if (result.Item1)
                            {
                                string folderName;
                                using (ZipArchive zip = ZipFile.OpenRead(result.Item2))
                                {
                                    folderName = zip.Entries.First().FullName;
                                    zip.ExtractToDirectory(dir.GetPath());
                                }

                                Directory.Move(Path.Combine(dir.GetPath(), folderName), Path.Combine(dir.GetPath(), item.ToString()));

                                File.Delete(result.Item2);
                            }
                        }
                    }
                }
            }
        }
    }
}
