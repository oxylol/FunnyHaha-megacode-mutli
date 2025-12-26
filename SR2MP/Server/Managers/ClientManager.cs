using System.Collections.Concurrent;
using System.Net;
using SR2MP.Server.Models;

namespace SR2MP.Server.Managers;

// Custom comparer for IPEndPoint to avoid string allocations
internal class IPEndPointComparer : IEqualityComparer<IPEndPoint>
{
    public bool Equals(IPEndPoint? x, IPEndPoint? y)
    {
        if (x == null || y == null) return x == y;
        return x.Address.Equals(y.Address) && x.Port == y.Port;
    }

    public int GetHashCode(IPEndPoint obj)
    {
        return HashCode.Combine(obj.Address, obj.Port);
    }
}

public class ClientManager
{
    private readonly ConcurrentDictionary<IPEndPoint, ClientInfo> clients = new(new IPEndPointComparer());

    public event Action<ClientInfo>? OnClientAdded;
    public event Action<ClientInfo>? OnClientRemoved;
    public int ClientCount => clients.Count;

    public bool TryGetClient(IPEndPoint endPoint, out ClientInfo? client)
    {
        return clients.TryGetValue(endPoint, out client);
    }

    public ClientInfo? GetClient(IPEndPoint endPoint)
    {
        clients.TryGetValue(endPoint, out var client);
        return client;
    }

    public ClientInfo AddClient(IPEndPoint endPoint, string playerId)
    {
        var client = new ClientInfo(endPoint, playerId);

        if (clients.TryAdd(endPoint, client))
        {
            SrLogger.LogMessage($"Client added! (PlayerId: {playerId})",
                $"Client added: {client.GetClientInfo()} (PlayerId: {playerId})");
            OnClientAdded?.Invoke(client);
            return client;
        }
        else
        {
            SrLogger.LogWarning($"Client already exists! (PlayerId: {playerId})",
                $"Client already exists: {client.GetClientInfo()} (PlayerId: {playerId})");
            return clients[endPoint];
        }
    }

    public bool RemoveClient(IPEndPoint endPoint)
    {
        if (clients.TryRemove(endPoint, out var client))
        {
            SrLogger.LogMessage($"Client removed!",
                $"Client removed: {client.GetClientInfo()}");
            OnClientRemoved?.Invoke(client);
            return true;
        }
        return false;
    }

    public void UpdateHeartbeat(IPEndPoint endPoint)
    {
        if (clients.TryGetValue(endPoint, out var client))
        {
            client.UpdateHeartbeat();
        }
    }

    public IEnumerable<ClientInfo> GetAllClients()
    {
        return clients.Values;
    }

    public IEnumerable<ClientInfo> GetTimedOutClients()
    {
        return clients.Values.Where(client => client.IsTimedOut());
    }

    public void RemoveTimedOutClients()
    {
        var timedOut = GetTimedOutClients().ToArray();
        foreach (var client in timedOut)
        {
            RemoveClient(client.EndPoint);
        }
    }

    public void Clear()
    {
        var allClients = clients.Values.ToList();
        clients.Clear();

        foreach (var client in allClients)
        {
            OnClientRemoved?.Invoke(client);
        }

        SrLogger.LogMessage("All clients cleared", SrLogger.LogTarget.Both);
    }
}