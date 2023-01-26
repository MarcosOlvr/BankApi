using BankApi.Repositories.Contracts;
using Desafio.Models;
using Moq;

namespace BankApi.Tests;

public class UserTest
{
    IUserRepository _userRepository;
    Mock<IUserRepository> _userRepositoryMock;
    User user;

    public UserTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userRepository = _userRepositoryMock.Object;

        user = new User
        {
            Id = 1,
            Nome = "Test",
            Sobrenome = "Test",
            Cpf = "00000000000",
            Senha = "password",
            SaldoInicial = 6000,
            CriadoEm = DateTime.Now
        };
    }

    [Fact(DisplayName = "Pegar usuário deve retornar o usuário")]
    public void GetUserById_ReturnUser()
    {
        var expected = user;
        _userRepositoryMock.Setup(x => x.GetUser(user.Id)).Returns(expected);

        var result = _userRepository.GetUser(user.Id);

        Assert.Equal(expected, result);
    }

    [Fact(DisplayName = "Pegar usuário deve retornar o usuário")]
    public void GetUserByCpf_ReturnUser()
    {
        var expected = user;
        _userRepositoryMock.Setup(x => x.GetUser(user.Cpf)).Returns(expected);

        var result = _userRepository.GetUser(user.Cpf);

        Assert.Equal(expected, result);
    }
}