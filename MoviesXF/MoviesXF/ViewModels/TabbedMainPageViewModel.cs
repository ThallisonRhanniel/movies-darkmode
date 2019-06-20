using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;

namespace MoviesXF.ViewModels
{
	public class TabbedMainPageViewModel : ViewModelBase
    {
        public TabbedMainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }
	}
}
