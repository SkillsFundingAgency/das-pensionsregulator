# Input bindings are passed in via param block.
param($Timer)

# The 'IsPastDue' property is 'true' when the current function invocation is later than scheduled.
if ($Timer.IsPastDue) {
    Write-Host "PowerShell timer is running late!"
}

Write-Host "Connect using Managed Identity at: $((Get-Date).ToUniversalTime())"
Connect-AzAccount -Identity

if ($env:DataShareSubscriptionName -ne "") {
    $shareSubscription = @{
        ResourceGroupName     = $env:DataShareResourceGroupName
        AccountName           = $env:DataShareName
        ShareSubscriptionName = $env:DataShareSubscriptionName
    }

    $syncRequestedAt = (Get-Date).ToUniversalTime()
    Write-Host "Initiate sync at: $syncRequestedAt"

    try {
        Start-AzDataShareSubscriptionSynchronization @shareSubscription -SynchronizationMode $env:DataShareSubscriptionSynchronizationMode -ErrorAction Stop
    }
    catch {
        $isLostOperation = $_.Exception.GetType().FullName -eq 'Microsoft.Rest.Azure.CloudException' -and
                           $_.Exception.Response.StatusCode -eq [System.Net.HttpStatusCode]::NotFound
        if (-not $isLostOperation) {
            throw
        }

        Write-Warning "Lost track of sync operation, checking synchronization history instead: $($_.Exception.Message)"

        $timeoutAt = (Get-Date).ToUniversalTime().AddMinutes(20)
        do {
            $sync = Get-AzDataShareSubscriptionSynchronization @shareSubscription -ErrorAction Stop |
                Where-Object { $_.StartTime -and $_.StartTime.ToUniversalTime() -ge $syncRequestedAt.AddMinutes(-1) } |
                Sort-Object StartTime -Descending |
                Select-Object -First 1

            if ($sync -and $sync.Status -notin @('Queued', 'InProgress')) {
                break
            }

            if ((Get-Date).ToUniversalTime() -gt $timeoutAt) {
                throw "Timed out waiting for sync to complete. Last status: $(if ($sync) { $sync.Status } else { 'not found' })"
            }

            Start-Sleep -Seconds 30
        } while ($true)

        Write-Host "Sync $($sync.SynchronizationId) finished with status: $($sync.Status)"
        if ($sync.Status -ne 'Succeeded') {
            throw "Sync $($sync.SynchronizationId) did not succeed. Status: $($sync.Status). Message: $($sync.Message)"
        }
    }

    Write-Host "Sync finished at: $((Get-Date).ToUniversalTime())"
} else {
    throw "No data share subscription name defined: $((Get-Date).ToUniversalTime())"
}
