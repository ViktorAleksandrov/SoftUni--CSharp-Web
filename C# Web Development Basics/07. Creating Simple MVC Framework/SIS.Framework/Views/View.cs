using System.IO;
using SIS.Framework.ActionResults.Contracts;

namespace SIS.Framework.Views
{
    public class View : IRenderable
    {
        private readonly string fullyQualifiedTemplateName;

        public View(string fullyQualifiedTemplateName)
        {
            this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
        }

        public string Render()
        {
            string fullHtml = this.ReadFile(this.fullyQualifiedTemplateName);

            return fullHtml;
        }

        private string ReadFile(string fullyQualifiedTemplateName)
        {
            if (!File.Exists(fullyQualifiedTemplateName))
            {
                throw new FileNotFoundException($"View does not exist at {fullyQualifiedTemplateName}");
            }

            return File.ReadAllText(fullyQualifiedTemplateName);
        }
    }
}
