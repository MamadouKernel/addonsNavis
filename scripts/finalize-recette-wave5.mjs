import fs from "node:fs/promises";
import path from "node:path";
import { FileBlob, SpreadsheetFile } from "@oai/artifact-tool";

const root = process.cwd();
const inputPath = path.join(
  root,
  "outputs",
  "recette-2026-07-24",
  "EscaleReport_cahier_de_recette_execution_vague4_2026-07-24.xlsx",
);
const previewDir = path.join(root, "outputs", "recette-2026-07-24", "wave5-previews");

const workbook = await SpreadsheetFile.importXlsx(await FileBlob.load(inputPath));
await fs.mkdir(previewDir, { recursive: true });

if (process.argv.includes("--inspect-vp")) {
  const scenarios = await workbook.inspect({kind:"table",sheetId:"Cahier de test",range:"A34:K95",include:"values,formulas",tableMaxRows:62,tableMaxCols:11,maxChars:50000});
  console.log(scenarios.ndjson);
  process.exit(0);
}

const overview = await workbook.inspect({
  kind: "sheet",
  include: "id,name",
  maxChars: 4000,
});
console.log(overview.ndjson);

const recipeRows = await workbook.inspect({
  kind: "table",
  sheetId: "Cahier de test",
  range: "A1:K40",
  include: "values,formulas",
  tableMaxRows: 40,
  tableMaxCols: 11,
  maxChars: 30000,
});
console.log(recipeRows.ndjson);

for (const sheetName of ["Pre-requis", "Cahier de test", "Synthèse"]) {
  const preview = await workbook.render({
    sheetName,
    autoCrop: "all",
    scale: 0.8,
    format: "png",
  });
  await fs.writeFile(
    path.join(previewDir, `before-${sheetName.replaceAll(" ", "-")}.png`),
    new Uint8Array(await preview.arrayBuffer()),
  );
}


