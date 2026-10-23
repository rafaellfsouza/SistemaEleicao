namespace SistemaEleicao.Core.Excecoes;

/// <summary>Lançada quando um eleitor não registrado tenta votar.</summary>
public class EleitorNaoRegistradoException : Exception
{
    public string Titulo { get; }

    public EleitorNaoRegistradoException(string titulo)
        : base($"O eleitor com o título '{titulo}' não está registrado. Registre-o antes de votar.")
    {
        Titulo = titulo;
    }
}
