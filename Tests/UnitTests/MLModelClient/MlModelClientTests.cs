using System.Net;
using Domain.DTOs;
using Microsoft.Extensions.Logging;
using ML_Model;
using Moq;
using Moq.Protected;

namespace Tests.UnitTests.MLModelClient;

public class MlHttpClientTests
{
    [Fact]
    public async Task PredictNextWateringTimeAsync_ReturnsPredictionResult()
    {
        var expectedResult = new PredictionResultDto
        {
            PredictionTime = DateTime.UtcNow,
            HoursUntilNextWatering = 4
        };
        var responseJson = System.Text.Json.JsonSerializer.Serialize(expectedResult);

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson)
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost/")
        };

        var loggerMock = new Mock<ILogger<MlHttpClient>>();
        var client = new MlHttpClient(httpClient, loggerMock.Object);

        var input = new MlModelDataDto();
        
        var result = await client.PredictNextWateringTimeAsync(input);

        Assert.NotNull(result);
        Assert.Equal(expectedResult.HoursUntilNextWatering, result.HoursUntilNextWatering);
    }
}