const testSheet = workbook.worksheets.getItem("Cahier de test");
const updates = {
7:["La fenêtre « Poste du jour » propose STS, TT, RTG et Autres engins ; le choix STS ouvre /Dispatch/Sts.","OK","Corrigé et rejoué le 24/07/2026 — connexion dispatcher1, sélection STS, HTTP 200."],
8:["Le lien « Poste du jour » permet de passer de STS à TT puis de revenir à STS sans déconnexion ; l’état persisté reste disponible.","OK","Corrigé et rejoué le 24/07/2026 — /Account/ChooseDispatchPost → /Dispatch/Tt puis /Dispatch/Sts, même session."],
9:["La déconnexion renvoie à l’écran de connexion ; la reconnexion réussit et les données de recette restent présentes.","OK","Rejoué le 24/07/2026 — logout/login HTTP 200 ; ABIDJAN STAR toujours présente."],
14:["Le rôle Administrateur conserve tous les accès sans lignes de permissions dédiées : escales, comptes, paramètres et audit.","OK","Rejoué le 24/07/2026 — GET /Escales/Create, /Users, /Parametrage et /Audit = HTTP 200 avec admin."],
19:["Le Coordinateur voit les données STS mais aucune action de modification Dispatch n’est rendue.","OK","Rejoué le 24/07/2026 — GET /Coordination ; données STS visibles, aucune action Add/Change/Close STS."],
20:["En affectation RTG, le bloc Incidents STS est affiché en lecture seule sans action d’ajout, clôture ou modification.","OK","Rejoué le 24/07/2026 — sélection RTG puis GET /Dispatch/Rtg ; lecture seule et absence d’action STS."],
22:["Le compte recette.auto est créé, apparaît dans la liste et permet une connexion réussie.","OK","Rejoué le 24/07/2026 — création Dispatcher puis connexion HTTP 200."],
23:["La seconde création de recette.auto est refusée avec un message explicite d’identifiant déjà utilisé.","OK","Rejoué le 24/07/2026 — POST /Users/Create en doublon ; message explicite affiché."],
24:["Le poste de recette.auto est passé de RTG à TT et le choix TT est actif après reconnexion.","OK","Rejoué le 24/07/2026 — UpdateProfile, reconnexion, TT affiché comme poste courant."],
25:["L’équipe A affectée à recette.auto est conservée après rechargement de la gestion des comptes.","OK","Rejoué le 24/07/2026 — UpdateProfile puis nouvelle lecture /Users."],
29:["Le nouveau mot de passe permet la connexion et l’ancien mot de passe est refusé.","OK","Rejoué le 24/07/2026 — ResetPassword ; nouveau accepté, ancien rejeté."],
32:["Un compte non administrateur est redirigé vers Accès refusé lorsqu’il ouvre directement le journal.","OK","Rejoué le 24/07/2026 — GET /Audit avec recette.auto → /Account/AccessDenied."]};
for (const [row, values] of Object.entries(updates)) {
  testSheet.getRange(`H${row}`).values=[[values[0]]]; testSheet.getRange(`I${row}`).values=[[values[1]]]; testSheet.getRange(`K${row}`).values=[[values[2]]];
}
const wave2Updates={
34:["Cartes avec navire, voyage, ligne, ETA, quai et statut.","OK","Rejoué le 24/07/2026 — cartes HTML du jeu de recette."],
35:["Les trois escales En cours précèdent les deux escales à venir.","OK","Corrigé et rejoué — tri serveur statut puis ETA."],
36:["Le volet actif exclut les terminées ; Terminées contient uniquement ATLANTIC HOPE et COASTAL SPIRIT.","OK","Corrigé et rejoué — filtres actives/terminees."],
37:["ABIDJAN retourne uniquement ABIDJAN STAR et CMA CGM ABIDJAN.","OK","Corrigé et rejoué — recherche navire."],
38:["415W, Poste 1 et CMA CGM filtrent voyage, quai et ligne.","OK","Corrigé et rejoué — résultats 1, 4 et 1 cartes."],
39:["RECETTE VP TEMP est créée et affichée immédiatement.","OK","Rejoué — POST /Escales/Create puis carte visible."],
40:["Sans nom, le formulaire affiche l’erreur et aucune escale n’est enregistrée.","OK","Rejoué — validation serveur Navire."],
41:["Les dix champs d’entête sont enregistrés et restitués après rechargement.","OK","Corrigé et rejoué — navire, voyage, ligne, quai, ETA, ATA, ETC, visite, shift, planificateur."],
42:["Trois statuts : Pas encore débutées, En cours, Terminées.","OK","Rejoué — options 0, 1 et 2."],
43:["Trois sous-statuts : Non planifié, Planifié, Plan validé.","OK","Rejoué — options de planification."],
44:["Après passage à En cours, l’escale apparaît dans Dispatch STS.","OK","Rejoué — changement puis GET /Dispatch/Sts."],
45:["Après passage à Terminées, l’escale quitte Actives et apparaît dans Terminées.","OK","Rejoué — bascule de volet."],
46:["L’escale terminée disparaît de la liste et des choix STS.","OK","Rejoué — occurrences STS de 3 à 0."],
47:["Supprimer demande une confirmation explicite incluant tous les registres.","OK","Corrigé et rejoué — confirm contrôlé dans le HTML."],
48:["Une escale avec anomalie enfant est supprimée ; sa fiche retourne HTTP 404.","OK","Corrigé et rejoué — cascade SQL effective."],
49:["GULF TRADER, non débuté avec ETA dépassée, reste visible dans STS.","OK","Rejoué — GET /Dispatch/Sts."],
50:["L’anomalie CASCADE0002 est ajoutée et restituée dans la fiche.","OK","Corrigé et rejoué après ajout des permissions de registres au Vessel Planner."]};
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

