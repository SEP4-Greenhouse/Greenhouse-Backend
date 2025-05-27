using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.IClients;
using Microsoft.Extensions.Logging;

namespace ML_Model;

public class MlHttpClient(HttpClient httpClient, ILogger<MlHttpClient> logger) : IMlHttpClient
{
    public async Task<PredictionResultDto> PredictNextWateringTimeAsync(MlModelDataDto preparedData)
    {
        try
        {
            var jsonPayload = JsonSerializer.Serialize(preparedData, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            logger.LogInformation("Sending JSON payload to MlConnection service:\n{JsonPayload}", jsonPayload);

            using var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/ml/predict", content);

            logger.LogInformation("MlConnection service responded with status: {StatusCode}", response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            logger.LogInformation("Raw MlConnection service response: {ResponseContent}", responseContent);

            response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<PredictionResultDto>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result == null)
            {
                logger.LogError("MlConnection service returned null prediction. Raw response: {ResponseContent}", responseContent);
                throw new Exception("MlConnection service returned null prediction.");
            }

            logger.LogInformation("Received prediction result: {@Result}", result);
            return result;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP request to MlConnection service failed.");
            throw;
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Failed to deserialize MlConnection service response.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during MlConnection prediction.");
            throw;
        }
    }
}
