using System;
using Xamarin.Forms;

namespace MoviesXF.Views
{
    public partial class SearchMovies : ContentPage
    {
        public SearchMovies()
        {
            InitializeComponent();

            TextFieldSearch.TextChanged += OnTextFieldSearchOnTextChanged;
        }

        private void OnTextFieldSearchOnTextChanged(object sender, TextChangedEventArgs e)
        {
            // Método nescessário para reiniciar a animação de pesquisa.
            if (e.NewTextValue == string.Empty)
                AnimationSearch.PlayFrameSegment(120, 180);
        }
    }
}
