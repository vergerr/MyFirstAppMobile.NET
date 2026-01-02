# Date au format AAAAMMJJ
$date = Get-Date -Format "yyyyMMdd"

# Nom du fichier d'export
$exportFile = "Export$date.txt"

# Supprime le fichier s'il existe déjà
if (Test-Path $exportFile) {
    Remove-Item $exportFile
}

# Récupération de tous les fichiers .cs récursivement
Get-ChildItem -Path . -Recurse -Filter "*.cs" | ForEach-Object {

    # Entête pour identifier le fichier
    Add-Content -Path $exportFile -Value "===== $($_.FullName) ====="
    Add-Content -Path $exportFile -Value ""

    # Contenu du fichier
    Get-Content $_.FullName | Add-Content -Path $exportFile

    # Séparation entre fichiers
    Add-Content -Path $exportFile -Value ""
    Add-Content -Path $exportFile -Value ""
}

Write-Host "Export terminé : $exportFile"