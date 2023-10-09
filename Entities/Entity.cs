public class Entity
{

	public Guid Id { get; set; }
	public string Name { get; set; }
	public bool Active { get; set; } = true;
	public string CreatedBy { get; set; }
	public DateTime CreatedOn { get; set; }
	public string EditedBy { get; set; }
	public DateTime EditedOn { get; set; }


	public Entity()
	{
		Id = Guid.NewGuid();
	}


}