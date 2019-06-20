using Prism.Commands;
using MoviesXF.Models;
using Prism.Navigation;
using Xamarin.Essentials;

namespace MoviesXF.ViewModels
{
	public class FullMovieInformationViewModel : ViewModelBase
    {
        #region Properties

        private FullMovieInformationModel _fullMovieInformationModel;
        public FullMovieInformationModel FullMovieInformationModel
        {
            get { return _fullMovieInformationModel; }
            set { SetProperty(ref _fullMovieInformationModel, value); }
        }

        public DelegateCommand<object> ClickCommand { get; private set; }

        public DelegateCommand<string> OpenWebSiteCommand { get; private set; }

        #endregion

        #region Methods

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("FullMovieInformationModel"))
                FullMovieInformationModel = (FullMovieInformationModel)parameters["FullMovieInformationModel"];
        }

        #endregion

        public FullMovieInformationViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            OpenWebSiteCommand = new DelegateCommand<string>(async Url =>
            {
                if (!string.IsNullOrEmpty(Url) && !Url.Contains("N/A"))
                    await Browser.OpenAsync(Url, BrowserLaunchMode.SystemPreferred);
            });

        }

    }
}
