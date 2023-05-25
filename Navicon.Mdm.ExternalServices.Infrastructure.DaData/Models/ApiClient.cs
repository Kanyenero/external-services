using Dadata;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models;

internal class ApiClient
{
    private readonly CleanClientAsync _cleanClient;
    private readonly SuggestClientAsync _suggestClient;
    private readonly OutwardClientAsync _outwardClient;
    private readonly ProfileClientAsync _profileClient;

    public ApiClient(string publicToken, string secretToken, HttpClient httpClient)
    {
        _cleanClient = new CleanClientAsync(publicToken, secretToken, client: httpClient);
        _suggestClient = new SuggestClientAsync(publicToken, client: httpClient);
        _outwardClient = new OutwardClientAsync(publicToken, client: httpClient);
        _profileClient = new ProfileClientAsync(publicToken, secretToken, client: httpClient);
    }

    public CleanClientAsync CleanClient => _cleanClient;

    public SuggestClientAsync SuggestClient => _suggestClient;

    public OutwardClientAsync OutwardClient => _outwardClient;

    public ProfileClientAsync ProfileClient => _profileClient;
}
