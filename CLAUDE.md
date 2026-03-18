# CLAUDE.md

## Projet

**docst** — outil CLI .NET 10 qui convertit des fichiers `.docx` en Typst (`.typ`) et Markdown/EPUB (`.md`).

## Commandes essentielles

```bash
dotnet build                  # build debug
dotnet build -c Release       # build release
dotnet run -- --docx <file>   # convertir un fichier
dotnet run                    # convertir tous les .docx du dossier courant
```

## Architecture

```
docst/
├── Program.cs           # Point d'entrée, parsing CLI (System.CommandLine)
├── DocumentParser.cs    # Lecture des .docx via OpenXml
├── Nodes.cs             # Modèles de données : Document, Paragraph, Run
├── AbstractExporter.cs  # Classe de base avec logique d'export commune
├── TypstExporter.cs     # Export vers .typ
└── EpubstExporter.cs    # Export vers .md
```

## Modèle de données

```csharp
record Document(ImmutableArray<Paragraph> Paragraphs);
record Paragraph(string StyleId, ImmutableArray<Run> Runs);
record Run(string Text, bool Italic);
```

## Ajouter un nouveau format d'export

1. Créer une classe héritant de `BaseExporter`
2. Implémenter `GetFileExtension()`, `GetTitre1()`, `GetElipse()`, `GetItalic()`
3. Instancier et ajouter l'exporteur dans `Program.cs`

## Styles Word supportés

| StyleId  | Signification         |
|----------|-----------------------|
| `Titre1` | Titre de section (H1) |
| `Elipse` | Sous-titre (H2)       |
| `Normal` | Paragraphe courant    |
| `Titre`  | Page de titre (ignoré)|

## Dépendances

- `DocumentFormat.OpenXml` 3.4.1 — lecture des .docx
- `System.CommandLine` 2.0.3 — parsing des arguments CLI

## Conventions

- Messages de commit en Conventional Commits (`feat:`, `fix:`, `refactor:`, etc.)
- Langue du code : anglais
- Langue des commits et discussions : français
