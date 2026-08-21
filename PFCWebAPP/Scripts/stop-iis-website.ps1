#Fetch Instance Id
$InstanceId = Get-EC2InstanceMetadata -Category InstanceId
#Stop IIS Service
Send-SSMCommand -DocumentName "AWS-RunPowerShellScript" -Parameter @{commands = "Stop-Website -Name PZPFC.SE.COM"} -Target @{Key="instanceids";Values=@($InstanceId)}

# Take backup of the website files exclude log folder
$sourcepath = "C:\inetpub\wwwroot\PZPFC.SE.COM"
# construct backup path
$DateTime = (Get-Date -Format "yyyyMMddHHmmss")
$destinationpath = Join-Path "D:\Backup\PZPFC.SE.COM\" "PZPFC.SE.COM-$DateTime.zip"
# exclusion rules. Can use wild cards (*)
$exclude = @("logs","*.zip")

$files = Get-ChildItem -Path $sourcepath -Exclude $exclude
# compress
Compress-Archive -Path $files -DestinationPath $destinationpath -CompressionLevel Fastest
