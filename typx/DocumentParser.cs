using DocumentFormat.OpenXml.Packaging;
using System.Collections.Immutable;
using M = typx.Models;
using W = DocumentFormat.OpenXml.Wordprocessing;

namespace typx
{
    public class DocumentParser
    {
        public static M.Document Parse(string docxFilePath)
        {
            using var doc = WordprocessingDocument.Open(docxFilePath, false);
            var mainPart = doc.MainDocumentPart
                ?? throw new InvalidOperationException("Document has no main part.");
            var wDoc = mainPart.Document
                ?? throw new InvalidOperationException("Document has no document element.");
            var body = wDoc.Body
                ?? throw new InvalidOperationException("Document has no body.");

            //var styles = ParseStyles(mainPart.StyleDefinitionsPart);
            //var numbering = ParseNumbering(mainPart.NumberingDefinitionsPart);
            //var defaults = ParseDocumentDefaults(mainPart.StyleDefinitionsPart);
            var paragraphs = ParseBody(body);
            //var footnotes = ParseFootnotes(mainPart.FootnotesPart);

            return new M.Document(paragraphs);
        }

        private static ImmutableArray<M.Paragraph> ParseBody(W.Body body)
        {
            var paragraphsBuilder = ImmutableList.CreateBuilder<M.Paragraph>();

            foreach (var child in body.ChildElements)
            {
                switch (child)
                {
                    case W.Paragraph p:
                        var paragraph = ParseParagraph(p);
                        paragraphsBuilder.Add(paragraph);
                        break;
                    case W.SectionProperties:
                        // Silently ignore section properties here.
                        break;
                    default:
                        throw new NotSupportedException($"Unsupported block-level element: {child.GetType().Name}");
                }
            }

            return paragraphsBuilder.ToImmutableArray();
        }

        private static M.Paragraph ParseParagraph(W.Paragraph p)
        {
            var props = p.ParagraphProperties;

            var styleId = props?.ParagraphStyleId?.Val?.ToString() ?? "Normal";

            switch (styleId)
            {
                case "Titre1":
                case "Normal":
                case "Elipse":
                    var runsBuilder = ImmutableArray.CreateBuilder<M.Run>();

                    foreach (var child in p.ChildElements)
                    {
                        switch (child)
                        {
                            case W.Run r:
                                var run = ParseRun(r);
                                runsBuilder.Add(run);
                                break;
                            case W.ProofError pe:
                                break;
                            case W.ParagraphProperties:
                            case W.BookmarkStart:
                            case W.BookmarkEnd:
                                // Silently ignore those
                                break;
                            default:
                                throw new NotSupportedException($"Unsupported paragrah-level element: {child.GetType().Name}");
                        }

                    }

                    return new M.Paragraph(styleId, runsBuilder.ToImmutableArray());
                case "Titre":
                    // Silently ignore this style; it's used for the title page, which we don't care about
                    return new M.Paragraph(styleId, ImmutableArray<M.Run>.Empty);
                default:
                    throw new NotSupportedException($"Paragraphs style {styleId} not supported.");
            }
        }

        private static M.Run ParseRun(W.Run r)
        {
            string text = "";
            bool italic = false;

            foreach (var child in r.ChildElements)
            {
                switch (child)
                {
                    case W.Text t:
                        text = t.InnerText ?? "";
                        //Console.WriteLine($"Found text: {text}");
                        break;
                    case W.RunProperties rp:
                        foreach (var property in rp.ChildElements)
                        {
                            switch (property)
                            {
                                case W.Italic:
                                case W.ItalicComplexScript:
                                    italic = true;
                                    break;
                                case W.Languages:
                                    // Silently ignore language properties for now
                                    break;
                                default:
                                   throw new NotSupportedException($"Unsupported run property: {property.GetType().Name}");
                            }
                        }
                        break;
                    case W.LastRenderedPageBreak:
                        // Silently ignore this element; it has no semantic meaning for our purposes
                        break;
                    default:
                        throw new NotSupportedException($"Unsupported run-level element: {child.GetType().Name}");
                }
            }

            return new M.Run(text, italic);
        }
    }
}
