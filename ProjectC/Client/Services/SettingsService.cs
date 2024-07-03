namespace ProjectC.Client.Services
{
    public class SettingsService
    {
        public event Action? OnChangeDarkMode;
        private bool _darkMode;

        public SettingsService()
        {
            DarkMode = true;
        }

        public bool DarkMode
        {
            get => _darkMode;
            set
            {
                _darkMode = value;
                NotifyOnChangeDarkMode();
            }
        }

        private void NotifyOnChangeDarkMode()
        {
            this.OnChangeDarkMode?.Invoke();
        }
    }
}
