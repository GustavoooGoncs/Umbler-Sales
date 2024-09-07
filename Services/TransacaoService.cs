using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using MyProject.Models;

public class PaymentRequest
{
    public required string MerchantOrderId { get; set; }
    public required PaymentDetails Payment { get; set; }
}

public class PaymentDetails
{
    public required string Type { get; set; }
    public required decimal Amount { get; set; }
    public required int Installments { get; set; }
    public required string SoftDescriptor { get; set; }
    public required CreditCardDetails CreditCard { get; set; }
}

public class CreditCardDetails
{
    public required string CardNumber { get; set; }
    public required string Holder { get; set; }
    public required string ExpirationDate { get; set; }
    public required string SecurityCode { get; set; }
    public required string Brand { get; set; }
}

public class Transacao
{
    public string Id { get; set; } = string.Empty;
    public string Cartao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Holder { get; set; } = string.Empty;
    public string ExpirationDate { get; set; } = string.Empty;
    public string SecurityCode { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
}

public class Customer
{
    public required string Name { get; set; }
    public required Address Address { get; set; }
}

public class Address
{
    // Adicione as propriedades necessárias para o endereço
}

public class CreditCard
{
    public required string CardNumber { get; set; }
    public required string Holder { get; set; }
    public required string ExpirationDate { get; set; }
    public required string Brand { get; set; }
    public required string PaymentAccountReference { get; set; }
}

public class Link
{
    public required string Method { get; set; }
    public required string Rel { get; set; }
    public required string Href { get; set; }
}

public class Payment
{
    public required int ServiceTaxAmount { get; set; }
    public required int Installments { get; set; }
    public required string Interest { get; set; }
    public required bool Capture { get; set; }
    public required bool Authenticate { get; set; }
    public required bool Recurrent { get; set; }
    public required CreditCard CreditCard { get; set; }
    public required string ProofOfSale { get; set; }
    public required string Tid { get; set; }
    public required string AuthorizationCode { get; set; }
    public required string PaymentId { get; set; }
    public required string Type { get; set; }
    public required int Amount { get; set; }
    public required string ReceivedDate { get; set; }
    public required string Currency { get; set; }
    public required string Country { get; set; }
    public required string Provider { get; set; }
    public required int Status { get; set; }
    public required List<Link> Links { get; set; }
}

public class TransacaoResponse
{
    public required string MerchantOrderId { get; set; }
    public required Customer Customer { get; set; }
    public required Payment Payment { get; set; }
}

public class TransacaoService
{
    private readonly HttpClient _httpClient;

    public TransacaoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<MyProject.Models.Transacao> CriarTransacaoCartaoCreditoAsync(MyProject.Models.Transacao transacao)
    {
        var requestBody = new PaymentRequest
        {
            MerchantOrderId = "2014111703",
            Payment = new PaymentDetails
            {
                Type = "CreditCard",
                Amount = transacao.Valor,
                Installments = 1,
                SoftDescriptor = "123456789ABCD",
                CreditCard = new CreditCardDetails
                {
                    CardNumber = transacao.Cartao,
                    Holder = transacao.Holder,
                    ExpirationDate = transacao.ExpirationDate,
                    SecurityCode = transacao.SecurityCode,
                    Brand = transacao.Brand
                }
            }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "https://apisandbox.cieloecommerce.cielo.com.br/1/sales")
        {
            Content = JsonContent.Create(requestBody)
        };
        request.Headers.Add("MerchantId", "c0d7d51b-918b-4703-8ef3-227fa39ced75");
        request.Headers.Add("MerchantKey", "CNUYAHXBYMNUUCGJGSEUKQPCCMXIXQXOOTECEZZE");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        try
        {
            var jsonDocument = JsonDocument.Parse(responseBody);
            var paymentElement = jsonDocument.RootElement.GetProperty("Payment");

            var createdTransacao = new MyProject.Models.Transacao
            {
                Id = paymentElement.GetProperty("PaymentId").GetString() ?? string.Empty,
                Cartao = transacao.Cartao,
                Valor = transacao.Valor,
                Status = paymentElement.GetProperty("Status").GetInt32().ToString(),
                Holder = transacao.Holder,
                ExpirationDate = transacao.ExpirationDate,
                SecurityCode = transacao.SecurityCode,
                Brand = transacao.Brand
            };

            return createdTransacao;
        }
        catch (JsonException)
        {
            throw new InvalidOperationException("A resposta da API não está no formato JSON esperado: " + responseBody);
        }
    }

    public async Task<MyProject.Models.Transacao[]> GetTransacoesAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://apiquerysandbox.cieloecommerce.cielo.com.br/1/sales/25984c1c-720d-42d3-9355-ac74a4b87921");
        request.Headers.Add("MerchantId", "c0d7d51b-918b-4703-8ef3-227fa39ced75");
        request.Headers.Add("MerchantKey", "CNUYAHXBYMNUUCGJGSEUKQPCCMXIXQXOOTECEZZE");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseBody); // Log do conteúdo da resposta JSON

        var transacoes = JsonSerializer.Deserialize<MyProject.Models.Transacao[]>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return transacoes ?? Array.Empty<MyProject.Models.Transacao>();
    }

    public async Task CancelTransacaoAsync(string id)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"https://api.exemplo.com/transacoes/{id}/cancel")
        {
            Content = JsonContent.Create(new { })
        };
        request.Headers.Add("MerchantId", "c0d7d51b-918b-4703-8ef3-227fa39ced75");
        request.Headers.Add("MerchantKey", "CNUYAHXBYMNUUCGJGSEUKQPCCMXIXQXOOTECEZZE");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task CaptureTransacaoAsync(string id)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"https://api.exemplo.com/transacoes/{id}/capture")
        {
            Content = JsonContent.Create(new { })
        };
        request.Headers.Add("MerchantId", "c0d7d51b-918b-4703-8ef3-227fa39ced75");
        request.Headers.Add("MerchantKey", "CNUYAHXBYMNUUCGJGSEUKQPCCMXIXQXOOTECEZZE");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}