using System.Net.Http.Json;
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
            logger.LogInformation("Sending prediction request. Data: {@PreparedData}", preparedData);

            var response = await httpClient.PostAsJsonAsync("api/ml/predict", preparedData);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PredictionResultDto>();

            if (result == null)
            {
                logger.LogError("ML service returned a null prediction.");
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
        catch (NotSupportedException ex)
        {
            logger.LogError(ex, "Unsupported content type from ML service.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during ML prediction.");
            throw;
        }
    }
}