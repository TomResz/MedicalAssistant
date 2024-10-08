namespace MedicalAssistant.UI.Shared.Response;

public class PagedList<T>
{
    public List<T> Items { get; set; }
    public int MyProperty { get; set; }
	public int Page { get; set; }
	public int PageSize { get; set; }
    public int PageTotalCount { get; set; }
	public int TotalCount { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}
