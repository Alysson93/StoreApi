using Flunt.Notifications;

public class Entity : Notifiable<Notification>
{

	public Guid Id { get; set; }
	public string Name { get; protected set; }
	public bool Active { get; protected set; }
	public string CreatedBy { get; set; }
	public DateTime CreatedOn { get; set; }
	public string EditedBy { get; set; }
	public DateTime EditedOn { get; set; }


	public Entity()
	{
		Id = Guid.NewGuid();
	}


}