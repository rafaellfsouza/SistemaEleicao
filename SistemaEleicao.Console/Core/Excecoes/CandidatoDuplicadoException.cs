namespace SistemaEleicao.Core.Excecoes;

/// <summary>
/// Lançada ao tentar cadastrar um candidato com número já utilizado por outro candidato.
/// </summary>
public class CandidatoDuplicadoException : Exception
{
    public int Numero { get; }

    public CandidatoDuplicadoException(int numero)
        : base($"Já existe um candidato cadastrado com o número {numero}.")
    {
        Numero = numero;
    }
}
