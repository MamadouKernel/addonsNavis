$ErrorActionPreference = 'Stop'

$path = (Resolve-Path -LiteralPath '.\scripts\run-wave4-sts-tests.ps1').Path
$content = [IO.File]::ReadAllText($path)

# PowerShell traite U+2019 comme un délimiteur de chaîne dans certains contextes.
# Deux apostrophes ASCII représentent une apostrophe littérale dans une chaîne simple.
$content = $content.Replace([string][char]0x2019, "''")
$content = $content.Replace([string][char]0x2018, "''")

[IO.File]::WriteAllText($path, $content, [Text.UTF8Encoding]::new($false))
Write-Output 'Guillemets typographiques du harnais normalisés.'
