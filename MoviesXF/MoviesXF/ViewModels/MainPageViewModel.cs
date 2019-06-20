using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using MoviesXF.Helpers;
using MoviesXF.Models;
using Newtonsoft.Json;

namespace MoviesXF.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {

        #region Properties

        public ObservableCollection<FullMovieInformationModel> ItemsHighlights { get; set; }

        public ObservableCollection<FullMovieInformationModel> ItemsMovies { get; set; }

        public ObservableCollection<FullMovieInformationModel> ItemsSeries { get; set; }

        public DelegateCommand<object> ClickCommand { get; private set; }

        #endregion

        #region Methods

        private List<FullMovieInformationModel> ReadJson(string fileName)
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPageViewModel)).Assembly;
            Stream stream = assembly.GetManifestResourceStream(fileName);
            if (stream != null)
            {
                string text = string.Empty;
                using (var reader = new System.IO.StreamReader(stream))
                    text = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<FullMovieInformationModel>>(text);
            }
            else
            {
                return new List<FullMovieInformationModel>();
            }
        }

        #endregion
        
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            ClickCommand = new DelegateCommand<object>(async itemClicked =>
            {
                await NavigationService.NavigateAsync("FullMovieInformation", new NavigationParameters { { "FullMovieInformationModel", itemClicked } });
            });


            ItemsHighlights = new ObservableCollection<FullMovieInformationModel>(ReadJson(PreloadingMovies.Highlights));

            ItemsMovies = new ObservableCollection<FullMovieInformationModel>(ReadJson(PreloadingMovies.Movies));

            ItemsSeries = new ObservableCollection<FullMovieInformationModel>(ReadJson(PreloadingMovies.Series));


            Title = "Main Page";
        }

    }
}
