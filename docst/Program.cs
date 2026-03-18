using System.CommandLine;
using docst;

Option<FileInfo?> docxOption = new("--docx", "-d") { Description = "Path to a .docx file to process." };

var rootCommand = new RootCommand("docst — converts .docx files to Epub and Typst formats.")
{
    docxOption
};

ParseResult parseResult = rootCommand.Parse(args);
FileInfo? fileInfo = parseResult.GetValue<FileInfo?>("--docx");

List<string> docxFiles = [];

if (fileInfo != null)
{
    if (fileInfo.Exists)
    {
        docxFiles = [fileInfo.FullName];
    }
}
else
{
    docxFiles = [.. Directory.GetFiles(Directory.GetCurrentDirectory(), "*.docx")
        .Where(f => !Path.GetFileName(f).StartsWith("~$"))
        .Select(f => Path.GetFileName(f))];
}

if (docxFiles.Count == 0)
{
    Console.WriteLine("No .docx files found in the current directory.");
    return;
}

TypstExporter typstExporter = new();
EpubstExporter epubstExporter = new();
foreach (string docxFile in docxFiles)
{
    Console.WriteLine($"Processing: {docxFile}");
    var document = DocumentParser.Parse(docxFile);
    typstExporter.Export(document);
    epubstExporter.Export(document);
}
