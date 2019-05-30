dotnet run --project .\XamarinSecurityScanner\XamarinSecurityScanner.App --path .\VulnerableApps\BankingApp

if($?) {
    # Continue the build...
    echo "Success!"
} else {
    # Break the build...
    echo "Failed!"
}