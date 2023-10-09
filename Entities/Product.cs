public class Product : Entity
{

	public string Description { get; set; }
	public bool HasStock { get; set; }
	public Category Category { get; set; }
	public Guid CategoryId { get; set; }

}