$ErrorActionPreference = 'Stop'

function Update-ExactFile {
    param(
        [Parameter(Mandatory)] [string] $Path,
        [Parameter(Mandatory)] [hashtable[]] $Replacements
    )

    $resolved = (Resolve-Path -LiteralPath $Path).Path
    $text = [IO.File]::ReadAllText($resolved).Replace("`r`n", "`n")
    foreach ($replacement in $Replacements) {
        $old = ([string]$replacement.Old).Replace("`r`n", "`n")
        $new = ([string]$replacement.New).Replace("`r`n", "`n")
        if (-not $text.Contains($old)) {
            if ($text.Contains($new)) {
                continue
            }
            throw "Bloc attendu introuvable dans $Path : $($old.Substring(0, [Math]::Min(100, $old.Length)))"
        }
        $text = $text.Replace($old, $new)
    }
    [IO.File]::WriteAllText($resolved, $text.Replace("`n", [Environment]::NewLine), [Text.UTF8Encoding]::new($false))
}

$details = 'EscaleReport.Web\Views\Escales\Details.cshtml'
Update-ExactFile -Path $details -Replacements @(
    @{
        Old = @'
    static string SensLabel(Sens s) => s == Sens.Debarquement ? "Débarquement" : "Embarquement";
'@
        New = @'
    static string SensLabel(Sens s) => s == Sens.Debarquement ? "Débarquement" : "Embarquement";
    static string GraviteLabel(IncidentGravite g) => g switch
    {
        IncidentGravite.Critique => "Critique",
        IncidentGravite.Moyen => "Moyen",
        _ => "Information"
    };
'@
    },
    @{
        Old = @'
        <a asp-action="Report" asp-route-id="@Model.Escale.Id" class="btn-primary">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z" /><path d="M14 2v6h6" /></svg>
            Rapport PDF
        </a>
        <a asp-action="ExportExcel" asp-route-id="@Model.Escale.Id" class="btn-secondary">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z" /><path d="M14 2v6h6" /><path d="M9.5 13l5 6M14.5 13l-5 6" /></svg>
            Export Excel
        </a>
'@
        New = @'
        @if (CurrentUser.HasPermission(Permissions.GenererPdf))
        {
            <a asp-action="Report" asp-route-id="@Model.Escale.Id" class="btn-primary">
                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z" /><path d="M14 2v6h6" /></svg>
                Rapport PDF
            </a>
        }
        @if (CurrentUser.HasPermission(Permissions.GenererExcel))
        {
            <a asp-action="ExportExcel" asp-route-id="@Model.Escale.Id" class="btn-secondary">
                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z" /><path d="M14 2v6h6" /><path d="M9.5 13l5 6M14.5 13l-5 6" /></svg>
                Export Excel
            </a>
        }
'@
    },
    @{
        Old = @'
                        <td class="hidden md:table-cell">@a.Position</td>
'@
        New = @'
                        <td class="hidden md:table-cell">
                            <form asp-controller="VesselPlanning" asp-action="UpdateAnomalyPosition" method="post">
                                <input type="hidden" name="anomalyId" value="@a.Id" />
                                <input type="hidden" name="escaleId" value="@Model.Escale.Id" />
                                <input name="position" value="@a.Position" aria-label="Position de @a.NumeroConteneur"
                                       class="js-auto-submit input-field min-w-24 py-1 text-xs" />
                            </form>
                        </td>
'@
    },
    @{
        Old = @'
                                <span class="badge-success">Résolu @(a.ResoluPar is not null ? "· " + a.ResoluPar : "")</span>
'@
        New = @'
                                <span class="badge-success">
                                    Résolu le @(a.DateResolutionUtc?.ToString("dd/MM/yyyy HH:mm") ?? "—")
                                    @(a.ResoluPar is not null ? "· " + a.ResoluPar : "")
                                </span>
'@
    },
    @{
        Old = @'
                        <td class="text-right">
                            @if (a.Statut == AnomalyStatus.NonResolu)
                            {
                                <form asp-controller="VesselPlanning" asp-action="ResolveAnomaly" method="post">
                                    <input type="hidden" name="anomalyId" value="@a.Id" />
                                    <input type="hidden" name="escaleId" value="@Model.Escale.Id" />
                                    <button type="submit" class="btn-link whitespace-nowrap">Marquer résolu</button>
                                </form>
                            }
                        </td>
'@
        New = @'
                        <td class="text-right">
                            <div class="inline-flex flex-col items-end gap-1">
                                @if (a.Statut == AnomalyStatus.NonResolu)
                                {
                                    <form asp-controller="VesselPlanning" asp-action="ResolveAnomaly" method="post">
                                        <input type="hidden" name="anomalyId" value="@a.Id" />
                                        <input type="hidden" name="escaleId" value="@Model.Escale.Id" />
                                        <button type="submit" class="btn-link whitespace-nowrap">Marquer résolu</button>
                                    </form>
                                }
                                <form asp-controller="VesselPlanning" asp-action="DeleteAnomaly" method="post"
                                      onsubmit="return confirm('Supprimer cette anomalie ?');">
                                    <input type="hidden" name="anomalyId" value="@a.Id" />
                                    <input type="hidden" name="escaleId" value="@Model.Escale.Id" />
                                    <button type="submit" class="btn-link whitespace-nowrap text-rose-700">Supprimer</button>
                                </form>
                            </div>
                        </td>
'@
    },
    @{
        Old = @'
        <div><label class="field-label">Conteneur</label><input name="NumeroConteneur" required class="input-field" /></div>
'@
        New = @'
        <div><label class="field-label">Conteneur(s), un par ligne</label><textarea name="NumeroConteneur" rows="3" required class="input-field"></textarea></div>
'@
    },
    @{
        Old = @'
                    <div><label class="field-label">Embarqué</label><input name="QuantiteEmbarquee" type="number" min="0" value="@v.QuantiteEmbarquee" class="input-field py-1.5 text-xs" /></div>
'@
        New = @'
                    <div><label class="field-label">Embarqué</label><input name="QuantiteEmbarquee" type="number" min="0" value="@v.QuantiteEmbarquee" class="js-auto-submit input-field py-1.5 text-xs" /></div>
'@
    },
    @{
        Old = @'
                        <th>Début</th>
                        <th class="hidden lg:table-cell">Durée</th>
                        <th>Gravité</th>
                        <th>Statut</th>
                        <th></th>
'@
        New = @'
                        <th>Début</th>
                        <th class="hidden lg:table-cell">Fin</th>
                        <th class="hidden lg:table-cell">Durée</th>
                        <th>Gravité</th>
                        <th>Statut</th>
                        <th class="hidden xl:table-cell">Description</th>
                        <th>Compléter</th>
'@
    },
    @{
        Old = @'
                    <tr><td colspan="7" class="!text-center text-slate-400 py-8">Aucun incident déclaré.</td></tr>
'@
        New = @'
                    <tr><td colspan="9" class="!text-center text-slate-400 py-8">Aucun incident déclaré.</td></tr>
'@
    },
    @{
        Old = @'
                        <td class="whitespace-nowrap">@i.DateDebutUtc.ToString("dd/MM HH:mm")</td>
                        <td class="hidden lg:table-cell">@(i.Duree.HasValue ? $"{(int)i.Duree.Value.TotalHours}h{i.Duree.Value.Minutes:D2}" : "—")</td>
'@
        New = @'
                        <td class="whitespace-nowrap">@i.DateDebutUtc.ToString("dd/MM HH:mm")</td>
                        <td class="hidden whitespace-nowrap lg:table-cell">@(i.DateFinUtc?.ToString("dd/MM HH:mm") ?? "—")</td>
                        <td class="hidden lg:table-cell">@(i.Duree.HasValue ? $"{(int)i.Duree.Value.TotalHours}h{i.Duree.Value.Minutes:D2}" : "—")</td>
'@
    },
    @{
        Old = @'
                        <td class="text-right">
                            @if (i.Statut == IncidentStatus.EnCours)
                            {
                                <form asp-controller="VesselPlanning" asp-action="ResolveIncident" method="post">
                                    <input type="hidden" name="incidentId" value="@i.Id" />
                                    <input type="hidden" name="escaleId" value="@Model.Escale.Id" />
                                    <button type="submit" class="btn-link whitespace-nowrap">Résoudre</button>
                                </form>
                            }
                        </td>
'@
        New = @'
                        <td class="hidden max-w-56 xl:table-cell">@i.Description</td>
                        <td class="text-right">
                            <form asp-controller="VesselPlanning" asp-action="UpdateIncident" method="post"
                                  class="inline-grid min-w-48 gap-1">
                                <input type="hidden" name="incidentId" value="@i.Id" />
                                <input type="hidden" name="escaleId" value="@Model.Escale.Id" />
                                <input name="dateFinUtc" type="datetime-local"
                                       value="@i.DateFinUtc?.ToString("yyyy-MM-ddTHH:mm")"
                                       min="@i.DateDebutUtc.ToString("yyyy-MM-ddTHH:mm")"
                                       aria-label="Fin de l'incident" class="input-field py-1 text-xs" />
                                <input name="description" value="@i.Description" placeholder="Description"
                                       aria-label="Description de l'incident" class="input-field py-1 text-xs" />
                                <input name="actionRealisee" value="@i.ActionRealisee" placeholder="Action réalisée"
                                       aria-label="Action réalisée" class="input-field py-1 text-xs" />
                                <button type="submit" class="btn-link whitespace-nowrap">Mettre à jour</button>
                            </form>
                            @if (i.Statut == IncidentStatus.EnCours)
                            {
                                <form asp-controller="VesselPlanning" asp-action="ResolveIncident" method="post" class="mt-1">
                                    <input type="hidden" name="incidentId" value="@i.Id" />
                                    <input type="hidden" name="escaleId" value="@Model.Escale.Id" />
                                    <button type="submit" class="btn-link whitespace-nowrap">Résoudre maintenant</button>
                                </form>
                            }
                        </td>
'@
    },
    @{
        Old = @'
    <form asp-controller="VesselPlanning" asp-action="AddIncident" method="post"
          class="grid grid-cols-1 gap-4 rounded-xl border border-slate-200 bg-slate-50/60 p-5 sm:grid-cols-2 lg:grid-cols-5">
'@
        New = @'
    <form asp-controller="VesselPlanning" asp-action="AddIncident" method="post"
          class="grid grid-cols-1 gap-4 rounded-xl border border-slate-200 bg-slate-50/60 p-5 sm:grid-cols-2 lg:grid-cols-6">
'@
    },
    @{
        Old = @'
        <div><label class="field-label">Début</label><input name="DateDebutUtc" type="datetime-local" class="input-field" /></div>
        <div>
'@
        New = @'
        <div><label class="field-label">Début</label><input name="DateDebutUtc" type="datetime-local" required class="input-field" /></div>
        <div><label class="field-label">Fin (facultative)</label><input name="DateFinUtc" type="datetime-local" class="input-field" /></div>
        <div>
'@
    },
    @{
        Old = @'
            <select name="Gravite" class="input-field">
                <option value="@((int)IncidentGravite.Information)">Information</option>
                <option value="@((int)IncidentGravite.Moyen)">Moyen</option>
                <option value="@((int)IncidentGravite.Critique)">Critique</option>
            </select>
'@
        New = @'
            <select name="Gravite" class="input-field">
                @foreach (var gravite in Model.GravitesIncidentDisponibles)
                {
                    <option value="@((int)gravite)">@GraviteLabel(gravite)</option>
                }
            </select>
'@
    },
    @{
        Old = @'
        <div class="sm:col-span-2 lg:col-span-5">
'@
        New = @'
        <div class="sm:col-span-2 lg:col-span-6">
'@
    },
    @{
        Old = @'
        <div><label class="field-label">Conteneur</label><input name="NumeroConteneur" required class="input-field" /></div>
'@
        New = @'
        <div><label class="field-label">Conteneur(s), un par ligne</label><textarea name="NumeroConteneur" rows="3" required class="input-field"></textarea></div>
'@
    },
    @{
        Old = @'
    <form asp-controller="VesselPlanning" asp-action="AddDangerous" method="post"
          class="grid grid-cols-1 gap-4 rounded-xl border border-slate-200 bg-slate-50/60 p-5 sm:grid-cols-2 lg:grid-cols-5">
'@
        New = @'
    <form asp-controller="VesselPlanning" asp-action="AddDangerous" method="post"
          class="grid grid-cols-1 gap-4 rounded-xl border border-slate-200 bg-slate-50/60 p-5 sm:grid-cols-2 lg:grid-cols-7">
'@
    },
    @{
        Old = @'
        <div><label class="field-label">Conteneur</label><input name="NumeroConteneur" required class="input-field" /></div>
'@
        New = @'
        <div><label class="field-label">Conteneur(s), un par ligne</label><textarea name="NumeroConteneur" rows="3" required class="input-field"></textarea></div>
'@
    },
    @{
        Old = @'
        <div><label class="field-label">Validité BADT</label><input name="DateValiditeBadt" type="date" class="input-field" /></div>
        <div class="flex items-end lg:col-span-5">
'@
        New = @'
        <div><label class="field-label">Validité BADT</label><input name="DateValiditeBadt" type="date" class="input-field" /></div>
        <div>
            <label class="field-label">État BADT</label>
            <select name="StatutBadt" class="input-field">
                @foreach (BadtStatus status in Enum.GetValues<BadtStatus>())
                {
                    <option value="@((int)status)">@BadtLabel(status)</option>
                }
            </select>
        </div>
        <div>
            <label class="field-label">Statut opérationnel</label>
            <select name="StatutOperationnel" class="input-field">
                @foreach (DangerousContainerStatus status in Enum.GetValues<DangerousContainerStatus>())
                {
                    <option value="@((int)status)">@StatutOpLabel(status)</option>
                }
            </select>
        </div>
        <div class="flex items-end lg:col-span-7">
'@
    }
)

Update-ExactFile -Path 'EscaleReport.Web\Views\Escales\Index.cshtml' -Replacements @(
    @{
        Old = @'
                    else
                    {
                        <span class="badge-success">Complète</span>
                    }
'@
        New = @'
                    else
                    {
                        <span class="badge-success">Complète</span>
                    }
                    <span class="badge-danger">@e.AnomaliesNonResoluesCount anomalie(s)</span>
                    <span class="badge-info">@e.AdditionnelsCount additionnel(s)</span>
'@
    }
)

Write-Output 'Vues des escales et compteurs du tableau de bord mis à jour.'
