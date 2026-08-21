#Fetch Instance Id
$InstanceId = Get-EC2InstanceMetadata -Category InstanceId
#Start IIS Service
Send-SSMCommand -DocumentName "AWS-RunPowerShellScript" -Parameter @{commands = "Start-Website -Name PZPFC.SE.COM"} -Target @{Key="instanceids";Values=@($InstanceId)}
