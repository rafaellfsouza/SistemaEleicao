using SistemaEleicao.Core.Excecoes;

namespace SistemaEleicao.Core;

/// <summary>
/// Gerencia a eleição: cadastro de candidatos, registro de eleitores,
/// votação e apuração dos votos.
/// </summary>
public class EleicaoManager
{
    /// <summary>
    /// Número que representa o voto em branco na urna.
    /// Qualquer outro número que não corresponda a um candidato gera voto nulo.
    /// </summary>
    public const int NumeroVotoBranco = 0;

    private readonly Dictionary<int, Candidato> _candidatos = new();
    private readonly Dictionary<string, Eleitor> _eleitores = new(StringComparer.Ordinal);
    private readonly List<Voto> _votos = new();

    /// <summary>Candidatos cadastrados até o momento.</summary>
    public IReadOnlyCollection<Candidato> Candidatos => _candidatos.Values.ToList().AsReadOnly();

    /// <summary>Eleitores registrados até o momento.</summary>
    public IReadOnlyCollection<Eleitor> Eleitores => _eleitores.Values.ToList().AsReadOnly();

    /// <summary>Votos já computados (nominais, brancos e nulos).</summary>
    public IReadOnlyCollection<Voto> Votos => _votos.ToList().AsReadOnly();

    /// <summary>Total de votos computados até o momento.</summary>
    public int TotalVotosRegistrados => _votos.Count;

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

    /// <summary>
    /// Registra um eleitor apto a votar.
    /// </summary>
    /// <exception cref="EleitorDuplicadoException">Lançada quando o título já foi registrado.</exception>
    /// <exception cref="ArgumentException">Lançada quando o título é nulo ou vazio.</exception>
    public Eleitor RegistrarEleitor(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("O título do eleitor não pode ser nulo ou vazio.", nameof(titulo));

        titulo = titulo.Trim();
        if (_eleitores.ContainsKey(titulo))
            throw new EleitorDuplicadoException(titulo);

        var eleitor = new Eleitor(titulo);
        _eleitores.Add(titulo, eleitor);
        return eleitor;
    }

    /// <summary>
    /// Registra o voto de um eleitor.
    /// Número 0 (<see cref="NumeroVotoBranco"/>) computa voto em branco;
    /// número de candidato inexistente computa voto nulo;
    /// número válido computa voto nominal.
    /// </summary>
    /// <exception cref="EleitorNaoRegistradoException">Lançada quando o eleitor não está cadastrado.</exception>
    /// <exception cref="EleitorJaVotouException">Lançada quando o eleitor tenta votar duas vezes.</exception>
    /// <exception cref="ArgumentException">Lançada quando o título é nulo ou vazio.</exception>
    public Voto Votar(string tituloEleitor, int numeroCandidato)
    {
        if (string.IsNullOrWhiteSpace(tituloEleitor))
            throw new ArgumentException("O título do eleitor não pode ser nulo ou vazio.", nameof(tituloEleitor));

        if (!_eleitores.TryGetValue(tituloEleitor.Trim(), out var eleitor))
            throw new EleitorNaoRegistradoException(tituloEleitor);

        if (eleitor.JaVotou)
            throw new EleitorJaVotouException(eleitor.Titulo);

        Voto voto;
        if (numeroCandidato == NumeroVotoBranco)
        {
            voto = new Voto(eleitor, TipoVoto.Branco);
        }
        else if (_candidatos.TryGetValue(numeroCandidato, out var candidato))
        {
            voto = new Voto(eleitor, TipoVoto.Candidato, candidato);
        }
        else
        {
            // requisito: voto em candidato inexistente é contabilizado como nulo
            voto = new Voto(eleitor, TipoVoto.Nulo);
        }

        _votos.Add(voto);
        eleitor.JaVotou = true;
        return voto;
    }

    /// <summary>
    /// Apura a eleição e gera o relatório final: total de votos por candidato,
    /// votos nulos, votos brancos e o candidato vencedor.
    /// </summary>
    public ResultadoEleicao Apurar()
    {
        var votosPorCandidato = _candidatos.Values.ToDictionary(candidato => candidato, _ => 0);
        var brancos = 0;
        var nulos = 0;

        foreach (var voto in _votos)
        {
            switch (voto.Tipo)
            {
                case TipoVoto.Branco:
                    brancos++;
                    break;
                case TipoVoto.Nulo:
                    nulos++;
                    break;
                case TipoVoto.Candidato:
                    votosPorCandidato[voto.Candidato!]++;
                    break;
            }
        }

        Candidato? vencedor = null;
        var houveEmpate = false;

        if (votosPorCandidato.Count > 0)
        {
            var maiorQtde = votosPorCandidato.Values.Max();
            if (maiorQtde > 0)
            {
                var lideres = votosPorCandidato
                    .Where(par => par.Value == maiorQtde)
                    .Select(par => par.Key)
                    .ToList();

                if (lideres.Count == 1)
                    vencedor = lideres[0];
                else
                    houveEmpate = true;
            }
        }

        return new ResultadoEleicao(votosPorCandidato, brancos, nulos, _votos.Count, vencedor, houveEmpate);
    }
}
