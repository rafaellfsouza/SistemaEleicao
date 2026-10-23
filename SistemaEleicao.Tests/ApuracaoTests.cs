using SistemaEleicao.Core;

namespace SistemaEleicao.Tests;

/// <summary>
/// Etapa 2 - Testes unitários da apuração: total de votos por candidato,
/// votos nulos, votos brancos e definição do vencedor.
/// </summary>
public class ApuracaoTests
{
    private readonly EleicaoManager _manager = new();

    private EleicaoManager ManagerComCenarioCompleto()
    {
        _manager.CadastrarCandidato("Ana Souza", 10);
        _manager.CadastrarCandidato("Bruno Lima", 20);
        _manager.CadastrarCandidato("Carla Dias", 30);
        _manager.RegistrarEleitor("111");
        _manager.RegistrarEleitor("222");
        _manager.RegistrarEleitor("333");
        _manager.RegistrarEleitor("444");
        _manager.RegistrarEleitor("555");
        _manager.RegistrarEleitor("666");
        return _manager;
    }

    [Fact]
    public void Apurar_ContaVotosPorCandidato()
    {
        var manager = ManagerComCenarioCompleto();
        manager.Votar("111", 10);
        manager.Votar("222", 10);
        manager.Votar("333", 20);

        var resultado = manager.Apurar();

        var ana = manager.BuscarCandidatoPorNumero(10)!;
        var bruno = manager.BuscarCandidatoPorNumero(20)!;
        var carla = manager.BuscarCandidatoPorNumero(30)!;
        Assert.Equal(2, resultado.VotosPorCandidato[ana]);
        Assert.Equal(1, resultado.VotosPorCandidato[bruno]);
        Assert.Equal(0, resultado.VotosPorCandidato[carla]); // candidato sem voto aparece com zero
    }

    [Fact]
    public void Apurar_ContabilizaNulosEBrancosSeparadamente()
    {
        var manager = ManagerComCenarioCompleto();
        manager.Votar("111", 10);
        manager.Votar("222", 99);  // candidato inexistente -> nulo
        manager.Votar("333", 98);  // candidato inexistente -> nulo
        manager.Votar("444", 0);   // branco

        var resultado = manager.Apurar();

        Assert.Equal(2, resultado.VotosNulos);
        Assert.Equal(1, resultado.VotosBrancos);
    }

    [Fact]
    public void Apurar_TotalDeVotosSomaCandidatosNulosEBrancos()
    {
        var manager = ManagerComCenarioCompleto();
        manager.Votar("111", 10);
        manager.Votar("222", 20);
        manager.Votar("333", 99);
        manager.Votar("444", 0);

        var resultado = manager.Apurar();

        Assert.Equal(4, resultado.TotalVotos);
        Assert.Equal(
            resultado.TotalVotos,
            resultado.VotosPorCandidato.Values.Sum() + resultado.VotosNulos + resultado.VotosBrancos);
    }

    [Fact]
    public void Apurar_RetornaVencedorComMaisVotos()
    {
        var manager = ManagerComCenarioCompleto();
        manager.Votar("111", 10);
        manager.Votar("222", 10);
        manager.Votar("333", 20);

        var resultado = manager.Apurar();

        Assert.NotNull(resultado.Vencedor);
        Assert.Equal("Ana Souza", resultado.Vencedor!.Nome);
        Assert.False(resultado.HouveEmpate);
    }

    [Fact]
    public void Apurar_Empate_NaoDefineVencedor()
    {
        var manager = ManagerComCenarioCompleto();
        manager.Votar("111", 10);
        manager.Votar("222", 20);

        var resultado = manager.Apurar();

        Assert.Null(resultado.Vencedor);
        Assert.True(resultado.HouveEmpate);
    }

    [Fact]
    public void Apurar_SemVotos_TotalZeroEVencedorNulo()
    {
        var manager = ManagerComCenarioCompleto();

        var resultado = manager.Apurar();

        Assert.Equal(0, resultado.TotalVotos);
        Assert.Null(resultado.Vencedor);
        Assert.False(resultado.HouveEmpate);
    }
}
