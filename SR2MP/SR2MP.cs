using System.Security.Cryptography;

namespace SR2MP;

public static class SR2MP
{
    private static string _hash;

    public static string GetModHash()
    {
        if (_hash != null)
            return _hash;

        var assemblyPath = typeof(SR2MP).Assembly.Location;
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(assemblyPath);
        var hashBytes = md5.ComputeHash(stream);
        return _hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLowerInvariant();
    }
}