# docst

Outil en ligne de commande pour convertir des documents Word (`.docx`) en formats alternatifs.

## Formats de sortie

| Format | Extension | Description |
|--------|-----------|-------------|
| Typst  | `.typ`    | Langage de composition pour générer des PDF |
| Markdown | `.md`  | Format compatible EPUB et Markdown standard |

## Prérequis

- [.NET 10](https://dotnet.microsoft.com/download)

## Installation

```bash
git clone https://github.com/amaurybennett/docst.git
cd docst
dotnet build -c Release
```

## Utilisation

**Convertir un fichier spécifique :**

```bash
dotnet run -- --docx <chemin/vers/fichier.docx>
```

**Convertir tous les `.docx` du répertoire courant :**

```bash
dotnet run
```

Chaque document produit un fichier `.typ` et un fichier `.md` dans le même répertoire que le document source.

## Styles Word pris en charge

| Style Word | Rendu |
|------------|-------|
| `Titre1`   | Titre de section (`=` en Typst, `#` en Markdown) |
| `Elipse`   | Sous-titre (`==` en Typst, `.elipse` en Markdown) |
| `Normal`   | Paragraphe de texte courant |
| Italique   | Texte en italique dans les deux formats |

## Licence

MIT — voir [LICENSE](LICENSE)
