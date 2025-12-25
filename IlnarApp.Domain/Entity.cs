namespace IlnarApp.Domain;


public abstract class Entity(Guid id)
{
	protected Entity() : this(Guid.NewGuid())
	{
	}

	
	public Guid Id { get; init; } = id;
}