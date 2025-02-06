namespace ToDoApp.Shared.Search;

public class SearchFilter
{
	public string PropertyName { get; set; }

	public object PropertyValue { get; set; }

	public FilterOperator Operator { get; set; }
}