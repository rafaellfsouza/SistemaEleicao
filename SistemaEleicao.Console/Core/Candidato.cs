namespace SistemaEleicao.Core;

/// <summary>
/// Representa um candidato à eleição, identificado por nome e número de urna.
/// O número é a identidade do candidato e não pode se repetir na mesma eleição.
/// </summary>
public class Candidato
{
    public int Numero { get; }
    public string Nome { get; }

    public Candidato(int numero, string nome)
    {
        if (numero <= 0)
            throw new ArgumentOutOfRangeException(nameof(numero), "O número do candidato deve ser um valor positivo.");

        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do candidato não pode ser nulo ou vazio.", nameof(nome));

        Numero = numero;
        Nome = nome.Trim();
    }

    public override string ToString() => $"{Numero:D2} - {Nome}";
}
