using Prism.Commands;
using System;
using Lottie.Forms;
using Prism;
using Prism.Navigation;
using Xamarin.Essentials;


namespace MoviesXF.ViewModels
{
	public class SettingsViewModel : ViewModelBase , IActiveAware
    {
        #region Properties

        public delegate void MyEventAction();
        //Evento criado para iniciar o lottie com um valor inicial caso tenha ativado o DarkMode...
        //Ao iniciar o valor diretamente na view, o lottie não funcionou corretamente.
        public event MyEventAction ChangeDarkModeEvent;

        public event EventHandler IsActiveChanged;

        private bool _IsActive;
        public bool IsActive
        {
            get { return _IsActive; }
            set { SetProperty(ref _IsActive, value, RaiseIsActiveChanged); } //RaiseIsActiveChanged
        }

        private bool _isReload;

        protected virtual void RaiseIsActiveChanged()
        {
            if (IsActive && !_isReload)
            {
                //uso essa variável para garantir que será chamado apenas uma vez.
                _isReload = true;
                if (Preferences.Get("IsDarkMode", false))
                    ChangeDarkModeEvent?.Invoke();
            }
        }
        #endregion

        #region Commands

        public DelegateCommand<AnimationView> SwitchToDarkModeCommand { get; private set; }

        #endregion
        
        public SettingsViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            SwitchToDarkModeCommand = new DelegateCommand<AnimationView>(async Switch =>
            {
                if (Preferences.Get("IsDarkMode", false))
                {
                    Switch.PlayFrameSegment(15, 50);
                    Preferences.Set("IsDarkMode", false);
                    App.ActivateDarkMode(false);
                }
                else
                {
                    Switch.PlayFrameSegment(0, 15);
                    Preferences.Set("IsDarkMode", true);
                    App.ActivateDarkMode(true);
                }
            });
        }
    }
}
