# Deployment URLs Configuration Guide

## 🌐 URL Configuration for Different Deployment Platforms

### Azure App Service (Recommended)

#### API URLs:
- **Development**: `http://localhost:5000/`
- **Production**: `https://bernard-portfolio-api.azurewebsites.net/`

#### Web URLs:
- **Development**: `http://localhost:5001/`
- **Production**: `https://bernard-portfolio-web.azurewebsites.net/`

### Alternative Hosting Platforms

#### Vercel (Web App)
- **URL**: `https://bernard-lusale-portfolio.vercel.app/`
- **API**: Still use Azure for API (Vercel is for static sites)

#### Netlify (Web App)
- **URL**: `https://bernard-lusale-portfolio.netlify.app/`
- **API**: Still use Azure for API

#### Railway (Full Stack)
- **API**: `https://bernard-portfolio-api.railway.app/`
- **Web**: `https://bernard-portfolio-web.railway.app/`

#### Heroku (Full Stack)
- **API**: `https://bernard-portfolio-api.herokuapp.com/`
- **Web**: `https://bernard-portfolio-web.herokuapp.com/`

## 🔧 Configuration Files

### 1. Web App Configuration (`src/Portfolio.Web/wwwroot/appsettings.json`)

**Development:**
```json
{
  "ApiBaseUrl": "http://localhost:5000/"
}
```

**Production:**
```json
{
  "ApiBaseUrl": "https://bernard-portfolio-api.azurewebsites.net/"
}
```

### 2. API CORS Configuration (`src/Portfolio.Api/appsettings.Production.json`)

```json
{
  "AllowedOrigins": [
    "https://bernard-portfolio-web.azurewebsites.net",
    "https://www.bernard-lusale.com",
    "https://bernard-lusale.com",
    "https://bernard-lusale-portfolio.vercel.app",
    "https://bernard-lusale-portfolio.netlify.app"
  ]
}
```

## 🚀 Platform-Specific Deployment Commands

### Azure App Service

```bash
# Create resource group
az group create --name bernard-portfolio-rg --location "East US"

# Create App Service plan
az appservice plan create --name bernard-portfolio-plan --resource-group bernard-portfolio-rg --sku B1 --is-linux

# Create API web app
az webapp create --resource-group bernard-portfolio-rg --plan bernard-portfolio-plan --name bernard-portfolio-api --runtime "DOTNETCORE:9.0"

# Create Web app (Static Web App)
az staticwebapp create --name bernard-portfolio-web --resource-group bernard-portfolio-rg --source https://github.com/YOUR_USERNAME/bernard-lusale-portfolio --branch main --app-location "src/Portfolio.Web" --output-location "wwwroot"
```

### Vercel (Web Only)

```bash
# Install Vercel CLI
npm i -g vercel

# Deploy from project root
vercel --prod

# Configure build settings:
# Build Command: dotnet publish src/Portfolio.Web/Portfolio.Web.csproj -c Release -o publish
# Output Directory: publish/wwwroot
```

### Netlify (Web Only)

```bash
# Install Netlify CLI
npm install -g netlify-cli

# Build and deploy
dotnet publish src/Portfolio.Web/Portfolio.Web.csproj -c Release -o publish
netlify deploy --prod --dir=publish/wwwroot
```

## 🔄 Environment-Specific URLs

### Development
- **Web**: `http://localhost:5001`
- **API**: `http://localhost:5000`
- **Database**: In-Memory

### Staging (Optional)
- **Web**: `https://bernard-portfolio-web-staging.azurewebsites.net`
- **API**: `https://bernard-portfolio-api-staging.azurewebsites.net`
- **Database**: Azure SQL (Staging)

### Production
- **Web**: `https://bernard-portfolio-web.azurewebsites.net`
- **API**: `https://bernard-portfolio-api.azurewebsites.net`
- **Database**: Azure SQL (Production)

## 🎯 Custom Domain Setup (Optional)

If you want to use a custom domain like `bernard-lusale.com`:

### 1. Purchase Domain
- GoDaddy, Namecheap, or Google Domains

### 2. Configure DNS
```
A Record: @ -> Your Azure App IP
CNAME: www -> bernard-portfolio-web.azurewebsites.net
CNAME: api -> bernard-portfolio-api.azurewebsites.net
```

### 3. Update Configuration
```json
{
  "ApiBaseUrl": "https://api.bernard-lusale.com/",
  "AllowedOrigins": [
    "https://bernard-lusale.com",
    "https://www.bernard-lusale.com"
  ]
}
```

## 🔒 SSL/HTTPS Configuration

All production URLs should use HTTPS:
- Azure App Service provides free SSL certificates
- Vercel/Netlify provide automatic HTTPS
- Custom domains need SSL certificate setup

## 📊 URL Testing Checklist

Before going live, test these URLs:

### API Endpoints:
- ✅ `GET /api/projects` - Returns projects list
- ✅ `POST /api/contact` - Accepts contact form
- ✅ `GET /api/skills` - Returns skills list

### Web Pages:
- ✅ `/` - Home page loads
- ✅ `/about` - About page loads
- ✅ `/contact` - Contact form works
- ✅ `/projects` - Projects page loads

### Cross-Origin Requests:
- ✅ Web app can call API endpoints
- ✅ Contact form submits successfully
- ✅ No CORS errors in browser console

Your URLs are now properly configured for deployment! 🎉