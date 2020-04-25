param(
    [Parameter(Mandatory=$true)]
    [string]$file
)

$tmp = "tmp.txt";

certutil.exe -encode $file $tmp | Out-Null

$txt = [System.Io.File]::ReadAllLines($tmp);

$text = ""
foreach($line in $txt)
{
  if(!$line.StartsWith("-----"))
  {
    $text += $line
  }
}

$text | Set-Clipboard

Remove-Item $tmp