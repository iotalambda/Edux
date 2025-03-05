
using DnsClient;

namespace Edux.Web.Stuff.Rare;

public class RequireCanonicalHostNameDelegatingHandler(Func<string, bool> condition) : DelegatingHandler
{
    static readonly LookupClient lookupClient = new(new LookupClientOptions { UseCache = true });

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri is not { } requestUri)
            throw new Exception();

        var originalHostName = requestUri.Host;

        async Task ResolveCanonicalHostName()
        {
            if (condition(originalHostName))
                return;

            var recordsResponse = await lookupClient.QueryAsync(originalHostName, QueryType.CNAME, cancellationToken: cancellationToken);

            foreach (var record in recordsResponse.Answers.CnameRecords())
                if (record.CanonicalName.Value.TrimEnd('.') is { } resolvedHostName && condition(resolvedHostName))
                {
                    var uriBuilder = new UriBuilder(requestUri);
                    uriBuilder.Host = resolvedHostName;
                    request.RequestUri = uriBuilder.Uri;
                    return;
                }

            throw new Exception("Could not resolve canonical host name!");
        }

        await ResolveCanonicalHostName();

        return await base.SendAsync(request, cancellationToken);
    }

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri is not { } requestUri)
            throw new Exception();

        var originalHostName = requestUri.Host;

        void ResolveCanonicalHostName()
        {
            if (condition(originalHostName))
                return;

            var recordsResponse = lookupClient.Query(originalHostName, QueryType.CNAME);

            foreach (var record in recordsResponse.Answers.CnameRecords())
            {
                if (record.CanonicalName.Value.TrimEnd('.') is { } resolvedHostName && condition(resolvedHostName))
                {
                    var uriBuilder = new UriBuilder(requestUri);
                    uriBuilder.Host = resolvedHostName;
                    request.RequestUri = uriBuilder.Uri;
                    return;
                }
            }

            throw new Exception("Could not resolve canonical host name!");
        }

        ResolveCanonicalHostName();

        return base.Send(request, cancellationToken);
    }
}
