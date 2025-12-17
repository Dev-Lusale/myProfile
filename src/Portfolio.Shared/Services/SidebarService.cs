using System;

namespace Portfolio.Shared.Services
{
    public class SidebarService
    {
        private bool _isCollapsed = true; // Start collapsed by default
        
        public event Action? OnStateChanged;
        
        public bool IsCollapsed => _isCollapsed;
        
        public void ToggleSidebar()
        {
            _isCollapsed = !_isCollapsed;
            OnStateChanged?.Invoke();
        }
        
        public void CollapseSidebar()
        {
            if (!_isCollapsed)
            {
                _isCollapsed = true;
                OnStateChanged?.Invoke();
            }
        }
        
        public void ExpandSidebar()
        {
            if (_isCollapsed)
            {
                _isCollapsed = false;
                OnStateChanged?.Invoke();
            }
        }

        // Method to handle keyboard shortcuts
        public void HandleKeyboardShortcut(string key, bool ctrlKey)
        {
            // Ctrl + B to toggle sidebar (common shortcut)
            if (ctrlKey && key.ToLower() == "b")
            {
                ToggleSidebar();
            }
        }
    }
}