const wave5Updates={
106:["20 total et 15 désignés sont persistés et restitués après rechargement.","OK","Rejoué le 27/07/2026 — HTTP authentifié + HTML rendu."],
107:["Avec 20 au parc et 15 désignés, la valeur calculée Non désignés est 5.","OK","Rejoué le 27/07/2026 — persistance SQL TotalParc=20, Designes=15, calcul=5."],
108:["La raison de non-désignation est persistée et visible après rechargement.","OK","Rejoué le 27/07/2026 — HTTP authentifié + HTML rendu."],
111:["La répartition TT par navire est reprise dans le poste STS sans ressaisie.","OK","Rejoué le 27/07/2026 — preuve croisée STS-20."],
112:["La déconnexion affiche tracteur, départ, retour automatique, motif et durée.","OK","Rejoué le 27/07/2026 — HTTP + persistance, durée calculée."],
113:["Une déconnexion sans retour est signalée En cours.","OK","Rejoué le 27/07/2026 — HTTP authentifié + HTML rendu."],
114:["Après clôture, le retour et la durée sont conservés et En cours disparaît.","OK","Rejoué le 27/07/2026 — HTTP authentifié + persistance."],
115:["Le total 10 et les compteurs d'effectif sont persistés et affichés.","OK","Rejoué le 27/07/2026 — HTTP authentifié + HTML rendu."],
116:["La panne RTG et sa cause apparaissent après rechargement.","OK","Rejoué le 27/07/2026 — HTTP authentifié + persistance."],
117:["Une panne avec retrait fait passer Disponible de 10 à 9 et Retiré de 0 à 1.","OK","Corrigé et rejoué le 27/07/2026 — compteur dérivé des pannes actives."],
118:["La clôture restaure Disponible à 10 et Retiré à 0.","OK","Rejoué le 27/07/2026 — HTTP authentifié + HTML rendu."],
119:["La durée persistée entre début et fin est exactement 120 minutes.","OK","Rejoué le 27/07/2026 — preuve SQL DATEDIFF=120."],
120:["Le clash est persisté avec lieu, engins et description.","OK","Rejoué le 27/07/2026 — HTTP authentifié + HTML rendu."],
121:["La livraison GATE est persistée et visible dans le tableau.","OK","Rejoué le 27/07/2026 — HTTP authentifié + HTML rendu."],
122:["Livraison et Réception sont proposées et distinguées.","OK","Rejoué le 27/07/2026 — contrôle des options HTML."],
124:["Reach Stackers, Empty Handlers et autres sont persistés séparément.","OK","Rejoué le 27/07/2026 — valeurs 7, 5 et 3 restituées."],
125:["La panne d'engin est persistée avec sa cause et son retrait.","OK","Rejoué le 27/07/2026 — HTTP + preuve SQL."],
126:["Le retrait Reach Stacker est calculé et affiché séparément.","OK","Rejoué le 27/07/2026 — compteur retiré=1."],
127:["La déconnexion opérateur et son motif sont persistés.","OK","Rejoué le 27/07/2026 — HTTP authentifié + HTML rendu."],
129:["Panne d'engin et déconnexion opérateur restent deux registres et compteurs distincts.","OK","Rejoué le 27/07/2026 — contrôles croisés après rechargement."],
130:["Les lignes panne, déconnexion et remplacement sont restituées après une nouvelle navigation.","OK","Rejoué le 27/07/2026 — persistance après rechargement."]
};
for(const [row,values] of Object.entries(wave5Updates)){testSheet.getRange(`H${row}`).values=[[values[0]]];testSheet.getRange(`I${row}`).values=[[values[1]]];testSheet.getRange(`K${row}`).values=[[values[2]]];}
console.log((await workbook.inspect({kind:"table",sheetId:"Synthèse",range:"A1:F26",include:"values,formulas",tableMaxRows:26,tableMaxCols:6,maxChars:10000})).ndjson);
console.log((await workbook.inspect({kind:"table",sheetId:"Cahier de test",range:"A86:K105",include:"values,formulas",tableMaxRows:20,tableMaxCols:11,maxChars:30000})).ndjson);
console.log((await workbook.inspect({kind:"match",searchTerm:"#REF!|#DIV/0!|#VALUE!|#NAME\\?|#N/A",options:{useRegex:true,maxResults:300},summary:"final formula error scan"})).ndjson);
for (const sheetName of ["Pre-requis","Cahier de test","Synthèse"]) {
 const preview=await workbook.render({sheetName,autoCrop:"all",scale:0.8,format:"png"});
 await fs.writeFile(path.join(previewDir,`after-${sheetName.replaceAll(" ","-")}.png`),new Uint8Array(await preview.arrayBuffer()));
}
const outputPath=path.join(root,"outputs","recette-2026-07-24","EscaleReport_cahier_de_recette_execution_vague4_2026-07-24.xlsx");
const output=await SpreadsheetFile.exportXlsx(workbook); await output.save(outputPath); console.log(`OUTPUT=${outputPath}`);
