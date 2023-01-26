using BankApi.Repositories.Contracts;
using Desafio.Models;
using Moq;

namespace BankApi.Tests;

public class TransacaoTest
{
    ITransacaoRepository _transacaoRepository;
    Mock<ITransacaoRepository> _transacaoRepositoryMock;
    User enviante;
    User recebedor;
    Transacao transacaoEx;

    public TransacaoTest()
    {
        _transacaoRepositoryMock = new Mock<ITransacaoRepository>();
        _transacaoRepository = _transacaoRepositoryMock.Object;

        enviante = new User
        {
            Id = 1,
            Nome = "Test",
            Sobrenome = "Test",
            Cpf = "00000000000",
            Senha = "password",
            SaldoInicial = 6000,
            CriadoEm = DateTime.Now
        };

        recebedor = new User
        {
            Id = 2,
            Nome = "Example",
            Sobrenome = "Example",
            Cpf = "11111111111",
            Senha = "password",
            SaldoInicial = 6000,
            CriadoEm = DateTime.Now
        };

        transacaoEx = new Transacao
        {
            Id = 1,
            ContaEnviante = enviante.Id,
            ContaRecebedora = recebedor.Id,
            DataDeProcessamento = DateTime.Now,
            PodeSerEstornada = true,
            Valor = 100
        };
    }

    [Fact(DisplayName = "Pegar transação por ID deve retornar a transação")]
    public void PegarTransacao_RetornarTransacao()
    {
        var expected = transacaoEx;

        _transacaoRepositoryMock.Setup(x => x.GetTransacao(transacaoEx.Id)).Returns(transacaoEx);

        var result = _transacaoRepository.GetTransacao(expected.Id);

        Assert.Equal(expected, result);
    }
}
