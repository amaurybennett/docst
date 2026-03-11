using System.Collections.Immutable;

namespace typx.Models;

public record Document(ImmutableArray<Paragraph> Paragraphs);

public record Paragraph(string StyleId, ImmutableArray<Run> Runs);

public record Run(string Text, bool Italic);
