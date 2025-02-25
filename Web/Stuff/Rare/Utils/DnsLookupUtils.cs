using DnsClient;

namespace Edux.Web.Stuff.Rare.Utils;

public class DnsLookupUtils
{
    static readonly LookupClient lookupClient = new(new LookupClientOptions { UseCache = true });
    public static async Task<string> ResolveKeyVaultUri(string configuredHostName)
    {
        if (configuredHostName.EndsWith("vault.azure.net"))
            return $"https://{configuredHostName}/";

        var response = await lookupClient.QueryAsync(configuredHostName, QueryType.CNAME);
        var record = response.Answers.CnameRecords().FirstOrDefault();
        if (record is not { })
            throw new Exception($"Could not resolve Key Vault uri for {nameof(configuredHostName)} of '{configuredHostName}'.");

        var canonicalName = record.CanonicalName.Value;
        var resolvedHostName = canonicalName.EndsWith('.') ? canonicalName[..^1] : canonicalName;

        return await ResolveKeyVaultUri(resolvedHostName);
    }
}
