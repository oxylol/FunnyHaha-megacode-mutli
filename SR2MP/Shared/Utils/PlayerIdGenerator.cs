namespace SR2MP.Shared.Utils;

public static class PlayerIdGenerator
{
    public static long GeneratePersistentPlayerId()
    {
        long newId;

        do
        {
            newId = Random.Shared.NextInt64(1, long.MaxValue); // -1 = Invalid, 0 = Host, MaxValue = Also Invalid
        }
        while (playerObjects.ContainsKey(newId));

        return newId;
    }

    public static bool IsValidPlayerId(long playerId) => playerId is not (-1 or long.MaxValue);
}