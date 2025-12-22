using Blazored.LocalStorage;
using Microsoft.JSInterop;

namespace Portfolio.Web.Services;

public interface IAnalyticsService
{
    Task InitializeAsync();
    Task TrackPageViewAsync(string pageUrl, string pageTitle);
    Task TrackEventAsync(string eventType, string? eventData = null);
    Task<AnalyticsMetrics> GetMetricsAsync();
    string SessionId { get; }
}

public class AnalyticsMetrics
{
    public int TotalVisitors { get; set; }
    public int ActiveSessions { get; set; }
    public int TotalProjectViews { get; set; }
}

public class AnalyticsService : IAnalyticsService
{
    private readonly ILocalStorageService _localStorage;
    private readonly IJSRuntime _jsRuntime;
    private string _sessionId = string.Empty;
    private bool _isNewVisitor = false;

    public string SessionId => _sessionId;

    public AnalyticsService(ILocalStorageService localStorage, IJSRuntime jsRuntime)
    {
        _localStorage = localStorage;
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        try
        {
            // Get or create visitor ID (persistent across sessions)
            var visitorId = await _localStorage.GetItemAsync<string>("visitorId");
            if (string.IsNullOrEmpty(visitorId))
            {
                visitorId = Guid.NewGuid().ToString();
                await _localStorage.SetItemAsync("visitorId", visitorId);
                _isNewVisitor = true;
            }

            // Get or create session ID (expires after 30 minutes of inactivity)
            _sessionId = await _localStorage.GetItemAsync<string>("sessionId") ?? string.Empty;
            var lastActivity = await _localStorage.GetItemAsync<DateTime?>("lastActivity");
            
            if (string.IsNullOrEmpty(_sessionId) || 
                lastActivity == null || 
                DateTime.UtcNow.Subtract(lastActivity.Value).TotalMinutes > 30)
            {
                _sessionId = Guid.NewGuid().ToString();
                await _localStorage.SetItemAsync("sessionId", _sessionId);
                
                // Track new session
                await IncrementCounter("totalSessions");
                
                // Track new visitor if this is their first visit
                if (_isNewVisitor)
                {
                    await IncrementCounter("totalVisitors");
                }
            }

            await _localStorage.SetItemAsync("lastActivity", DateTime.UtcNow);
            
            // Update active sessions (cleanup old ones)
            await UpdateActiveSessions();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Analytics initialization error: {ex.Message}");
            _sessionId = Guid.NewGuid().ToString();
        }
    }

    public async Task TrackPageViewAsync(string pageUrl, string pageTitle)
    {
        try
        {
            if (string.IsNullOrEmpty(_sessionId))
                await InitializeAsync();

            // Track page view
            await IncrementCounter("totalPageViews");
            
            // Track specific page views
            var pageKey = $"page_{pageUrl.Replace("/", "_").Replace("?", "_")}";
            await IncrementCounter(pageKey);
            
            // Update last activity
            await _localStorage.SetItemAsync("lastActivity", DateTime.UtcNow);
            
            // Track in session storage for active session tracking
            await UpdateSessionActivity();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Page view tracking error: {ex.Message}");
        }
    }

    public async Task TrackEventAsync(string eventType, string? eventData = null)
    {
        try
        {
            if (string.IsNullOrEmpty(_sessionId))
                await InitializeAsync();

            // Track specific events
            if (eventType == "project_view")
            {
                await IncrementCounter("totalProjectViews");
            }
            
            await IncrementCounter($"event_{eventType}");
            await _localStorage.SetItemAsync("lastActivity", DateTime.UtcNow);
            await UpdateSessionActivity();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Event tracking error: {ex.Message}");
        }
    }

    public async Task<AnalyticsMetrics> GetMetricsAsync()
    {
        try
        {
            var totalVisitors = await GetCounter("totalVisitors");
            var activeSessions = await GetActiveSessions();
            var projectViews = await GetCounter("totalProjectViews");

            return new AnalyticsMetrics
            {
                TotalVisitors = totalVisitors,
                ActiveSessions = activeSessions,
                TotalProjectViews = projectViews
            };
        }
        catch
        {
            return new AnalyticsMetrics();
        }
    }

    private async Task<int> GetCounter(string key)
    {
        try
        {
            return await _localStorage.GetItemAsync<int>(key);
        }
        catch
        {
            return 0;
        }
    }

    private async Task IncrementCounter(string key)
    {
        try
        {
            var current = await GetCounter(key);
            await _localStorage.SetItemAsync(key, current + 1);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Counter increment error for {key}: {ex.Message}");
        }
    }

    private async Task UpdateSessionActivity()
    {
        try
        {
            var activeSessions = await _localStorage.GetItemAsync<Dictionary<string, DateTime>>("activeSessions") 
                ?? new Dictionary<string, DateTime>();
            
            activeSessions[_sessionId] = DateTime.UtcNow;
            await _localStorage.SetItemAsync("activeSessions", activeSessions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Session activity update error: {ex.Message}");
        }
    }

    private async Task UpdateActiveSessions()
    {
        try
        {
            var activeSessions = await _localStorage.GetItemAsync<Dictionary<string, DateTime>>("activeSessions") 
                ?? new Dictionary<string, DateTime>();
            
            // Remove sessions older than 30 minutes
            var cutoff = DateTime.UtcNow.AddMinutes(-30);
            var activeSessionsFiltered = activeSessions
                .Where(kvp => kvp.Value > cutoff)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            
            // Add current session
            activeSessionsFiltered[_sessionId] = DateTime.UtcNow;
            
            await _localStorage.SetItemAsync("activeSessions", activeSessionsFiltered);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Active sessions update error: {ex.Message}");
        }
    }

    private async Task<int> GetActiveSessions()
    {
        try
        {
            var activeSessions = await _localStorage.GetItemAsync<Dictionary<string, DateTime>>("activeSessions") 
                ?? new Dictionary<string, DateTime>();
            
            var cutoff = DateTime.UtcNow.AddMinutes(-30);
            return activeSessions.Count(kvp => kvp.Value > cutoff);
        }
        catch
        {
            return 1; // At least current session
        }
    }
}