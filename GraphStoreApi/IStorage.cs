namespace GraphStoreApi;

public interface IStorage
{
    IEnumerable<object> GetAll(string endpoint);
    object? Get(string endpoint, string id);
    void Create(string endpoint, string id, object value);
    void Update(string endpoint, string id, object value);
    void Delete(string endpoint, string id);
}