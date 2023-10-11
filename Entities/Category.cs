using Flunt.Validations;

public class Category : Entity
{

	public Category(string name, string createdBy, string editedBy)
	{
		
		Name = name;
		Active = true;
		CreatedBy = createdBy;
		CreatedOn = DateTime.UtcNow;
		EditedBy = editedBy;
		EditedOn = DateTime.UtcNow;
		Validate();
	
	}

	public void EditInfo(string name, bool active)
	{
		Name = name;
		Active = active;
		Validate();
	}

	public void Validate()
	{
		var contract = new Contract<Category>()
		.IsNotNullOrEmpty(Name, "Name", "Nome é um campo obrigatório.")
		.IsNotNullOrEmpty(CreatedBy, "CreatedBy", "Nome do criador obrigatório.")
		.IsNotNullOrEmpty(EditedBy, "EditedBy", "Nome do editor obrigatório.");
		AddNotifications(contract);
	}

}