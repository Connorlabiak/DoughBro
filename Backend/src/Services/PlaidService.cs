using DoughBro.src.Exceptions;
using DoughBro.src.Services.Interfaces;
using System.Text.Json;

namespace DoughBro.src.Services;

public class PlaidService : IPlaidService
{

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IUserService _userService;
    private readonly IConfiguration _config;

    public PlaidService(IHttpClientFactory httpClientFactory, IUserService userService, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _userService = userService;
        _config = configuration;
    }

    public async Task<string> CreateLinkTokenAsync(string userId)
    {
        HttpClient _httpClient = _httpClientFactory.CreateClient("PlaidHttpClient");

        var payload = new
        {
            client_id = _config["Plaid:ClientId"],
            secret = _config["Plaid:Secret"],
            client_name = "DoughBro",
            user = new { client_user_id = userId },
            products = new[] { "transactions" },
            country_codes = new[] { "US" },
            language = "en"
        };

        var response = await _httpClient.PostAsJsonAsync("/link/token/create", payload);
        string jsonString = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(jsonString))
        {
            throw new HttpRequestException($"API Error. Status: {response.StatusCode}, Content: {jsonString}");
        }

        using var doc = JsonDocument.Parse(jsonString);
        return doc.RootElement.GetProperty("link_token").GetString()!;
    }

    public async Task<string> ExchangePublicTokenAsync(string publicToken, string userId, string institutionName)
    {
        HttpClient _httpClient = _httpClientFactory.CreateClient("PlaidHttpClient");

        var payload = new
        {
            client_id = _config["Plaid:ClientId"],
            secret = _config["Plaid:Secret"],
            public_token = publicToken
        };

        var response = await _httpClient.PostAsJsonAsync("/item/public_token/exchange", payload);
        response.EnsureSuccessStatusCode();

        using var doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        var root = doc.RootElement;

        await _userService.SavePlaidAccessToken(new()
        {
            ItemId = root.GetProperty("item_id").GetString()!,
            Token = root.GetProperty("access_token").GetString()!,
            UserId = userId,
            InstitutionName = institutionName,
            CreatedAt = DateTime.UtcNow.ToString()
        });

        return root.GetProperty("item_id").GetString()!;
    }

    public async Task<JsonElement?> FetchTransactionsAsync(string accessToken, string? cursor = null)
    {
        HttpClient _httpClient = _httpClientFactory.CreateClient("PlaidHttpClient");

        var payload = new
        {
            client_id = _config["Plaid:ClientId"],
            secret = _config["Plaid:Secret"],
            access_token = accessToken,
            cursor = string.IsNullOrEmpty(cursor) ? null : cursor,
            count = 490
        };

        var response = await _httpClient.PostAsJsonAsync("/transactions/sync", payload);
        if (!response.IsSuccessStatusCode)
        {
            try
            {
                var errorJson = await response.Content.ReadFromJsonAsync<JsonElement>();

                string errorType = errorJson.GetProperty("error_type").GetString() ?? "UNKNOWN";
                string errorCode = errorJson.GetProperty("error_code").GetString() ?? "UNKNOWN";
                string errorMessage = errorJson.GetProperty("error_message").GetString() ?? "An unmapped error occurred.";

                throw new PlaidApiException(errorType, errorCode, errorMessage);
            }
            catch (JsonException)
            {
                throw new PlaidApiException("INTERNAL", "HTTP_FAILURE", $"Plaid returned raw network status: {response.StatusCode}");
            }
        }

        return await response.Content.ReadFromJsonAsync<JsonElement>();
    }
}
