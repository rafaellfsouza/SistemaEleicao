namespace SistemaEleicao.Core;

/// <summary>Tipo do voto registrado na urna.</summary>
public enum TipoVoto
{
    /// <summary>Voto em um candidato válido.</summary>
    Candidato,

    /// <summary>Voto em branco (número 0).</summary>
    Branco,

    /// <summary>Voto nulo (número de candidato inexistente).</summary>
    Nulo
}

/// <summary>
/// Representa um voto já validado: quem votou, o tipo do voto
/// e, quando nominal, o candidato escolhido.
/// </summary>
public class Voto
{
    public Eleitor Eleitor { get; }
    public TipoVoto Tipo { get; }
    public Candidato? Candidato { get; }

    public Voto(Eleitor eleitor, TipoVoto tipo, Candidato? candidato = null)
    {
        Eleitor = eleitor ?? throw new ArgumentNullException(nameof(eleitor));

        if (tipo == TipoVoto.Candidato && candidato is null)
            throw new ArgumentException("Voto do tipo Candidato exige um candidato.", nameof(candidato));

        if (tipo != TipoVoto.Candidato && candidato is not null)
            throw new ArgumentException("Votos brancos e nulos não podem ser associados a um candidato.", nameof(candidato));

        Tipo = tipo;
        Candidato = candidato;
    }
}
