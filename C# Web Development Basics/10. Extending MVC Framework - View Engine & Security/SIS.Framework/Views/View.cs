using SIS.Framework.ActionResults.Contracts;

namespace SIS.Framework.Views
{
    public class View : IRenderable
    {
        private readonly string fullHtmlContent;

        public View(string fullHtmlContent)
        {
            this.fullHtmlContent = fullHtmlContent;
        }

        public string Render()
        {
            return this.fullHtmlContent;
        }
    }
}
