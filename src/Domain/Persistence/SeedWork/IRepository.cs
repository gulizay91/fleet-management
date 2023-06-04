namespace Domain.Persistence.SeedWork;

public interface IRepository<T> where T : class, IAggregateRoot
{
}