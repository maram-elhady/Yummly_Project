﻿namespace Yummly.DTO.Search
{
    public class SearchCategoryResponseDto
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<SearchCategoryDto> Categories { get; set; }
    }
}
