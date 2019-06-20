
using System.Collections.Generic;
using Newtonsoft.Json;


namespace MoviesXF.Models
{
    public class Search
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string imdbID { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
    }

    public class MoviesSearch
    {
        [JsonProperty("Search")]
        public List<Search> Movies { get; set; }
        public string totalResults { get; set; }
        public string Response { get; set; }
    }
}
