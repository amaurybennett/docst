using typx.Models;

namespace typx
{
    public class TypstExporter : BaseExporter
    {
        public override string GetFileExtension() => "typ";

        public override string GetTitre1(string content) => $"= {content}";

        public override string GetItalic(string content) => $"_{content}_";

        public override string GetElipse(string content) => $"== {content}";
    }
}
