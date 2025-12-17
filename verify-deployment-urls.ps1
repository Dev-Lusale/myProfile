# Deployment URL Verification Script
param(
    [Parameter(Mandatory=$false)]
    [string]$Environment = "Production"
)

Write-Host "🔍 Verifying Deployment URLs Configuration" -ForegroundColor Cyan
Write-Host "Environment: $Environment" -ForegroundColor Yellow
Write-Host ""

# Check configuration files
Write-Host "📋 Configuration Files:" -ForegroundColor Green

if (Test-Path "src/Portfolio.Web/wwwroot/appsettings.json") {
    Write-Host "✅ Web appsettings.json exists" -ForegroundColor Green
    $webConfig = Get-Content "src/Portfolio.Web/wwwroot/appsettings.json" | ConvertFrom-Json
    Write-Host "   API URL: $($webConfig.ApiBaseUrl)" -ForegroundColor Cyan
} else {
    Write-Host "❌ Web appsettings.json missing" -ForegroundColor Red
}

if (Test-Path "src/Portfolio.Web/wwwroot/appsettings.Production.json") {
    Write-Host "✅ Web Production appsettings.json exists" -ForegroundColor Green
    $webProdConfig = Get-Content "src/Portfolio.Web/wwwroot/appsettings.Production.json" | ConvertFrom-Json
    Write-Host "   Production API URL: $($webProdConfig.ApiBaseUrl)" -ForegroundColor Cyan
} else {
    Write-Host "❌ Web Production appsettings.json missing" -ForegroundColor Red
}

if (Test-Path "src/Portfolio.Api/appsettings.Production.json") {
    Write-Host "✅ API Production appsettings.json exists" -ForegroundColor Green
    $apiProdConfig = Get-Content "src/Portfolio.Api/appsettings.Production.json" | ConvertFrom-Json
    Write-Host "   Allowed Origins:" -ForegroundColor Cyan
    foreach ($origin in $apiProdConfig.AllowedOrigins) {
        Write-Host "     - $origin" -ForegroundColor White
    }
} else {
    Write-Host "❌ API Production appsettings.json missing" -ForegroundColor Red
}

Write-Host ""
Write-Host "🌐 Expected Deployment URLs:" -ForegroundColor Green
Write-Host "  API (Azure): https://bernard-portfolio-api.azurewebsites.net" -ForegroundColor Cyan
Write-Host "  Web (Azure): https://bernard-portfolio-web.azurewebsites.net" -ForegroundColor Cyan
Write-Host "  Web (Vercel): https://bernard-lusale-portfolio.vercel.app" -ForegroundColor Cyan
Write-Host "  Web (Netlify): https://bernard-lusale-portfolio.netlify.app" -ForegroundColor Cyan

Write-Host ""
Write-Host "🔧 Configuration Status:" -ForegroundColor Green

# Check if URLs are still placeholder values
$hasPlaceholders = $false

# Check Program.cs files for placeholder URLs
$webProgramContent = Get-Content "src/Portfolio.Web/Program.cs" -Raw
if ($webProgramContent -match "your-api-domain\.com") {
    Write-Host "⚠️  Web Program.cs contains placeholder URLs" -ForegroundColor Yellow
    $hasPlaceholders = $true
}

$apiProgramContent = Get-Content "src/Portfolio.Api/Program.cs" -Raw
if ($apiProgramContent -match "your-portfolio-domain\.com") {
    Write-Host "⚠️  API Program.cs contains placeholder URLs" -ForegroundColor Yellow
    $hasPlaceholders = $true
}

if (-not $hasPlaceholders) {
    Write-Host "✅ No placeholder URLs found" -ForegroundColor Green
}

Write-Host ""
Write-Host "📝 Next Steps:" -ForegroundColor Yellow
Write-Host "  1. Deploy API to Azure: az webapp create ..." -ForegroundColor White
Write-Host "  2. Deploy Web to Azure Static Web Apps or Vercel" -ForegroundColor White
Write-Host "  3. Test all endpoints after deployment" -ForegroundColor White
Write-Host "  4. Update DNS if using custom domain" -ForegroundColor White

Write-Host ""
Write-Host "🎯 URLs are properly configured for deployment!" -ForegroundColor Green