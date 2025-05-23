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

            logger.LogInformation("Sending JSON payload to ML service:\n{JsonPayload}", jsonPayload);

            using var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/ml/predict", content);

            logger.LogInformation("ML service responded with status: {StatusCode}", response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            logger.LogInformation("Raw ML service response: {ResponseContent}", responseContent);

            response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<PredictionResultDto>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result == null)
            {
                logger.LogError("ML service returned null prediction. Raw response: {ResponseContent}", responseContent);
                throw new Exception("ML service returned null prediction.");
            }

            logger.LogInformation("Received prediction result: {@Result}", result);
            return result;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP request to ML service failed.");
            throw;
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Failed to deserialize ML service response.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during ML prediction.");
            throw;
        }
    }
}
