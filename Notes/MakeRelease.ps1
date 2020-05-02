pushd "..\PatchMaker.App\bin\Release"

$file = Get-ChildItem "PatchMaker.dll"
$version = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($file).FileVersion
$idx = $version.LastIndexOf('.')
$version = $version.Substring(0,$idx)

Remove-Item "Newtonsoft.Json.dll"
Remove-Item "Sitecore.Kernel.dll"
Remove-Item "PatchMaker.App.exe.config"

Compress-Archive -Path "*.*" -CompressionLevel "Optimal" -DestinationPath "PatchMaker-v$($version).zip"

popd