namespace Portal.Api.Dtos.Models
{
    public class SearchModel<T> where T: class
    {
        public string[] Filters { get; set; }
        public string[] Sorts { get; set; } 

    }
}
