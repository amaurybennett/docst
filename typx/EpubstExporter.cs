using System.Collections.Immutable;
using System.IO.Enumeration;
using typx.Models;

namespace typx
{
    public class EpubstExporter : BaseExporter
    {
        public override string GetFileExtension() => "md";

        public override string GetTitre1(string content) => $"# {content}";
        
        public override string GetItalic(string content) => $"_{content}_";

        public override string GetElipse(string content) => $"{{.elipse}}\n{content}";
    }
}
