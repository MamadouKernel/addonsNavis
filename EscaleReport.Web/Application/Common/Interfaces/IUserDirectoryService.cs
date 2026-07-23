namespace EscaleReport.Web.Application.Common.Interfaces;

// CDC §17 filtre "équipe" : BaseAuditableEntity.CreatedBy capture déjà le nom d'utilisateur de
// l'auteur de chaque enregistrement (voir AuditableEntitySaveChangesInterceptor), mais l'équipe
// n'est portée que par ApplicationUser (Infrastructure/Identity), inaccessible depuis
// l'Application. Cette interface fait le pont sans faire fuiter UserManager/ApplicationUser
// vers la couche Application — même principe que ICurrentUserService.
public interface IUserDirectoryService
{
    Task<IReadOnlyDictionary<string, string?>> GetEquipesByUserNameAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<string>> GetEquipesDisponiblesAsync(CancellationToken cancellationToken);
}
