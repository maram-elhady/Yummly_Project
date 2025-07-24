namespace Yummly.DTO.Search
{
    public class SearchUserResponseDto
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<SearchUserDto> Users { get; set; }
    }
}
