namespace SistemaEleicao.Core.Excecoes;

/// <summary>Lançada ao tentar registrar um eleitor cujo título já foi cadastrado.</summary>
public class EleitorDuplicadoException : Exception
{
    public string Titulo { get; }

    public EleitorDuplicadoException(string titulo)
        : base($"O eleitor com o título '{titulo}' já está registrado.")
    {
        Titulo = titulo;
    }
}
