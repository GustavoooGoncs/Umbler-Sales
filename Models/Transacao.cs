using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
    public class Transacao
    {
        [Key]
        public required string Id { get; set; }
        public required string Cartao { get; set; }
        public required decimal Valor { get; set; }
        public required string Status { get; set; }
        public required string Holder { get; set; }
        public required string ExpirationDate { get; set; }
        public required string SecurityCode { get; set; }
        public required string Brand { get; set; }
        // Construtor que inicializa todos os membros requeridos
        public Transacao(string id, string cartao, decimal valor, string status, string holder, string expirationDate, string securityCode, string brand)
        {
            Id = id;
            Cartao = cartao;
            Valor = valor;
            Status = status;
            Holder = holder;
            ExpirationDate = expirationDate;
            SecurityCode = securityCode;
            Brand = brand;
        }

        // Construtor padrão para inicialização sem parâmetros
        public Transacao()
        {
            Id = string.Empty;
            Cartao = string.Empty;
            Valor = 0;
            Status = string.Empty;
            Holder = string.Empty;
            ExpirationDate = string.Empty;
            SecurityCode = string.Empty;
            Brand = string.Empty;
        }
    }
}