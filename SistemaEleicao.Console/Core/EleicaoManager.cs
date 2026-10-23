using SistemaEleicao.Core.Excecoes;

namespace SistemaEleicao.Core;

/// <summary>
/// Gerencia a eleição: cadastro de candidatos, registro de eleitores,
/// votação e apuração dos votos.
/// </summary>
public class EleicaoManager
{
    private readonly Dictionary<int, Candidato> _candidatos = new();

    /// <summary>Candidatos cadastrados até o momento.</summary>
    public IReadOnlyCollection<Candidato> Candidatos => _candidatos.Values.ToList().AsReadOnly();

    /// <summary>
    /// Cadastra um candidato na eleição.
    /// </summary>
    /// <exception cref="CandidatoDuplicadoException">
    /// Lançada quando já existe outro candidato com o mesmo número.
    /// </exception>
    /// <exception cref="ArgumentException">Lançada quando o nome é nulo ou vazio.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Lançada quando o número não é positivo.</exception>
    public Candidato CadastrarCandidato(string nome, int numero)
    {
        if (_candidatos.ContainsKey(numero))
            throw new CandidatoDuplicadoException(numero);

        var candidato = new Candidato(numero, nome);
        _candidatos.Add(numero, candidato);
        return candidato;
    }

    /// <summary>
    /// Busca um candidato pelo número de urna. Retorna null quando não encontrado.
    /// </summary>
    public Candidato? BuscarCandidatoPorNumero(int numero) =>
        _candidatos.TryGetValue(numero, out var candidato) ? candidato : null;
}
