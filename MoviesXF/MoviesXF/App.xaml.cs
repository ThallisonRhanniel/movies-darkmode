using MoviesXF.Services;
using Prism;
using Prism.Ioc;
using MoviesXF.ViewModels;
using MoviesXF.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MoviesXF
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            #if DEBUG
            HotReloader.Current.Start(this);
            #endif

            InitializeComponent();

            XF.Material.Forms.Material.Init(this);

            if (Preferences.Get("IsDarkMode", false))
                ActivateDarkMode(true);

            await NavigationService.NavigateAsync("TabbedMainPage"); //NavigationPage

        }

        public static void ActivateDarkMode(bool parameter)
        {
            
            var statusBarStyleManager = DependencyService.Get<IStatusBarStyleManager>();
            if (parameter)
            {
                statusBarStyleManager.SetDarkTheme();
                //Main Page //DarkMode
                Current.Resources["BarItemColor"] = Color.White;
                Current.Resources["BarSelectedItemColor"] = Color.FromHex("#0276cb");
                Current.Resources["BarBackgroundColor"] = Color.Black;
                Current.Resources["TextColor"] = Color.White;
                //Background
                Current.Resources["BackgroundColor"] = Color.FromHex("#1e201f");
                // SearchMovies                
                Current.Resources["MaterialTextFieldStyle"] = App.Current.Resources["DarkModeMaterialTextFieldStyle"];
                Current.Resources["BackgroundSearchColor"] = Color.Black;
                Current.Resources["BackgroundMaterialTextFieldColor"] = Color.FromHex("#2d302d");
                // FullMovieInformation
                Current.Resources["LabelTextColor"] = Color.White;
                Current.Resources["LabelTextWebSiteColor"] = Color.FromHex("#303F9F");
                Current.Resources["BackgroundTitleColor"] = Color.FromHex("#2a2a2d");
                Current.Resources["BackgroundContentColor"] = Color.FromHex("#363638");
                // ListMovies
                Current.Resources["ListTitleLabelColor"] = Color.White;
                Current.Resources["ListContentLabelColor"] = Color.LightGray;
            }
            else
            {
                //StatusBar
                statusBarStyleManager.SetLightTheme();
                //Main Page // DefaultMode
                Current.Resources["BarItemColor"] = Color.SlateGray;
                Current.Resources["BarSelectedItemColor"] = Color.White;
                Current.Resources["BarBackgroundColor"] = Color.FromHex("#3F51B5");
                Current.Resources["TextColor"] = Color.Black;
                //Background
                Current.Resources["BackgroundColor"] = Color.White;
                // SearchMovies                
                Current.Resources["MaterialTextFieldStyle"] = App.Current.Resources["DefaultMaterialTextFieldStyle"];
                Current.Resources["BackgroundSearchColor"] = Color.FromHex("#3F51B5");
                Current.Resources["BackgroundMaterialTextFieldColor"] = Color.White;
                // FullMovieInformation
                Current.Resources["LabelTextColor"] = Color.DimGray;
                Current.Resources["LabelTextWebSiteColor"] = Color.FromHex("#0d47a1");
                Current.Resources["BackgroundTitleColor"] = Color.FromHex("#455a64");
                Current.Resources["BackgroundContentColor"] = Color.White;
                // ListMovies
                Current.Resources["ListTitleLabelColor"] = Color.Black;
                Current.Resources["ListContentLabelColor"] = Color.Gray;
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<FullMovieInformation, FullMovieInformationViewModel>();
            containerRegistry.RegisterForNavigation<TabbedMainPage, TabbedMainPageViewModel>();
            containerRegistry.RegisterForNavigation<SearchMovies, SearchMoviesViewModel>();
            containerRegistry.RegisterForNavigation<Settings, SettingsViewModel>();
        }
    }
}
