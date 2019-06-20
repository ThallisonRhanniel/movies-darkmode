using MoviesXF.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MoviesXF.Views
{
    public partial class Settings : ContentPage
    {

        private readonly SettingsViewModel _settingsViewModel;
        public Settings()
        {
            InitializeComponent();
            _settingsViewModel = (SettingsViewModel)this.BindingContext;
            _settingsViewModel.ChangeDarkModeEvent += SettingsViewModelChangeDarkModeEvent;
        }

        private void SettingsViewModelChangeDarkModeEvent() => AnimationDarkMode.PlayFrameSegment(15, 15);
        
    }
}
