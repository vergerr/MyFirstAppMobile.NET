$date = Get-Date -Format "yyyyMMdd"
$exportFile = "Export$date.txt"

if (Test-Path $exportFile) { Remove-Item $exportFile }

# On récupère les fichiers avec \* pour que -Include fonctionne
Get-ChildItem -Path ".\*" -Recurse -Include *.cs, *.xaml | Where-Object { 
    # Filtre 1 : Exclure le dossier 'obj'
    $_.FullName -notmatch '\\obj\\' -and 
    $_.FullName -notmatch '\\Resources\\' -and 
    # Filtre 2 : Exclure le fichier d'export lui-même pour éviter une boucle infinie
    $_.Name -ne $exportFile 
} | ForEach-Object {

    Write-Host "Traitement de : $($_.Name)" # Optionnel : pour voir l'avancement

    Add-Content -Path $exportFile -Value "===== $($_.FullName) ====="
    Add-Content -Path $exportFile -Value ""

    # Lecture et ajout du contenu
    Get-Content $_.FullName | Add-Content -Path $exportFile

    # Ajout de lignes vides pour la séparation
    Add-Content -Path $exportFile -Value "`n`n"
}

Write-Host "Export terminé : $exportFile" -ForegroundColor Green