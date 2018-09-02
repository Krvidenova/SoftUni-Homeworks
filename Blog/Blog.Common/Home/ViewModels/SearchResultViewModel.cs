namespace Blog.Common.Home.ViewModels
{
    using System.Collections.Generic;

    public class SearchResultViewModel
    {
        public string SearchTerm { get; set; }

        public IEnumerable<SearchPostConciseViewModel> Posts { get; set; }
    }
}