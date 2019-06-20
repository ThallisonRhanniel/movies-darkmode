using System.Threading.Tasks;
using MoviesXF.Models;
using Refit;

namespace MoviesXF.Services
{
    public interface IRestApi
    {


        [Get("/?s={searchBy}&apikey={apiKey}&y={year}&page={pageNumber}")]
        Task<MoviesSearch> GetMovies(string searchBy, string apiKey, string year = "", int pageNumber = 1);

        [Get("/?t={movieTitle}&y={year}&apikey={apiKey}")]
        Task<FullMovieInformationModel> GetMovieWithTheYear(string movieTitle, int year, string apiKey);
        
        [Get("/?i={imdbId}&apikey={apiKey}")]
        Task<FullMovieInformationModel> GetMovie(string imdbId, string apiKey);



        
    }
}
