$version = "v1.2"

$url = "https://github.com/Perlten/Wtile/releases/download/$version/wtile.exe"
$location = "$env:USERPROFILE\AppData\Local\wtile\wtile.exe"

New-Item -ItemType Directory -Force -Path "$env:USERPROFILE\AppData\Local\wtile\"

Invoke-WebRequest -Uri $url -OutFile $location

$SourceExe = $location
$DestinationPath = "$env:USERPROFILE\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\wtile.lnk"

#Create Shortcut
$WshShell = New-Object -comObject WScript.Shell
$Shortcut = $WshShell.CreateShortcut($DestinationPath)
$Shortcut.TargetPath = $SourceExe
$Shortcut.Save()


$SourceExe = $location
$DestinationPath = "$env:USERPROFILE\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\wtile.lnk"

$WshShell = New-Object -comObject WScript.Shell
$Shortcut = $WshShell.CreateShortcut($DestinationPath)
$Shortcut.TargetPath = $SourceExe
$Shortcut.Save()