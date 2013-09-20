function Compare-Sln(
    [string]$fileA,
    [string]$fileB
)
{
    pushd .\Laan.SolutionConverter\bin\Debug

    $temp = (get-env TEMP).Trim("\")

    $fileNameA = [System.IO.Path]::GetFileName($fileA)
    $fileNameB = [System.IO.Path]::GetFileName($fileB)

    cp $fileA "$temp\$fileNameA"
    cp $fileB "$temp\$fileNameB"

    .\Laan.SolutionConverter.exe -i "$temp\$fileNameA" -m Xml -o "$temp\$fileNameA.xml"
    .\Laan.SolutionConverter.exe -i "$temp\$fileNameB" -m Xml -o "$temp\$fileNameB.xml"

    Start-Process `
        'C:\Program Files (x86)\Beyond Compare 3\BComp.exe' `
        -ArgumentList "$temp\$fileNameA.xml", "$temp\$fileNameB.xml", "/title1=$fileA", "/title2=$fileB" `
        -Wait

    popd
}