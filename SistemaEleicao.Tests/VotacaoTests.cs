using SistemaEleicao.Core;
using SistemaEleicao.Core.Excecoes;

namespace SistemaEleicao.Tests;

/// <summary>
/// Etapa 2 - Testes unitários do controle de eleitores e da votação.
/// Regras: um eleitor não pode votar duas vezes, eleitor deve estar registrado
/// e voto em candidato inexistente é contabilizado como nulo.
/// </summary>
public class VotacaoTests
{
    private readonly EleicaoManager _manager = new();

    private EleicaoManager ManagerComCandidatosEEleitores()
    {
        _manager.CadastrarCandidato("Ana Souza", 10);
        _manager.CadastrarCandidato("Bruno Lima", 20);
        _manager.RegistrarEleitor("111.222.333-44");
        _manager.RegistrarEleitor("555.666.777-88");
        return _manager;
    }

    [Fact]
    public void RegistrarEleitor_TituloValido_RegistraComSucesso()
    {
        var eleitor = _manager.RegistrarEleitor("111.222.333-44");

        Assert.Equal("111.222.333-44", eleitor.Titulo);
        Assert.False(eleitor.JaVotou);
        Assert.Single(_manager.Eleitores);
    }

    [Fact]
    public void RegistrarEleitor_TituloDuplicado_LancaEleitorDuplicadoException()
    {
        _manager.RegistrarEleitor("111.222.333-44");

        Assert.Throws<EleitorDuplicadoException>(
            () => _manager.RegistrarEleitor("111.222.333-44"));
        Assert.Single(_manager.Eleitores);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void RegistrarEleitor_TituloInvalido_LancaArgumentException(string? titulo)
    {
        Assert.Throws<ArgumentException>(() => _manager.RegistrarEleitor(titulo!));
    }

    [Fact]
    public void Votar_EleitorNaoRegistrado_LancaEleitorNaoRegistradoException()
    {
        _manager.CadastrarCandidato("Ana Souza", 10);

        Assert.Throws<EleitorNaoRegistradoException>(
            () => _manager.Votar("999.999.999-99", 10));
        Assert.Equal(0, _manager.TotalVotosRegistrados);
    }

    [Fact]
    public void Votar_CandidatoExistente_RegistraVotoDoTipoCandidato()
    {
        var manager = ManagerComCandidatosEEleitores();

        var voto = manager.Votar("111.222.333-44", 10);

        Assert.Equal(TipoVoto.Candidato, voto.Tipo);
        Assert.NotNull(voto.Candidato);
        Assert.Equal(10, voto.Candidato!.Numero);
        Assert.Equal(1, manager.TotalVotosRegistrados);
    }

    [Fact]
    public void Votar_MesmoEleitorDuasVezes_LancaEleitorJaVotouException()
    {
        var manager = ManagerComCandidatosEEleitores();
        manager.Votar("111.222.333-44", 10);

        Assert.Throws<EleitorJaVotouException>(
            () => manager.Votar("111.222.333-44", 20));
        Assert.Equal(1, manager.TotalVotosRegistrados); // o segundo voto não pode ser computado
    }

    [Fact]
    public void Votar_EleitoresDiferentesMesmoCandidato_ComputaAmbosOsVotos()
    {
        var manager = ManagerComCandidatosEEleitores();

        manager.Votar("111.222.333-44", 10);
        manager.Votar("555.666.777-88", 10);

        Assert.Equal(2, manager.TotalVotosRegistrados);
    }

    [Fact]
    public void Votar_CandidatoInexistente_ComputaComoVotoNulo()
    {
        var manager = ManagerComCandidatosEEleitores();

        var voto = manager.Votar("111.222.333-44", 99);

        Assert.Equal(TipoVoto.Nulo, voto.Tipo);
        Assert.Null(voto.Candidato);
    }

    [Fact]
    public void Votar_NumeroZero_ComputaComoVotoBranco()
    {
        var manager = ManagerComCandidatosEEleitores();

        var voto = manager.Votar("111.222.333-44", EleicaoManager.NumeroVotoBranco);

        Assert.Equal(TipoVoto.Branco, voto.Tipo);
        Assert.Null(voto.Candidato);
    }

    [Fact]
    public void Eleitor_AposVotar_FicaMarcadoComoJaVotou()
    {
        var manager = ManagerComCandidatosEEleitores();
        var eleitor = manager.RegistrarEleitor("999.888.777-66");
        Assert.False(eleitor.JaVotou);

        manager.Votar("999.888.777-66", 20);

        Assert.True(eleitor.JaVotou);
    }
}
