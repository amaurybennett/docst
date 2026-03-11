using System.Collections.Immutable;
using typx.Models;

namespace typx
{
    public class TypstExporter
    {
        public void Export(Document document)
        {
            // Placeholder for export logic
            Console.WriteLine("Exporting document to Typst format...");
            // Here you would implement the logic to convert the Document object
            // into the desired output format (e.g., .typst file).
            var index = 0;
            var chapterList = ImmutableArray.CreateBuilder<string>();

            var content = "";
            FileStream? handle = null;

            foreach (var paragraph in document.Paragraphs)
            {
                if (paragraph.StyleId == "Titre1")
                {
                    if (content != "")
                    {
                        handle!.Write(System.Text.Encoding.UTF8.GetBytes(content));
                        handle.Flush();
                        handle.Close();
                        index++;
                    }
                    var title = string.Join(" ", paragraph.Runs.Select(r => r.Text));
                    var fileName = $"{index:00} - {title}.typ";
                    var file = $"Chapitres/{fileName}";

                    if (File.Exists(file))
                    {
                        // Delete file
                        File.Delete(file);
                    }

                    chapterList.Add($"#include \"{fileName}\"");

                    handle = File.OpenWrite(file);

                    // open file for writing and write chapter content
                    content = $"= {title}";
                }
                else
                {
                    foreach (var run in paragraph.Runs)
                    {
                        if (run.Italic) content += "_";
                        content += run.Text;
                        if (run.Italic) content += "_";
                    }
                }
                content += "\n\n";
            }

            handle!.Write(System.Text.Encoding.UTF8.GetBytes(content));
            handle.Flush();
            handle.Close();

            if (File.Exists("Chapitres/_index.typ"))
            {
                // Delete file
                File.Delete("Chapitres/_index.typ");
            }
            var handleIndex = File.OpenWrite("Chapitres/_index.typ");
            handleIndex.Write(System.Text.Encoding.UTF8.GetBytes(string.Join("\n", chapterList)));
            handleIndex.Flush();
            handleIndex.Close();
        }
    }
}
