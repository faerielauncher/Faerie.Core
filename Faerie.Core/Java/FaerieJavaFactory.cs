using Faerie.Core.Data;
using Faerie.Core.DataStore;

namespace Faerie.Core.Java
{
    internal class FaerieJavaFactory
    {
        private readonly List<string> jre = new List<string>();
        private FaerieDirectory? dir = new(FaerieData.PATH, "runtime");
        public FaerieJavaFactory AddRuntime(string jre)
        {
            if (string.IsNullOrWhiteSpace(jre))
            {
                throw new ArgumentException($"'{nameof(jre)}' cannot be null or whitespace.", nameof(jre));
            }
            this.jre.Add(jre);

            return this;
        }
        public FaerieJavaFactory SetDirectory(FaerieDirectory dir)
        {
            this.dir = dir;
            return this;
        }

        public bool Build()
        {
            return false;
        }
    }
}
