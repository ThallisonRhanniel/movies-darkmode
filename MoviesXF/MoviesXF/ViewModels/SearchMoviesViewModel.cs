using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Lottie.Forms;
using MoviesXF.Helpers;
using MoviesXF.Models;
using MoviesXF.Services;
using Prism;
using Prism.Navigation;
using Refit;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using XF.Material.Forms.UI.Dialogs.Configurations;

namespace MoviesXF.ViewModels
{
	public class SearchMoviesViewModel : ViewModelBase
    {
        #region Properties

        private int page = 1;

        private int MaxPage = 0;

        private string _oldSearchValue;

        private MoviesSearch _unSelecItem = null;
        public MoviesSearch UnSelecItem
        {
            get => null;
            set { SetProperty(ref _unSelecItem, value); }
        }


        private string _searchParameter;
        public string SearchParameter
        {
            get { return _searchParameter; }
            set
            {
                if (value == string.Empty)
                {
                    Items.Clear();
                    IsSearching = false;
                }

                if (!string.IsNullOrEmpty(_oldSearchValue) && _oldSearchValue != value)
                {
                    IsSearching = false;
                    _searchAnimationView.PlayFrameSegment(0, 0);
                }

                SetProperty(ref _searchParameter, value);
            }
        }
        
        private bool _isSearching;
        public bool IsSearching
        {
            get { return _isSearching; }
            set { SetProperty(ref _isSearching, value); }
        }

        #endregion

        #region Methods

        private MaterialAlertDialogConfiguration SetDialogConfiguration()
        {
            var alertDialogConfiguration = new MaterialAlertDialogConfiguration
            {
                BackgroundColor = (Color)PrismApplicationBase.Current.Resources["BackgroundColor"],
                TitleTextColor = (Color)PrismApplicationBase.Current.Resources["TextColor"],
                MessageTextColor = (Color)PrismApplicationBase.Current.Resources["TextColor"],
                TintColor = Color.FromHex("#448AFF"),
                CornerRadius = 8,
                ScrimColor = Color.FromHex("#232F34").MultiplyAlpha(0.32),
                ButtonAllCaps = false
            };
            return alertDialogConfiguration;
        }

        private MaterialLoadingDialogConfiguration SetLoadingDialogConfiguration()
        {
            var loadingDialogConfiguration = new MaterialLoadingDialogConfiguration()
            {
                BackgroundColor = (Color)PrismApplicationBase.Current.Resources["BackgroundColor"],
                MessageTextColor = (Color)PrismApplicationBase.Current.Resources["TextColor"],
                TintColor = Color.FromHex("#448AFF"),
                CornerRadius = 8,
                ScrimColor = Color.FromHex("#232F34").MultiplyAlpha(0.32)
            };


            return loadingDialogConfiguration;
        }

        private async Task Dialog(string title, string message) => await MaterialDialog.Instance.AlertAsync(title: title,
            message: message,
            acknowledgementText: "Got It",
            configuration: SetDialogConfiguration());

        private async Task LoadMore()
        {
            if (page < MaxPages())
            {
                page++;
                await GetMoviesFromApi(true);
            }
            else
            {
                await Dialog("Alert", "This is the end");
            }
        }

        private int MaxPages() => MaxPage.ToString().Contains("0") ? MaxPage / 10 : (MaxPage / 10) + 1;

        private void ResetPageCounter() => page = 1;

        private async Task GetMoviesFromApi(bool isloadMore = false)
        {
            try
            {
                var omDbApi = RestService.For<IRestApi>(EndPoints.BaseUrl);
                var result = await omDbApi.GetMovies(_oldSearchValue, OmDb.ApiKey, pageNumber: page);
                if (result.Movies != null)
                {

                    _searchAnimationView.PlayFrameSegment(0, 110);
                    MaxPage = Convert.ToInt16(result.totalResults);

                    foreach (var item in result.Movies)
                        Items.Add(item);
                    if (!isloadMore)
                        IsSearching = true;
                }
                else
                {
                    await Dialog("Alert", "Movie not found");
                    // Isso é para forçar os filmes vim na pagina 1.
                    if (!isloadMore)
                        IsSearching = false;
                }
            }
            catch (Exception)
            {
                await Dialog("Alert", "Movie not found");
                // Isso é para forçar os filmes vim na pagina 1.
                if (!isloadMore)
                    IsSearching = false;
            }
        }

        #endregion

        #region Commands

        public ObservableCollection<Search> Items { get; private set; }

        public DelegateCommand<Search> ItemClickCommand { get; private set; }

        private AnimationView _searchAnimationView;

        public DelegateCommand<AnimationView> SearchCommand { get; private set; }

        public DelegateCommand<AnimationView> loadMoreCommand { get; private set; }
        #endregion

        
        public SearchMoviesViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            
            Items = new ObservableCollection<Search>();

            SearchCommand = new DelegateCommand<AnimationView>(async SearchLottie =>
            {
                if (_searchAnimationView == null)
                    _searchAnimationView = SearchLottie;
                if (IsSearching)
                {
                    //Após fazer uma pesquisa e clicar no "X" limpo o que foi digitado.
                    if (SearchParameter != string.Empty)
                        SearchParameter = string.Empty;
                    return;
                }

                _oldSearchValue = SearchParameter;
                ResetPageCounter();
                using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Searching...",
                    configuration: SetLoadingDialogConfiguration()))
                {
                    await GetMoviesFromApi();
                }
            });

            ItemClickCommand = new DelegateCommand<Search>(async itemClicked =>
            {
                try
                {
                    var OmDbApi = RestService.For<IRestApi>(EndPoints.BaseUrl);
                    var result = await OmDbApi.GetMovie(itemClicked.imdbID, OmDb.ApiKey);
                    await NavigationService.NavigateAsync("FullMovieInformation", new NavigationParameters { { "FullMovieInformationModel", result } });
                }
                catch (Exception )
                {
                    await Dialog("Alert", "Not found in Matrix, try again later...");
                }
                
            });

            loadMoreCommand = new DelegateCommand<AnimationView>(async loadMoreAnimationView =>
            {
                loadMoreAnimationView.Loop = true;
                loadMoreAnimationView.Play();
                await LoadMore();
                loadMoreAnimationView.Loop = false;
                loadMoreAnimationView.PlayFrameSegment(0, 0);
            });

        }

    }
}
