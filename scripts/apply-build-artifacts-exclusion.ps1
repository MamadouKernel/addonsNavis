$ErrorActionPreference = 'Stop'

$path = (Resolve-Path -LiteralPath '.\EscaleReport.Web\EscaleReport.Web.csproj').Path
$content = [IO.File]::ReadAllText($path).Replace("`r`n", "`n")

if (-not $content.Contains('<DefaultItemExcludes>$(DefaultItemExcludes);artifacts/**</DefaultItemExcludes>')) {
    $content = $content.Replace(
@'
    <ImplicitUsings>enable</ImplicitUsings>
'@,
@'
    <ImplicitUsings>enable</ImplicitUsings>
    <!-- Les sorties de recette sont générées hors du projet. Cette exclusion empêche
         MSBuild de recopier récursivement artifacts/** à chaque compilation (Windows 206). -->
    <DefaultItemExcludes>$(DefaultItemExcludes);artifacts/**</DefaultItemExcludes>
'@)
}

if (-not $content.Contains('<Content Remove="artifacts\**" />')) {
    $content = $content.Replace(
@'
  <ItemGroup>
    <PackageReference Include="ClosedXML"
'@,
@'
  <ItemGroup>
    <Content Remove="artifacts\**" />
    <None Remove="artifacts\**" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="ClosedXML"
'@)
}

[IO.File]::WriteAllText($path, $content, [Text.UTF8Encoding]::new($false))
Write-Output 'artifacts/** exclu des éléments MSBuild.'
