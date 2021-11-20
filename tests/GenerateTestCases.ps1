
$path = "C:\Users\mikec\RiderProjects\NetworkPrimitives\tests\randomips.csv"
$out = "C:\Users\mikec\RiderProjects\NetworkPrimitives\tests\randomips.json"
Import-Csv $path | Foreach-Object {
    [PSCustomObject]@{
        'IpString' = $_.'IP String'
        'Ip' = $_.IP
        'Cidr' = $_.Cidr
        'Mask' = $_.'Mask (Hex)'
        'MaskString' = $_.'Mask (IP)'
        'TotalHosts' = $_.'# Num Hosts'
        'UsableHosts' = $_.'# Usable'
        'Network' = $_.'Network Address'
        'FirstUsable' = $_.'First Usable'
        'LastUsable' = $_.'Last Usable'
        'Broadcast' = $_.Broadcast
    }
} | ConvertTo-Json | Set-Content $out