namespace SistemaEleicao.Core;

/// <summary>
/// Resultado final da apuração: votos por candidato, nulos, brancos,
/// total de votos e o vencedor da eleição (quando houver).
/// </summary>
public class ResultadoEleicao
{
    /// <summary>Total de votos nominais de cada candidato (inclui candidatos com zero voto).</summary>
    public IReadOnlyDictionary<Candidato, int> VotosPorCandidato { get; }

    public int VotosBrancos { get; }
    public int VotosNulos { get; }
    public int TotalVotos { get; }

    /// <summary>Candidato vencedor. Null quando não há votos válidos ou quando há empate.</summary>
    public Candidato? Vencedor { get; }

    /// <summary>True quando dois ou mais candidatos terminam empatados na liderança.</summary>
    public bool HouveEmpate { get; }

    public ResultadoEleicao(
        IReadOnlyDictionary<Candidato, int> votosPorCandidato,
        int votosBrancos,
        int votosNulos,
        int totalVotos,
        Candidato? vencedor,
        bool houveEmpate)
    {
        VotosPorCandidato = votosPorCandidato;
        VotosBrancos = votosBrancos;
        VotosNulos = votosNulos;
        TotalVotos = totalVotos;
        Vencedor = vencedor;
        HouveEmpate = houveEmpate;
    }
}
