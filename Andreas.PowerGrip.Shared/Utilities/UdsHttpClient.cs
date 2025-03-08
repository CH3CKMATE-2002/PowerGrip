using System.Net.Http.Json;

namespace Andreas.PowerGrip.Shared;

public sealed class UdsHttpClient: IDisposable
{
    private bool _isDisposed = false;

    private readonly HttpClient _client;

    public UdsHttpClient(string udsPath)
    {
        var handler = new SocketsHttpHandler
        {
            ConnectCallback = async (context, cancellationToken) =>
            {
                try
                {
                    Socket socket = new(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
                    UnixDomainSocketEndPoint endpoint = new(udsPath);
                    await socket.ConnectAsync(endpoint, cancellationToken);
                    return new NetworkStream(socket, ownsSocket: true);
                }
                catch (Exception ex)
                {
                    throw new HttpRequestException("Failed to connect to the Unix Domain Socket", ex);
                }
            }
        };

        _client = new HttpClient(handler);
    }

    /// <summary>
    /// Uses GET HTTP method to get a resources given a path
    /// </summary>
    /// <typeparam name="T">The type of the returned resource (will be parsed as JSON)</typeparam>
    /// <param name="path">The path to the resource that starts with <c>'/'</c></param>
    /// <returns>The <see cref="Task{T}"/> that has <see cref="T"/> as its data</returns>
    public async Task<T?> GetAsync<T>(string path)
    {
        using var response = await _client.GetAsync($"http://localhost{path}");

        var content = response.Content;

        if (typeof(T) == typeof(string))
        {
            object result = await content.ReadAsStringAsync();
            return (T)result;
        }

        return await content.ReadFromJsonAsync<T>();
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _client.Dispose();

        _isDisposed = true;
        GC.SuppressFinalize(this);
    }

    //! not needed at all:
    // ~UdsHttpClient()
    // {
    //     this.Dispose();
    // }
}