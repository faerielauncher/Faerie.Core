namespace Faerie.Core.Java
{
    internal class JavaFactory
    {
        private List<string> jre = new List<string>();
        private string? path;
        public JavaFactory AddRuntime(string jre)
        {
            if (string.IsNullOrWhiteSpace(jre))
            {
                throw new ArgumentException($"'{nameof(jre)}' cannot be null or whitespace.", nameof(jre));
            }
            this.jre.Add(jre);

            return this;
        }
        public JavaFactory SetPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            }
            this.path = path;

            return this;
        }

        public bool Build()
        {
            return false;
        }
    }
}
