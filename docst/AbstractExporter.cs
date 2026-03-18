using System.Linq.Expressions;
using docst.Models;

namespace docst
{
    public abstract class AbstractExporter
    {
        public abstract void Export(Document document, string baseFile);

        public abstract string GetFileExtension();

        public abstract string GetTitre1(string content);

        public abstract string GetItalic(string content);

        public abstract string GetElipse(string content);
    }

    public abstract class BaseExporter : AbstractExporter
    {
        public sealed override void Export(Document document, string baseFile)
        {
            var filename = $"{baseFile}.{GetFileExtension()}";
            FileStream handle = File.OpenWrite(filename);

            var content = "";

            foreach (var paragraph in document.Paragraphs)
            {
                if (paragraph.StyleId == "Titre1")
                {
                    handle.Write(System.Text.Encoding.UTF8.GetBytes(content));
                    handle.Flush();
                    content = "";
                    var title = string.Join("", paragraph.Runs.Select(r => r.Text));
                    content = GetTitre1(title);
                }
                else if (paragraph.StyleId == "Normal")
                {
                    foreach (var run in paragraph.Runs)
                    {
                        if (run.Italic) content += GetItalic(run.Text);
                        else content += run.Text;
                    }
                }
                else if (paragraph.StyleId == "Elipse")
                {
                    content += GetElipse(string.Join("", paragraph.Runs.Select(r => r.Text)));
                }
                else if (paragraph.StyleId == "Titre")
                {
                    // Ignoring this style for now
                }
                else throw new Exception($"Unknown style: {paragraph.StyleId}");
                content += "\n\n";
            }

            handle.Write(System.Text.Encoding.UTF8.GetBytes(content));
            handle.Flush();
            handle.Close();
        }
    }
}