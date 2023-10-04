# Input bindings are passed in via param block.
param($Timer)

# The 'IsPastDue' property is 'true' when the current function invocation is later than scheduled.
if ($Timer.IsPastDue) {
    Write-Host "PowerShell timer is running late!"
}

Write-Host "Connect using Managed Identity at: $((Get-Date).ToUniversalTime())"
Connect-AzAccount -Identity

if ($env:DataShareSubscriptionName -ne "") {
    Write-Host "Initiate sync at: $((Get-Date).ToUniversalTime())"
    Start-AzDataShareSubscriptionSynchronization -ResourceGroupName $env:DataShareResourceGroupName -AccountName $env:DataShareName -ShareSubscriptionName $env:DataShareSubscriptionName -SynchronizationMode $env:DataShareSubscriptionSynchronizationMode -ErrorAction Stop
    Write-Host "Sync finished at: $((Get-Date).ToUniversalTime())"
} else {
    throw "No data share subscription name defined: $((Get-Date).ToUniversalTime())"
}
