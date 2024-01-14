using Faerie.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faerie.Core.DataStore
{
    internal class FaerieData
    {
        public static string FOLDER_NAME = "Faerie_Alpha";
        public static string PATH = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FOLDER_NAME);

        private readonly List<FaerieDirectory> directories = new()
        {
            {
                new FaerieDirectory(PATH, "runtime")
            },
            {
                new FaerieDirectory(PATH, "instances")
            },
            {
                new FaerieDirectory(PATH, "libraries")
            },
            {
                new FaerieDirectory(PATH, "assets")
            },
            {
                new FaerieDirectory(PATH, "logs")
            },
        };
        public FaerieData VerifyStructure()
        {
            try
            {
                if (!Directory.Exists(PATH))
                {
                    Directory.CreateDirectory(PATH);
                }

                directories.ForEach(directory =>
                {
                    if (!directory.Exists())
                    {
                        directory.CreateDirectory();
                    }
                });

                return this;
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message, ex);
            }

        }

    }
}
