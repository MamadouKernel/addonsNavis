$ErrorActionPreference = 'Stop'

$path = (Resolve-Path -LiteralPath '.\scripts\build-recette-wave1.mjs').Path
$content = [IO.File]::ReadAllText($path).Replace("`r`n", "`n")

$content = $content.Replace(
    '"wave3-previews"',
    '"wave4-previews"')

$marker = 'for(const [row,values] of Object.entries(wave3Updates)){testSheet.getRange(`H${row}`).values=[[values[0]]];testSheet.getRange(`I${row}`).values=[[values[1]]];testSheet.getRange(`K${row}`).values=[[values[2]]];}'
if (-not $content.Contains($marker)) {
    throw 'Boucle wave3Updates introuvable.'
}

$wave4 = @'
for(const [row,values] of Object.entries(wave3Updates)){testSheet.getRange(`H${row}`).values=[[values[0]]];testSheet.getRange(`I${row}`).values=[[values[1]]];testSheet.getRange(`K${row}`).values=[[values[2]]];}

const wave4Updates={
17:["Le Cargo Controller ne voit aucun sous-statut de planification dans le tableau ni dans la fiche cargo.","OK","Rejoué le 24/07/2026 — GET /Cargo et /Cargo/Detail/{id} = HTTP 200 ; aucune valeur Non planifié/Planifié/Plan validé."],
31:["Le journal contient CreateEscaleCommand avec son horodatage et l'auteur vplanner.","OK","Rejoué le 24/07/2026 — GET /Audit?actionType=CreateEscaleCommand = HTTP 200, action, date/heure et auteur contrôlés."],
86:["La date du 24/07/2026 et le shift Matin sont affichés dans des contrôles modifiables.","OK","Rejoué le 24/07/2026 — input date et liste shift actifs, non disabled/readonly."],
87:["Les saisies du 24/07 sont absentes le 25/07 puis réapparaissent sans perte au retour.","OK","Corrigé et rejoué — affectations, incidents, pointeurs et ROPN filtrés par date/shift."],
88:["La ligne navire est affichée en lecture seule, sans champ de modification.","OK","Rejoué — contrôle du HTML de la ligne navire."],
89:["Les huit portiques CR1 à CR8 sont présents.","OK","Rejoué — pool fixe complet contrôlé dans le HTML."],
90:["CR1 passe En panne et sa bulle prend l'apparence danger.","OK","Rejoué — POST ChangeGantryStatus, statut persisté et badge danger."],
91:["Le type de panne et la cause apparaissent directement dans la bulle du portique.","OK","Corrigé et rejoué — formulaire inline sans fenêtre modale."],
92:["L'affectation d'un portique en panne est refusée et aucune ligne n'est créée.","OK","Corrigé et rejoué — validation FluentValidation et garde du handler."],
93:["La remise en service clôture automatiquement la panne avec une heure de fin.","OK","Corrigé et rejoué — incident portique passé Repris avec fin calculée."],
94:["Le glisser-déposer est câblé vers l'affectation et crée la liaison portique/navire.","OK","Corrigé et rejoué — HTML draggable/dropzone + même contrat POST AssignGantry exécuté."],
95:["L'heure de début est enregistrée automatiquement au moment de l'affectation.","OK","Corrigé et rejoué — champ optionnel, heure persistée dans la minute du POST."],
96:["L'heure de début corrigée à 08:00 est conservée après rechargement.","OK","Corrigé et rejoué — commande UpdateGantryAssignment."],
97:["Le retrait clôture la ligne à 09:00 sans supprimer l'historique.","OK","Rejoué — EndGantryAssignment avec heure de fin explicite."],
98:["Une affectation de 08:00 à 09:00 affiche une durée de 1h00.","OK","Corrigé et rejoué — durée calculée et affichée."],
99:["CR2 est réaffecté au second navire sans écraser la première affectation.","OK","Rejoué — deux lignes distinctes et historique conservé."],
100:["L'incident CR3 apparaît avec navire, type, début et fin.","OK","Rejoué — POST AddStsIncident et contrôle de la ligne."],
101:["Navire, type et heures sont modifiables ; la durée est recalculée à 2h00.","OK","Corrigé et rejoué — commande UpdateStsIncident et persistance."],
102:["Les incidents navire et pannes portique disposent de deux registres et listes distincts.","OK","Corrigé et rejoué — listes Attente documents navire et Panne spreader séparées."],
103:["Un pointeur Terre avec ses deux horaires affiche une durée de 1h30.","OK","Corrigé et rejoué — ajout, rôle Terre et durée calculée."],
104:["Un ROPN avec ses horaires et sa difficulté affiche une durée de 1h15.","OK","Corrigé et rejoué — champs DateDebutUtc/DateFinUtc persistés par migration."],
105:["Les 7 tracteurs affectés côté TT sont repris sur la ligne du navire côté STS.","OK","Corrigé et rejoué — dernière répartition TT agrégée par escale."]
};
for(const [row,values] of Object.entries(wave4Updates)){testSheet.getRange(`H${row}`).values=[[values[0]]];testSheet.getRange(`I${row}`).values=[[values[1]]];testSheet.getRange(`K${row}`).values=[[values[2]]];}
'@
$content = $content.Replace($marker, $wave4)

$content = $content.Replace(
    'range:"A34:K85",include:"values,formulas",tableMaxRows:52,tableMaxCols:11,maxChars:24000',
    'range:"A86:K105",include:"values,formulas",tableMaxRows:20,tableMaxCols:11,maxChars:30000')

$content = $content.Replace(
    'EscaleReport_cahier_de_recette_execution_vague3_2026-07-24.xlsx',
    'EscaleReport_cahier_de_recette_execution_vague4_2026-07-24.xlsx')

[IO.File]::WriteAllText($path, $content, [Text.UTF8Encoding]::new($false))
Write-Output 'Générateur du cahier enrichi avec les 22 résultats de la vague 4.'
