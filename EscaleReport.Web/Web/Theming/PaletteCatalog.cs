namespace EscaleReport.Web.Web.Theming;

// CDC §15.2 "pourra proposer... une dizaine de thèmes" (indicatif) : bibliothèque de palettes
// nommées, chacune déclinée en clair/sombre dans ClientAssets/input.css (sélecteurs
// :root[data-palette="X"] / :root[data-palette="X"][data-theme="dark"]). Une valeur de cookie
// inconnue retombe sur Lagune plutôt que d'être injectée telle quelle dans l'attribut HTML.
public static class PaletteCatalog
{
    public const string Lagune = "lagune";
    public const string Ocean = "ocean";
    public const string Corail = "corail";
    public const string Ambre = "ambre";
    public const string Foret = "foret";
    public const string Violet = "violet";

    public static readonly IReadOnlyList<(string Key, string Label, string SwatchHex)> All =
    [
        (Lagune, "Lagune", "#15B8A0"),
        (Ocean, "Océan", "#2E86AB"),
        (Corail, "Corail", "#D9714A"),
        (Ambre, "Ambre", "#C99A2E"),
        (Foret, "Forêt", "#2FA66B"),
        (Violet, "Violet", "#8354C9")
    ];

    public static bool IsValid(string? key) => All.Any(p => p.Key == key);

    public static string FromCookie(string? cookieValue) => IsValid(cookieValue) ? cookieValue! : Lagune;
}
