# Professional Portfolio Platform

A comprehensive, data-driven portfolio platform built with Blazor WebAssembly, .NET MAUI Blazor Hybrid, and ASP.NET Core Web API. This platform serves as a living portfolio system for Software Engineers with real-time analytics, visitor tracking, and engagement metrics.

## 🏗️ Architecture

### Frontend
- **Blazor WebAssembly** - Public web portfolio
- **.NET MAUI Blazor Hybrid** - Cross-platform mobile & desktop app
- **Shared Razor Components** - Reusable UI components

### Backend
- **ASP.NET Core Web API** - RESTful endpoints
- **Entity Framework Core** - Data access layer
- **PostgreSQL/SQL Server** - Database options

## 🚀 Features

### Core Pages
1. **Landing/Hero** - Professional introduction with live metrics
2. **About Me** - Biography, education, certifications
3. **Skills & Tech Stack** - Categorized skills with proficiency levels
4. **Projects & Track Record** - Project showcase with analytics
5. **Experience & Achievements** - Timeline-based career history
6. **Analytics Dashboard** - Admin-only metrics and trends
7. **Contact & Interest Capture** - Lead generation and contact forms

### Analytics & Tracking
- Real-time visitor metrics
- Session duration tracking
- Project view analytics
- Interest heatmaps
- Event tracking (CV downloads, clicks, form submissions)
- Trending projects based on engagement

### Admin Features
- JWT authentication
- Content management (CRUD operations)
- Analytics dashboard with charts
- Lead management
- Real-time metrics

## 🛠️ Technology Stack

- **.NET 9**
- **Blazor WebAssembly & Server**
- **.NET MAUI 9**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **PostgreSQL/SQL Server**
- **Bootstrap 5**
- **Chart.js/LiveCharts**
- **JWT Authentication**

## 📁 Project Structure

```
Portfolio/
├── src/
│   ├── Portfolio.Shared/          # Shared models, DTOs, components
│   │   ├── Models/               # Data models
│   │   ├── DTOs/                 # Data transfer objects
│   │   └── Components/           # Shared Razor components
│   ├── Portfolio.Api/            # ASP.NET Core Web API
│   │   ├── Controllers/          # API controllers
│   │   ├── Services/             # Business logic
│   │   └── Data/                 # Database context
│   ├── Portfolio.Web/            # Blazor WebAssembly
│   │   ├── Pages/                # Razor pages
│   │   ├── Shared/               # Layout components
│   │   └── Services/             # Client services
│   └── Portfolio.MauiBlazor/     # .NET MAUI Blazor Hybrid
│       ├── Components/           # Blazor components
│       ├── Platforms/            # Platform-specific code
│       └── Resources/            # App resources
└── Portfolio.sln                 # Solution file
```

## 🚦 Getting Started

### Prerequisites
- .NET 9 SDK
- Visual Studio 2022 or VS Code
- SQL Server or PostgreSQL



## 📊 Analytics Features

### Tracked Events
- Page views and session duration
- Project card interactions
- GitHub/demo link clicks
- CV download events
- Contact form submissions
- Section visits and scroll depth

### Dashboard Metrics
- Real-time visitor count
- Active sessions
- Project view trends
- Geographic visitor distribution
- Device and browser analytics
- Conversion funnel analysis

## 🔐 Security

- JWT-based authentication for admin features
- Secure API endpoints with authorization
- Input validation and sanitization
- CORS configuration for cross-origin requests
- Environment-based configuration management

## 🎨 UI/UX Features

- Responsive design for all devices
- Modern, professional styling
- Interactive charts and visualizations
- Real-time metrics updates
- Smooth animations and transitions
- Accessibility compliance

## 📱 Cross-Platform Support

The .NET MAUI Blazor Hybrid app provides:
- Native mobile experience (iOS, Android)
- Desktop applications (Windows, macOS)
- Shared codebase with web version
- Platform-specific optimizations

## 🔧 Configuration

### API Configuration
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your connection string"
  },
  "JwtSettings": {
    "SecretKey": "Your secret key",
    "Issuer": "PortfolioApi",
    "Audience": "PortfolioClient"
  }
}
```

### Environment Variables
- `ASPNETCORE_ENVIRONMENT` - Development/Production
- `DATABASE_URL` - Database connection string
- `JWT_SECRET` - JWT signing key

## 📈 Performance

- Blazor WebAssembly for fast client-side rendering
- Efficient data loading with pagination
- Optimized images and assets
- CDN-ready static file serving
- Database query optimization

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🎯 Roadmap

- [ ] Advanced analytics with ML insights
- [ ] Social media integration
- [ ] Blog/article management system
- [ ] Multi-language support
- [ ] Advanced SEO optimization
- [ ] Integration with external APIs (GitHub, LinkedIn)
- [ ] Real-time chat/contact system
- [ ] Portfolio template system
