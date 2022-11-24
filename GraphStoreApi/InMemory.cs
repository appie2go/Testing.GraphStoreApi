namespace GraphStoreApi;

public class InMemory : IStorage
{
    private static readonly Dictionary<string, object> Cache = new();

    public IEnumerable<object> GetAll(string endpoint) => Cache
        .Where(x => x.Key.StartsWith(endpoint))
        .Select(x => x.Value);

    public object? Get(string endpoint, string id)
    {
        var key = CreateKey(endpoint, id);
        if (!Cache.TryGetValue(key, out var result))
        {
            throw new KeyNotFoundException($"No such item. An item with {key} does not exist.");
        }

        return result;
    }

    public void Create(string endpoint, string id, object value)
    {
        var key = CreateKey(endpoint, id);
        Cache.Add(key, value);
    }
    
    public void Update(string endpoint, string id, object value)
    {
        var key = CreateKey(endpoint, id);
        if (!Cache.ContainsKey(key))
        {
            throw new KeyNotFoundException($"No such item. An item with {key} does not exist.");
        }

        Cache[key] = value;
    }

    public void Delete(string endpoint, string id)
    {
        var key = CreateKey(endpoint, id);
        if (!Cache.ContainsKey(key))
        {
            throw new KeyNotFoundException($"No such item. An item with {key} does not exist.");
        }

        Cache.Remove(key);
    }

    private static string CreateKey(string endpoint, string id) => $"{endpoint}_{id}";
}