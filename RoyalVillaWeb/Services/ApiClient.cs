using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using RoyalVillaWeb.Models;

namespace RoyalVillaWeb.Services;

public sealed class ApiClient(HttpClient http, IHttpContextAccessor accessor, ILogger<ApiClient> logger) : IApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<(IReadOnlyList<VillaDto> Items, string? Error)> GetVillasAsync()
    {
        var (data, error) = await GetResultAsync<List<VillaDto>>("api/Villa");
        return (data ?? [], error);
    }

    public Task<(VillaDto? Item, string? Error)> GetVillaAsync(int id)
        => GetResultAsync<VillaDto>($"api/Villa/{id}");

    public async Task<(IReadOnlyList<AmenityDto> Items, string? Error)> GetAmenitiesAsync(int villaId)
    {
        var (data, error) = await GetResultAsync<List<AmenityDto>>($"api/VillaAmenities?villaId={villaId}");
        return (data ?? [], error);
    }

    public async Task<(bool Success, string? Error)> CreateVillaAsync(VillaFormViewModel villa)
    {
        using var request = Authorized(new HttpRequestMessage(HttpMethod.Post, "api/Villa")
        {
            Content = JsonContent.Create(ToPayload(villa))
        });
        return await SendWriteAsync(request);
    }

    public async Task<(bool Success, string? Error)> UpdateVillaAsync(VillaFormViewModel villa)
    {
        using var request = Authorized(new HttpRequestMessage(HttpMethod.Put, $"api/Villa/{villa.Id}")
        {
            Content = JsonContent.Create(ToPayload(villa))
        });
        return await SendWriteAsync(request);
    }

    public async Task<(bool Success, string? Error)> DeleteVillaAsync(int id)
    {
        using var request = Authorized(new HttpRequestMessage(HttpMethod.Delete, $"api/Villa/{id}"));
        return await SendWriteAsync(request);
    }

    public async Task<(string? Token, string? Error)> LoginAsync(string email, string password)
    {
        try
        {
            using var response = await http.PostAsJsonAsync("api/Auth/Login", new { email, password });
            if (!response.IsSuccessStatusCode)
                return (null, await ReadErrorAsync(response));
            var payload = await response.Content.ReadFromJsonAsync<TokenResponse>(JsonOptions);
            return (payload?.Token, payload?.Token is null ? "The API did not return an access token." : null);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            logger.LogWarning(ex, "Villa API login request failed");
            return (null, "The villa service is unavailable. Please try again shortly.");
        }
    }

    public async Task<(bool Success, string? Error)> RegisterAsync(RegisterViewModel account)
    {
        try
        {
            using var response = await http.PostAsJsonAsync("api/Auth/Register", new
            {
                firstName = account.FirstName,
                lastName = account.LastName,
                username = account.Username,
                email = account.Email,
                password = account.Password,
                confirmPassword = account.ConfirmPassword,
                role = "User"
            });
            return response.IsSuccessStatusCode ? (true, null) : (false, await ReadErrorAsync(response));
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            logger.LogWarning(ex, "Villa API registration request failed");
            return (false, "The villa service is unavailable. Please try again shortly.");
        }
    }

    private async Task<(T? Data, string? Error)> GetResultAsync<T>(string path)
    {
        try
        {
            using var response = await http.GetAsync(path);
            if (!response.IsSuccessStatusCode) return (default, await ReadErrorAsync(response));
            var envelope = await response.Content.ReadFromJsonAsync<ApiEnvelope<T>>(JsonOptions);
            return (envelope is null ? default : envelope.Result, envelope is { IsSuccess: false } ? envelope.Message : null);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            logger.LogWarning(ex, "Villa API request to {Path} failed", path);
            return (default, "Villa listings are temporarily unavailable. Please try again shortly.");
        }
    }

    private async Task<(bool Success, string? Error)> SendWriteAsync(HttpRequestMessage request)
    {
        try
        {
            using var response = await http.SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            return (false, await ReadErrorAsync(response));
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            logger.LogWarning(ex, "Villa API write request failed");
            return (false, "The villa service is unavailable. Please try again shortly.");
        }
    }

    private HttpRequestMessage Authorized(HttpRequestMessage request)
    {
        var token = accessor.HttpContext?.Session.GetString("ApiToken");
        if (!string.IsNullOrWhiteSpace(token)) request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return request;
    }

    private static object ToPayload(VillaFormViewModel villa) => new
    {
        villa.Name, villa.Details, villa.Rate, villa.Sqft, villa.Occupancy, villa.ImageUrl
    };

    private static async Task<string> ReadErrorAsync(HttpResponseMessage response)
    {
        try
        {
            var text = await response.Content.ReadAsStringAsync();
            using var json = JsonDocument.Parse(text);
            if (json.RootElement.ValueKind == JsonValueKind.Array)
                return string.Join(" ", json.RootElement.EnumerateArray().Select(x => x.GetString()).Where(x => !string.IsNullOrWhiteSpace(x)));
            if (json.RootElement.TryGetProperty("message", out var message) && message.ValueKind == JsonValueKind.String)
                return message.GetString() ?? "The request could not be completed.";
            if (json.RootElement.TryGetProperty("errors", out var errors))
            {
                if (errors.ValueKind == JsonValueKind.Array)
                    return string.Join(" ", errors.EnumerateArray().Select(x => x.GetString()).Where(x => !string.IsNullOrWhiteSpace(x)));
                if (errors.ValueKind == JsonValueKind.Object)
                    return string.Join(" ", errors.EnumerateObject().SelectMany(x => x.Value.EnumerateArray().Select(v => v.GetString())).Where(x => !string.IsNullOrWhiteSpace(x)));
            }
        }
        catch (JsonException) { }
        return response.StatusCode switch
        {
            System.Net.HttpStatusCode.Unauthorized => "Your session has expired. Please sign in again.",
            System.Net.HttpStatusCode.Forbidden => "Your account does not have permission to manage villas.",
            System.Net.HttpStatusCode.NotFound => "The requested villa could not be found.",
            System.Net.HttpStatusCode.Conflict => "A villa with that name already exists.",
            _ => "The request could not be completed. Please try again."
        };
    }

    private sealed class ApiEnvelope<T>
    {
        public bool IsSuccess { get; init; }
        public string? Message { get; init; }
        public T? Result { get; init; }
    }

    private sealed class TokenResponse { public string? Token { get; init; } }
}
