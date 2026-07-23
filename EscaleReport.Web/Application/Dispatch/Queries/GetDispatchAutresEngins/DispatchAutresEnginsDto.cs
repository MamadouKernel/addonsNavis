using EscaleReport.Web.Application.Common.Models;
using EscaleReport.Web.Domain.Dispatch;

namespace EscaleReport.Web.Application.Dispatch.Queries.GetDispatchAutresEngins;

public class DispatchAutresEnginsDto
{
    public AutresEnginsEffectifDto Effectif { get; set; } = new();
    public PagedResult<EnginProblemeDto> Problemes { get; set; } = new();
    public PagedResult<EnginDeconnexionDto> Deconnexions { get; set; } = new();
    public PagedResult<RemplacementOperateurDto> Remplacements { get; set; } = new();

    // Comptes "en cours" pour les pastilles d'onglet : calculés sur l'ensemble des enregistrements.
    public int ProblemesEnCoursCount { get; set; }
    public int DeconnexionsEnCoursCount { get; set; }
}

public class AutresEnginsEffectifDto
{
    public int DisponibleReachStackers { get; set; }
    public int DisponibleEmptyHandlers { get; set; }
    public int DisponibleAutres { get; set; }

    // CDC §9.1 : "calculé automatiquement" à partir des problèmes d'engins ouverts avec
    // retrait d'effectif — pas saisi par l'utilisateur.
    public int RetireReachStackers { get; set; }
    public int RetireEmptyHandlers { get; set; }
    public int RetireAutres { get; set; }
}

public class EnginProblemeDto
{
    public Guid Id { get; set; }
    public string Engin { get; set; } = string.Empty;
    public CategorieEngin Categorie { get; set; }
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateFinUtc { get; set; }
    public TimeSpan? Duree { get; set; }
    public string Probleme { get; set; } = string.Empty;
    public bool RetireEffectif { get; set; }
    public string? ActionRealisee { get; set; }
    public bool EstResolu { get; set; }
}

public class EnginDeconnexionDto
{
    public Guid Id { get; set; }
    public string Engin { get; set; } = string.Empty;
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateRetourUtc { get; set; }
    public TimeSpan? Duree { get; set; }
    public string? Motif { get; set; }
    public bool EstResolu { get; set; }
}

public class RemplacementOperateurDto
{
    public Guid Id { get; set; }
    public string Operateur { get; set; } = string.Empty;
    public string EnginQuitte { get; set; } = string.Empty;
    public string NouvelEngin { get; set; } = string.Empty;
    public DateTime DateHeureUtc { get; set; }
    public string? Raison { get; set; }
    public string? Commentaire { get; set; }
}
