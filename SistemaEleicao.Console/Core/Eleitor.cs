namespace SistemaEleicao.Core;

/// <summary>
/// Representa um eleitor apto a votar, identificado pelo número do título (simulado).
/// </summary>
public class Eleitor
{
    public string Titulo { get; }

    /// <summary>Indica se o eleitor já registrou seu voto nesta eleição.</summary>
    public bool JaVotou { get; internal set; }

    public Eleitor(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("O título do eleitor não pode ser nulo ou vazio.", nameof(titulo));

        Titulo = titulo.Trim();
    }

    public override string ToString() => JaVotou ? $"{Titulo} (já votou)" : Titulo;
}
