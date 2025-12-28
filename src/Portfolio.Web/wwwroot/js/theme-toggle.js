// Theme Toggle Functionality
window.getThemePreference = function() {
    // Check localStorage first, then system preference
    const savedTheme = localStorage.getItem('theme-preference');
    if (savedTheme !== null) {
        return savedTheme === 'dark';
    }
    
    // Check system preference
    return window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
};

window.saveThemePreference = function(isDarkMode) {
    localStorage.setItem('theme-preference', isDarkMode ? 'dark' : 'light');
};

window.applyTheme = function(isDarkMode) {
    const body = document.body;
    const root = document.documentElement;
    
    if (isDarkMode) {
        body.classList.add('dark-mode');
        body.classList.remove('light-mode');
        
        // Set CSS custom properties for dark mode
        root.style.setProperty('--bg-primary', '#0f172a');
        root.style.setProperty('--bg-secondary', '#1e293b');
        root.style.setProperty('--bg-tertiary', '#334155');
        root.style.setProperty('--text-primary', '#f8fafc');
        root.style.setProperty('--text-secondary', '#e2e8f0');
        root.style.setProperty('--text-muted', '#94a3b8');
        root.style.setProperty('--border-color', '#475569');
        root.style.setProperty('--shadow-color', 'rgba(0, 0, 0, 0.5)');
        root.style.setProperty('--card-bg', '#1e293b');
        root.style.setProperty('--hero-bg', 'linear-gradient(135deg, #0f172a 0%, #1e293b 100%)');
    } else {
        body.classList.add('light-mode');
        body.classList.remove('dark-mode');
        
        // Set CSS custom properties for light mode
        root.style.setProperty('--bg-primary', '#ffffff');
        root.style.setProperty('--bg-secondary', '#f8f9fa');
        root.style.setProperty('--bg-tertiary', '#e9ecef');
        root.style.setProperty('--text-primary', '#212529');
        root.style.setProperty('--text-secondary', '#495057');
        root.style.setProperty('--text-muted', '#6c757d');
        root.style.setProperty('--border-color', '#dee2e6');
        root.style.setProperty('--shadow-color', 'rgba(0, 0, 0, 0.1)');
        root.style.setProperty('--card-bg', '#ffffff');
        root.style.setProperty('--hero-bg', 'linear-gradient(135deg, #ffffff 0%, #f8f9fa 100%)');
    }
};

// Initialize theme on page load
document.addEventListener('DOMContentLoaded', function() {
    const isDarkMode = window.getThemePreference();
    window.applyTheme(isDarkMode);
});

// Listen for system theme changes
if (window.matchMedia) {
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', function(e) {
        // Only apply system preference if user hasn't manually set a preference
        if (localStorage.getItem('theme-preference') === null) {
            window.applyTheme(e.matches);
        }
    });
}