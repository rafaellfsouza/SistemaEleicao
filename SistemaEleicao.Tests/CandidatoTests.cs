using SistemaEleicao.Core;
using SistemaEleicao.Core.Excecoes;

namespace SistemaEleicao.Tests;

/// <summary>
/// Etapa 1 - Testes unitários das regras de cadastro de candidatos.
/// Regra principal: não é possível cadastrar dois candidatos com o mesmo número.
/// </summary>
public class CandidatoTests
{
    private readonly EleicaoManager _manager = new();

    [Fact]
    public void CadastrarCandidato_DadosValidos_RegistraCandidato()
    {
        var candidato = _manager.CadastrarCandidato("Ana Souza", 10);

        Assert.Equal(10, candidato.Numero);
        Assert.Equal("Ana Souza", candidato.Nome);
        Assert.Single(_manager.Candidatos);
    }

    [Fact]
    public void CadastrarCandidato_NumeroDuplicado_LancaCandidatoDuplicadoException()
    {
        _manager.CadastrarCandidato("Ana Souza", 10);

        var excecao = Assert.Throws<CandidatoDuplicadoException>(
            () => _manager.CadastrarCandidato("Bruno Lima", 10));

        Assert.Equal(10, excecao.Numero);
        Assert.Single(_manager.Candidatos); // o duplicado não pode ter sido registrado
    }

    [Fact]
    public void CadastrarCandidato_NumerosDiferentes_RegistraTodos()
    {
        _manager.CadastrarCandidato("Ana Souza", 10);
        _manager.CadastrarCandidato("Bruno Lima", 20);
        _manager.CadastrarCandidato("Carla Dias", 30);

        Assert.Equal(3, _manager.Candidatos.Count);
    }

    [Fact]
    public void CadastrarCandidato_MesmoNomeNumerosDiferentes_PermiteRegistro()
    {
        _manager.CadastrarCandidato("Ana Souza", 10);
        _manager.CadastrarCandidato("Ana Souza", 20);

        Assert.Equal(2, _manager.Candidatos.Count);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void CadastrarCandidato_NomeInvalido_LancaArgumentException(string? nome)
    {
        Assert.Throws<ArgumentException>(() => _manager.CadastrarCandidato(nome!, 10));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void CadastrarCandidato_NumeroNaoPositivo_LancaArgumentOutOfRangeException(int numero)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _manager.CadastrarCandidato("Ana Souza", numero));
    }

    [Fact]
    public void BuscarCandidatoPorNumero_Existente_RetornaCandidato()
    {
        _manager.CadastrarCandidato("Ana Souza", 10);

        var encontrado = _manager.BuscarCandidatoPorNumero(10);

        Assert.NotNull(encontrado);
        Assert.Equal("Ana Souza", encontrado!.Nome);
    }

    [Fact]
    public void BuscarCandidatoPorNumero_Inexistente_RetornaNull()
    {
        Assert.Null(_manager.BuscarCandidatoPorNumero(99));
    }

    [Fact]
    public void CadastrarCandidato_NomeComEspacosExtras_RegistraNomeTrimmed()
    {
        var candidato = _manager.CadastrarCandidato("  Ana Souza  ", 10);

        Assert.Equal("Ana Souza", candidato.Nome);
    }
}
