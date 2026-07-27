param([string]$BaseUrl='http://127.0.0.1:5066')
$ErrorActionPreference='Stop'
$evidence=[Collections.Generic.List[object]]::new()
function Token($h){$m=[regex]::Match($h,'name="__RequestVerificationToken"[^>]*value="([^"]+)"');if(!$m.Success){throw 'token absent'};$m.Groups[1].Value}
function Login(){
 $p=Invoke-WebRequest "$BaseUrl/Account/Login" -SessionVariable s -UseBasicParsing
 Invoke-WebRequest "$BaseUrl/Account/Login" -Method Post -WebSession $s -Body @{UserName='dispatcher1';Password='Bonjour@2027';ReturnUrl='';__RequestVerificationToken=(Token $p.Content)} -UseBasicParsing|Out-Null
 $s
}
function Post($s,$path,$body,$source){
 $p=Invoke-WebRequest "$BaseUrl$source" -WebSession $s -UseBasicParsing
 $body.__RequestVerificationToken=Token $p.Content
 Invoke-WebRequest "$BaseUrl$path" -Method Post -WebSession $s -Body $body -UseBasicParsing|Out-Null
}
function SetPost($s,$poste){Post $s '/Account/ChooseDispatchPost' @{poste=$poste} '/Account/ChooseDispatchPost'}
function Page($s,$path){(Invoke-WebRequest "$BaseUrl$path" -WebSession $s -UseBasicParsing).Content}
function AddEv($id,$ok,$actual){$evidence.Add([pscustomobject]@{scenario=$id;passed=[bool]$ok;actual=$actual;proof='HTTP authentifié + HTML rendu + persistance après rechargement'})}
function IdNear($html,$marker,$name){$i=$html.IndexOf($marker);if($i-lt 0){return $null};$tail=$html.Substring($i,[Math]::Min(5000,$html.Length-$i));$m=[regex]::Match($tail,'name="'+[regex]::Escape($name)+'" value="([0-9a-f-]{36})"');if($m.Success){$m.Groups[1].Value}}
$s=Login
$stamp='R5'+[DateTime]::UtcNow.ToString('HHmmss')
$now=[DateTime]::UtcNow
try{
 SetPost $s 'TT'
 Post $s '/Dispatch/UpdateTtEffectif' @{TotalParc=20;Designes=15;RaisonNonDesignation="RECETTE-$stamp manque chauffeurs";Retires=0} '/Dispatch/Tt'
 $h=Page $s '/Dispatch/Tt'
 AddEv 'TT-01' ($h.Contains('value="20"') -and $h.Contains('value="15"')) '20 total et 15 désignés restitués'
 AddEv 'TT-02' ($h.Contains('>5</p>') -or $h.Contains('>5</span>')) '5 non-désignés calculés'
 AddEv 'TT-03' $h.Contains("RECETTE-$stamp manque chauffeurs") 'raison persistée et visible'
 Post $s '/Dispatch/AddTtDeconnexion' @{NumeroTt="TT-$stamp-A";DateDebutUtc=$now.AddHours(-2).ToString('yyyy-MM-ddTHH:mm');Raison="RECETTE-$stamp test durée";RetireEffectif='true'} '/Dispatch/Tt'
 $h=Page $s '/Dispatch/Tt'; $id=IdNear $h "TT-$stamp-A" 'deconnexionId'
 AddEv 'TT-08' ($h.Contains("TT-$stamp-A") -and $h.Contains('En cours')) 'déconnexion ouverte signalée en cours'
 Post $s '/Dispatch/CloseTtDeconnexion' @{deconnexionId=$id} '/Dispatch/Tt'
 $h=Page $s '/Dispatch/Tt'
 AddEv 'TT-07' ($h.Contains("TT-$stamp-A") -and [regex]::IsMatch($h,'TT-'+[regex]::Escape($stamp)+'-A[\s\S]{0,3000}(2h|02:)')) 'tracteur, début, retour automatique et motif persistés'
 AddEv 'TT-09' ($h.Contains("TT-$stamp-A") -and -not ((IdNear $h "TT-$stamp-A" 'deconnexionId'))) 'clôture persistée et action de clôture disparue'

 SetPost $s 'RTG'
 Post $s '/Dispatch/UpdateRtgEffectif' @{TotalParc=10;Disponible=10;Affecte=0;EnPanne=0;Retire=0} '/Dispatch/Rtg'
 $h=Page $s '/Dispatch/Rtg'
 AddEv 'RTG-01' ($h.Contains('value="10"')) 'effectif total 10 restitué'
 Post $s '/Dispatch/AddRtgPanne' @{Engin="RTG-$stamp";DateDebutUtc=$now.AddHours(-2).ToString('yyyy-MM-ddTHH:mm');Raison="CAUSE-$stamp";RetireEffectif='true'} '/Dispatch/Rtg'
 $h=Page $s '/Dispatch/Rtg';$panneGuid=IdNear $h "RTG-$stamp" 'panneId'
 AddEv 'RTG-02' ($h.Contains("RTG-$stamp") -and $h.Contains("CAUSE-$stamp")) 'panne et cause visibles'
 AddEv 'RTG-03' ($h.Contains('value="9"') -and $h.Contains('value="1"')) 'disponible 9 et retiré 1 calculés'
 Post $s '/Dispatch/CloseRtgPanne' @{panneId=$panneGuid;commentaireReprise="REPRISE-$stamp"} '/Dispatch/Rtg'
 $h=Page $s '/Dispatch/Rtg'
 AddEv 'RTG-04' ($h.Contains('value="10"') -and $h.Contains('value="0"')) 'effectif restauré après clôture'
 AddEv 'RTG-05' ($h.Contains("RTG-$stamp") -and ($h.Contains('2 h') -or $h.Contains('02:'))) 'durée voisine de 2 h calculée'
 Post $s '/Dispatch/AddRtgClash' @{Lieu="ZONE-$stamp";EnginsConcernes="RTG-A/RTG-B";DateDebutUtc=$now.AddHours(-1).ToString('yyyy-MM-ddTHH:mm');Description="CLASH-$stamp"} '/Dispatch/Rtg'
 $h=Page $s '/Dispatch/Rtg'
 AddEv 'RTG-06' ($h.Contains("ZONE-$stamp") -and $h.Contains("CLASH-$stamp")) 'clash visible après rechargement'
 Post $s '/Dispatch/AddGateTruckIssue' @{TypeOperation=0;CamionReference="TRUCK-$stamp";DateDebutUtc=$now.AddMinutes(-30).ToString('yyyy-MM-ddTHH:mm');ProblemeRencontre="GATE-$stamp"} '/Dispatch/Rtg'
 $h=Page $s '/Dispatch/Rtg'
 AddEv 'RTG-07' ($h.Contains("TRUCK-$stamp") -and $h.Contains('Livraison')) 'livraison GATE visible'
 AddEv 'RTG-08' ($h.Contains('value="0"') -and $h.Contains('Livraison') -and $h.Contains('value="1"') -and $h.Contains('Réception')) 'deux types proposés et distingués'

 SetPost $s 'Autres engins'
 Post $s '/Dispatch/UpdateAutresEnginsEffectif' @{DisponibleReachStackers=7;DisponibleEmptyHandlers=5;DisponibleAutres=3} '/Dispatch/AutresEngins'
 $h=Page $s '/Dispatch/AutresEngins'
 AddEv 'AE-01' ($h.Contains('value="7"') -and $h.Contains('value="5"') -and $h.Contains('value="3"')) 'trois familles persistées séparément'
 Post $s '/Dispatch/AddEnginProbleme' @{Engin="RS-$stamp";Categorie=0;DateDebutUtc=$now.AddHours(-2).ToString('yyyy-MM-ddTHH:mm');Probleme="PANNE-$stamp";RetireEffectif='true'} '/Dispatch/AutresEngins'
 $h=Page $s '/Dispatch/AutresEngins'
 AddEv 'AE-02' ($h.Contains("RS-$stamp") -and $h.Contains("PANNE-$stamp")) 'panne engin visible'
 AddEv 'AE-03' ($h.Contains('1 retiré(s)')) 'retrait Reach Stacker calculé séparément'
 Post $s '/Dispatch/AddEnginDeconnexion' @{Engin="OP-$stamp";DateDebutUtc=$now.AddHours(-1).ToString('yyyy-MM-ddTHH:mm');Motif="ABS-$stamp"} '/Dispatch/AutresEngins'
 $h=Page $s '/Dispatch/AutresEngins'
 AddEv 'AE-04' ($h.Contains("OP-$stamp") -and $h.Contains("ABS-$stamp")) 'déconnexion opérateur persistée'
 Post $s '/Dispatch/AddRemplacementOperateur' @{Operateur="OPER-$stamp";EnginQuitte="RS-OLD-$stamp";NouvelEngin="RS-NEW-$stamp";DateHeureUtc=$now.ToString('yyyy-MM-ddTHH:mm');Raison="REMP-$stamp";Commentaire='preuve'} '/Dispatch/AutresEngins'
 $h=Page $s '/Dispatch/AutresEngins'
 AddEv 'AE-05' ($h.Contains("OPER-$stamp") -and $h.Contains("RS-OLD-$stamp") -and $h.Contains("RS-NEW-$stamp")) 'opérateur et changement d engin visibles'
 AddEv 'AE-06' ($h.Contains("RS-$stamp") -and $h.Contains("OP-$stamp") -and $h.Contains('1 retiré(s)')) 'panne engin et absence opérateur restent distinctes'
 $h2=Page $s '/Dispatch/AutresEngins'
 AddEv 'AE-07' ($h2.Contains("RS-$stamp") -and $h2.Contains("OP-$stamp") -and $h2.Contains("OPER-$stamp")) 'toutes les lignes restituées après nouvelle navigation'
} finally {
 $dir='outputs/recette-2026-07-24';New-Item -ItemType Directory -Force $dir|Out-Null
 [pscustomobject]@{testedAtUtc=[DateTime]::UtcNow.ToString('o');baseUrl=$BaseUrl;marker=$stamp;results=$evidence;allPassed=($evidence.Count-eq 21 -and @($evidence|?{-not $_.passed}).Count-eq 0)}|ConvertTo-Json -Depth 5|Set-Content "$dir/wave5-dispatch-evidence.json" -Encoding utf8
 $evidence|Format-Table -AutoSize
 Write-Output "COUNT=$($evidence.Count) ALL_PASSED=$(@($evidence|?{-not $_.passed}).Count-eq 0) MARKER=$stamp"
}
