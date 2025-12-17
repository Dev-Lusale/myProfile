# Deploy API to Railway - Quick Guide

## 🚂 Railway Deployment (Easiest Option)

Railway is perfect for .NET APIs and much simpler than Azure.

### Step 1: Prepare for Railway

1. **Go to [Railway.app](https://railway.app)**
2. **Sign up with GitHub** (connects to your repository)
3. **Click "New Project"**
4. **Select "Deploy from GitHub repo"**
5. **Choose your portfolio repository**

### Step 2: Configure Railway Deployment

#### Create Railway Configuration:
Create `railway.toml` in your project root:

```toml
[build]
builder = "nixpacks"
buildCommand = "dotnet publish src/Portfolio.Api/Portfolio.Api.csproj -c Release -o publish"

[deploy]
startCommand = "dotnet publish/Portfolio.Api.dll"
restartPolicyType = "ON_FAILURE"
restartPolicyMaxRetries = 10

[env]
ASPNETCORE_ENVIRONMENT = "Production"
ASPNETCORE_URLS = "http://0.0.0.0:$PORT"
```

### Step 3: Set Environment Variables in Railway

In Railway dashboard, go to Variables tab and add:

```bash
# Email Configuration
EmailSettings__SmtpServer=smtp.gmail.com
EmailSettings__SmtpPort=587
EmailSettings__SmtpUsername=bernardlusale20@gmail.com
EmailSettings__SmtpPassword=hmciydyldqnaxvec
EmailSettings__FromEmail=bernardlusale20@gmail.com
EmailSettings__FromName=Bernard Lusale Portfolio
EmailSettings__ToEmail=bernardlusale20@gmail.com
EmailSettings__EnableSsl=true

# JWT Configuration
JwtSettings__SecretKey=YourProductionSecretKeyThatIsAtLeast32CharactersLongAndVerySecure!
JwtSettings__Issuer=PortfolioApi
JwtSettings__Audience=PortfolioClient
JwtSettings__ExpirationInMinutes=60

# CORS Configuration
AllowedOrigins__0=https://my-profile-three-theta.vercel.app
AllowedOrigins__1=https://bernard-lusale.com
AllowedOrigins__2=https://www.bernard-lusale.com

# Database (Railway provides PostgreSQL)
ConnectionStrings__DefaultConnection=${{Postgres.DATABASE_URL}}
```

### Step 4: Add PostgreSQL Database

1. In Railway dashboard, click **"+ New"**
2. Select **"Database"** → **"PostgreSQL"**
3. Railway will automatically create `DATABASE_URL` variable

### Step 5: Update Your Code for PostgreSQL

Install PostgreSQL package:
```bash
dotnet add src/Portfolio.Api package Npgsql.EntityFrameworkCore.PostgreSQL
```

Update `Program.cs`:
```csharp
// Replace SQL Server with PostgreSQL
if (builder.Environment.IsDevelopment())
{
    options.UseInMemoryDatabase("PortfolioDb");
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
}
```

### Step 6: Deploy

1. **Push your changes to GitHub**:
   ```bash
   git add .
   git commit -m "Add Railway configuration"
   git push
   ```

2. **Railway will automatically deploy** when you push to main branch

3. **Your API will be available at**: `https://your-project-name.railway.app`

### Step 7: Update Vercel with API URL

Once Railway gives you the API URL, update:

**File**: `src/Portfolio.Web/wwwroot/appsettings.Production.json`
```json
{
  "ApiBaseUrl": "https://your-project-name.railway.app/"
}
```

Then redeploy Vercel:
```bash
vercel --prod
```

## 🎯 Alternative: Quick Heroku Deployment

If you prefer Heroku:

```bash
# Install Heroku CLI
npm install -g heroku

# Login and create app
heroku login
heroku create bernard-portfolio-api

# Add PostgreSQL
heroku addons:create heroku-postgresql:mini

# Set environment variables
heroku config:set EmailSettings__SmtpUsername=bernardlusale20@gmail.com
heroku config:set EmailSettings__SmtpPassword=hmciydyldqnaxvec
# ... (add all other variables)

# Deploy
git subtree push --prefix src/Portfolio.Api heroku main
```

## ✅ After API Deployment

1. **Test API**: Visit `https://your-api-url.com/api/projects`
2. **Test Contact Form**: Submit a message on your Vercel site
3. **Check Email**: You should receive the notification email
4. **Celebrate**: Your portfolio is fully functional! 🎉

Railway is the easiest option - it handles everything automatically and gives you a PostgreSQL database for free!