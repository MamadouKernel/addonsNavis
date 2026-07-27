$ErrorActionPreference = 'Stop'

$path = (Resolve-Path '.\scripts\build-recette-wave1.mjs').Path
$text = [IO.File]::ReadAllText($path).Replace("`r`n", "`n")

$anchor = @'
for(const [row,values] of Object.entries(wave2Updates)){testSheet.getRange(`H${row}`).values=[[values[0]]];testSheet.getRange(`I${row}`).values=[[values[1]]];testSheet.getRange(`K${row}`).values=[[values[2]]];}

'@

$wave3 = @'
for(const [row,values] of Object.entries(wave2Updates)){testSheet.getRange(`H${row}`).values=[[values[0]]];testSheet.getRange(`I${row}`).values=[[values[1]]];testSheet.getRange(`K${row}`).values=[[values[2]]];}

const wave3Updates={
51:["Les sept raisons actives correspondent exactement à la liste « Raisons d'anomalie » du paramétrage.","OK","Rejoué le 24/07/2026 — comparaison HTML entre la fiche escale et /Parametrage."],
52:["Un collage de trois numéros crée trois anomalies distinctes avec les mêmes attributs.","OK","Corrigé et rejoué — découpage CR/LF, validation et trois lignes persistées."],
53:["Le collage comportant une ligne vide crée uniquement les trois conteneurs réels ; compteur = 3.","OK","Corrigé et rejoué — lignes vides ignorées et doublons normalisés."],
54:["La position BAY-CORRIGEE est conservée après modification et rechargement.","OK","Corrigé et rejoué — commande UpdateContainerAnomalyPosition."],
55:["L'anomalie reste visible avec le statut Résolu, la date et l'utilisateur de résolution.","OK","Corrigé et rejoué — date de résolution rendue dans la ligne."],
56:["Le compteur de la carte passe de 3 à 4 après l'ajout.","OK","Corrigé et rejoué — agrégation des anomalies non résolues sur le tableau de bord."],
57:["Après résolution, le compteur des anomalies non résolues revient de 4 à 3.","OK","Corrigé et rejoué — compteur limité aux statuts Non résolu."],
58:["La ligne supprimée ne réapparaît pas après rechargement.","OK","Corrigé et rejoué — commande DeleteContainerAnomaly et suppression persistante."],
59:["La section 1 du PDF contient les anomalies résolues et non résolues.","OK","Rejoué — PDF HTTP 200, extraction texte et rendu visuel Poppler (2 pages)."],
60:["La cible REC-VID-50 est ajoutée et restituée dans l'onglet des vides.","OK","Rejoué — POST AddEmptyTarget et lecture de la fiche."],
61:["Avec 50 souhaités, 10 ajoutés, 30 embarqués et 5 coupés, le reste affiché est 25.","OK","Rejoué — formule métier 50 + 10 - 30 - 5 = 25."],
62:["Après passage de l'embarqué à 31, le reste est recalculé à 24 au changement du champ.","OK","Corrigé et rejoué — soumission automatique du champ Embarqué et recalcul serveur."],
63:["Le reste est une valeur calculée rendue en lecture seule ; aucun champ QuantiteRestante n'existe.","OK","Rejoué — contrôle du HTML rendu."],
64:["Une coupure sans motif est refusée avec un message explicite et les anciennes valeurs sont conservées.","OK","Corrigé et rejoué — validation contrôleur et handler."],
65:["Pour trois lignes, les totaux sont 100 souhaités, 10 ajoutés, 40 planifiés, 31 embarqués, 5 coupés et 74 restants.","OK","Rejoué — agrégats calculés sur toutes les lignes de l'escale."],
66:["Une quantité négative est signalée et aucune ligne négative n'est créée.","OK","Corrigé et rejoué — validation des ajouts et mises à jour."],
67:["La section 2 du PDF contient les trois lignes et les totaux 100/10/40/31/5/74.","OK","Corrigé et rejoué — colonne Ajoutée et ligne de totaux ajoutées au PDF."],
68:["L'incident REC-INC-DUREE est ajouté et restitué avec toutes ses valeurs.","OK","Rejoué — POST AddIncident puis rechargement."],
69:["Un début à 08:00 et une fin à 09:30 donnent automatiquement une durée de 1h30.","OK","Corrigé et rejoué — DateFinUtc saisissable et durée calculée."],
70:["Sans heure de fin, l'incident affiche En cours et aucune durée.","OK","Rejoué — statut EnCours et durée absente."],
71:["La description vide à la création est complétée ultérieurement puis restituée sans perte.","OK","Corrigé et rejoué — commande UpdateOperationalIncident."],
72:["Une fin à 09:00 pour un début à 10:00 est refusée ; aucune durée négative n'est affichée.","OK","Corrigé et rejoué — validation chronologique et garde domaine."],
73:["Les sept catégories et les trois gravités actives proviennent des listes du paramétrage.","OK","Corrigé et rejoué — nouvelle liste Gravités d'incident, seed et validation active."],
74:["La section 3 du PDF contient début, fin, durée 1h30, gravité, statut et description.","OK","Corrigé et rejoué — PDF accessible au Vessel Planner et colonnes complètes."],
75:["Le conteneur MSCU8000001 est ajouté et restitué dans le tableau.","OK","Rejoué — POST AddAdditional puis lecture."],
76:["La décision Embarqué est conservée et reste sélectionnée après rechargement.","OK","Rejoué — SetAdditionalDecision, valeur 2 persistée."],
77:["Un collage de trois numéros crée trois lignes additionnelles distinctes.","OK","Corrigé et rejoué — saisie multiligne normalisée."],
78:["La section 4 du PDF contient les quatre additionnels et leurs décisions.","OK","Rejoué — extraction et contrôle visuel du PDF."],
79:["Le compteur de la carte passe de 0 à 4 après les ajouts.","OK","Corrigé et rejoué — compteur additionnels agrégé par escale."],
80:["Le conteneur MSCU9000001 est ajouté avec classe IMO, position, validité et état BADT.","OK","Corrigé et rejoué — état BADT et statut opérationnel disponibles dès l'ajout."],
81:["Les trois états BADT sont proposés : Non pris, Pris et À renouveler.","OK","Rejoué — contrôle des options du formulaire."],
82:["La ligne À renouveler possède un fond rose et un badge danger distinctif.","OK","Rejoué — contrôle du HTML rendu."],
83:["Dans le PDF, la ligne À renouveler reste sur fond rouge avec le symbole d'alerte.","OK","Corrigé et rejoué — rendu visuel Poppler, page 2."],
84:["Un collage de trois numéros crée trois lignes de conteneurs dangereux.","OK","Corrigé et rejoué — saisie multiligne normalisée."],
85:["La section 5 du PDF contient conteneur, classe IMO, BADT et statut opérationnel.","OK","Rejoué — extraction texte et rendu visuel du PDF."]
};
for(const [row,values] of Object.entries(wave3Updates)){testSheet.getRange(`H${row}`).values=[[values[0]]];testSheet.getRange(`I${row}`).values=[[values[1]]];testSheet.getRange(`K${row}`).values=[[values[2]]];}

'@

if (-not $text.Contains($anchor)) {
    throw 'Ancre wave2 introuvable dans le générateur.'
}
$text = $text.Replace($anchor, $wave3)
$text = $text.Replace('"wave1-previews"', '"wave3-previews"')
$text = $text.Replace('range:"A34:K50"', 'range:"A34:K85"')
$text = $text.Replace('tableMaxRows:17', 'tableMaxRows:52')
$text = $text.Replace('EscaleReport_cahier_de_recette_execution_vague2_2026-07-24.xlsx', 'EscaleReport_cahier_de_recette_execution_vague3_2026-07-24.xlsx')

[IO.File]::WriteAllText(
    $path,
    $text.Replace("`n", [Environment]::NewLine),
    [Text.UTF8Encoding]::new($false))

Write-Output 'Générateur du cahier enrichi avec les 35 résultats de la vague 3.'
