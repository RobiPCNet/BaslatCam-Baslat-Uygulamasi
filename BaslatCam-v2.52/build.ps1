param([string]$OutputDirectory='bin\Portable')
$ErrorActionPreference='Stop'
$fx=Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319'
$csc=Join-Path $fx 'csc.exe'
if(!(Test-Path -LiteralPath $csc)){throw 'Windows .NET Framework 4.x compiler bulunamadı.'}
Push-Location $PSScriptRoot
try {
 New-Item -ItemType Directory -Path $OutputDirectory -Force | Out-Null
 $refs=@('/r:System.dll','/r:System.Core.dll','/r:System.Web.Extensions.dll','/r:System.Drawing.dll','/r:System.Windows.Forms.dll',"/r:$fx\WPF\WindowsBase.dll","/r:$fx\WPF\PresentationCore.dll","/r:$fx\WPF\PresentationFramework.dll","/r:$fx\System.Xaml.dll")
 $device=Join-Path $env:WINDIR 'Microsoft.NET\assembly\GAC_MSIL\System.Device\v4.0_4.0.0.0__b77a5c561934e089\System.Device.dll'
 $refs+=('/r:'+$device)
 $assets=@(Get-ChildItem 'Assets\Weather\gunduz','Assets\Weather\gece','Assets\Weather\efektler','Assets\Weather\ekler' -File | ForEach-Object { '/resource:'+$_.FullName+',BaslatCam.Weather.'+$_.Directory.Name+'.'+$_.Name })
 & $csc /nologo /target:winexe "/out:$OutputDirectory\BaslatCam-v2.52.exe" /win32icon:BaslatCam.ico /win32manifest:app.manifest @refs @assets BaslatCam.cs WeatherMotion.cs Weather252.cs Verify252.cs MusicPlayer.cs Features246.cs Theme247.cs Atmosphere249.cs Controls250.cs Verify250.cs Verify251.cs Verify249.cs Verify247.cs Verify248.cs Verify246.cs Startup241.cs Dashboard242.cs Media242.cs Verify242.cs AudioWave244.cs CompactMedia244.cs Verify244.cs
 if($LASTEXITCODE -ne 0){throw 'Derleme başarısız.'}
} finally {Pop-Location}





