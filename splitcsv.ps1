# Configuration
$location = "C:\csvdrop\" # CSVs location
$rowsMax = 900; # how many rows per CSV?
 
# Get all CSV under current folder
$allCSVs = Get-ChildItem $location\* -include *.csv
 
# Read and split all of them
$allCSVs | ForEach-Object {
    Write-Host $_.Name;
    $content = Import-Csv $_.Name;
    $insertLocation = ($_.Name.Length - 4);
    for($i=1; $i -le $content.length ;$i+=$rowsMax){
    $newName = $_.Name.Insert($insertLocation, "splitted_"+$i)
    $content|select -first $i|select -last $rowsMax | convertto-csv -NoTypeInformation | % { $_ -replace '"', ""} | out-file $location\$newName -fo -en ascii
    }
}