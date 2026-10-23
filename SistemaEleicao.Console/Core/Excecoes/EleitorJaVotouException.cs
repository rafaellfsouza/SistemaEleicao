namespace SistemaEleicao.Core.Excecoes;

/// <summary>Lançada quando um eleitor tenta votar mais de uma vez.</summary>
public class EleitorJaVotouException : Exception
{
    public string Titulo { get; }

    public EleitorJaVotouException(string titulo)
        : base($"O eleitor com o título '{titulo}' já votou nesta eleição.")
    {
        Titulo = titulo;
    }
}
