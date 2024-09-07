# Sistema de Transações de Cartão de Crédito

## Descrição
Este projeto é um sistema de transações de cartão de crédito que permite aos usuários criar, capturar e cancelar transações através de uma interface web. A aplicação interage com o Gateway de pagamentos Cielo, utilizando o ambiente sandbox disponibilizado pela Cielo para testes.

## Tecnologias Utilizadas
- C#
- ASP.NET Core
- Blazor (server-side)

## Funcionalidades
- **Criação de Transações**: Inserção de detalhes do cartão e valor.
- **Captura de Transações**: Confirmação manual de transações autorizadas.
- **Cancelamento de Transações**: Reversão de transações autorizadas.

## Regras de Negócio
- **Cartões Aceitos**: Apenas cartões das bandeiras Visa e Mastercard são aceitos.
  - **Visa**: Deve começar com '4', ter 13 ou 16 dígitos, e um CVC de 3 dígitos.
  - **Mastercard**: Deve começar com '5', ter 16 dígitos, e um CVC de 3 dígitos.

## Instalação e Execução
### Pré-requisitos
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

### Passos para Execução
1. Clone o repositório:
    ```bash
    git clone https://github.com/GustavoooGoncs/Umbler-Sales.git
    cd Umbler-Sales
    ```

2. Execute a aplicação:
    ```bash
    dotnet watch run
    ```

3. Acesse a aplicação no navegador:
    ```
    http://localhost:5000
    ```

## Estrutura do Projeto
- **Pages**: Contém as páginas Blazor da aplicação.
  - [`Transacoes.razor`] Página principal para gerenciar transações.
- **Services**: Contém os serviços para interação com a API da Cielo.
  - [`TransacaoService.cs`] Serviço para operações de transações.
- **Models**: Contém as classes de modelo da aplicação.
  - [`Transacao.cs`] Modelo de dados para transações.
