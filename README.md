<div align="center">

# 🗳️ Sistema de Eleição

**Aplicação em .NET 8 para gestão de eleições: cadastro de candidatos, registro de eleitores, votação (nominal, branca e nula) e apuração com detecção de empate — com 25 testes unitários em xUnit.**

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-12-239120?style=flat-square&logo=csharp&logoColor=white)
![xUnit](https://img.shields.io/badge/xUnit-2.5.3-5C2D91?style=flat-square)
![Testes](https://img.shields.io/badge/testes-25%20m%C3%A9todos-2EA043?style=flat-square)
![Licença](https://img.shields.io/badge/licen%C3%A7a-MIT-blue?style=flat-square)

</div>

---

## 📖 Sobre o projeto

Sistema de votação desenvolvido em **C#/.NET 8** aplicando os princípios de **orientação a objetos**, **encapsulamento** e **desenvolvimento orientado a testes (TDD)**.

O domínio modela uma eleição completa: candidatos com número único, eleitores identificados por título, votos classificados em nominais/brancos/nulos e uma apuração que aponta o vencedor ou sinaliza empate.

## 🏗️ Arquitetura

```text
SistemaEleicao.sln
├── SistemaEleicao.Console/          → aplicação
│   ├── Core/                        → camada de domínio
│   │   ├── Candidato.cs
│   │   ├── Eleitor.cs
│   │   ├── Voto.cs
│   │   ├── ResultadoEleicao.cs
│   │   ├── EleicaoManager.cs        → orquestrador (fachada do domínio)
│   │   └── Excecoes/                → exceções de negócio
│   └── Program.cs                   → entry point
└── SistemaEleicao.Tests/            → testes unitários (xUnit)
    ├── CandidatoTests.cs
    ├── VotacaoTests.cs
    └── ApuracaoTests.cs
```

## 🧩 Modelo de domínio

| Classe | Responsabilidade |
|:---|:---|
| `Candidato` | Entidade candidato — nome e número único na urna |
| `Eleitor` | Entidade eleitor — identificado pelo título, controla se já votou |
| `Voto` | Registro do voto e seu tipo (candidato, branco ou nulo) |
| `ResultadoEleicao` | Resultado da apuração: contagem por candidato, brancos, nulos, total e vencedor |
| `EleicaoManager` | Fachada que orquestra cadastro, registro, votação e apuração |

## 🔌 API do domínio

Namespace `SistemaEleicao.Core`:

```csharp
public class EleicaoManager
{
    public const int NumeroVotoBranco = 0;   // nº 0 = branco; nº inexistente = nulo

    public IReadOnlyCollection<Candidato> Candidatos { get; }
    public IReadOnlyCollection<Eleitor>   Eleitores  { get; }
    public IReadOnlyCollection<Voto>      Votos      { get; }
    public int TotalVotosRegistrados { get; }

    public Candidato  CadastrarCandidato(string nome, int numero);
    public Candidato? BuscarCandidatoPorNumero(int numero);
    public Eleitor    RegistrarEleitor(string titulo);
    public Voto       Votar(string tituloEleitor, int numeroCandidato);
    public ResultadoEleicao Apurar();
}
```

## 🛡️ Regras de negócio e exceções

| Situação | Comportamento |
|:---|:---|
| Número de candidato já cadastrado | lança `CandidatoDuplicadoException` |
| Nome nulo/vazio ou número não positivo | lança `ArgumentException` / `ArgumentOutOfRangeException` |
| Título de eleitor já registrado | lança `EleitorDuplicadoException` |
| Eleitor não cadastrado tentando votar | lança `EleitorNaoRegistradoException` |
| Eleitor votando duas vezes | lança `EleitorJaVotouException` |
| Voto no número `0` | computado como **voto em branco** |
| Voto em número sem candidato | computado como **voto nulo** |
| Empate na apuração | nenhum vencedor é definido |

## 🧪 Testes unitários

**25 métodos de teste** (`[Fact]` e `[Theory]`) cobrindo as três frentes do domínio:

| Arquivo | Testes | Cobertura |
|:---|:---:|:---|
| `CandidatoTests.cs` | 9 | cadastro válido, número duplicado, nomes iguais com números distintos, validação de nome e número |
| `VotacaoTests.cs` | 10 | registro de eleitor, título duplicado/inválido, voto único, eleitor inexistente, tipos de voto |
| `ApuracaoTests.cs` | 6 | contagem por candidato, nulos e brancos separados, total, vencedor, empate e eleição sem votos |

## ▶️ Como executar

Pré-requisito: [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

```bash
# clonar
git clone https://github.com/rafaellfsouza/SistemaEleicao.git
cd SistemaEleicao

# rodar a suíte de testes
dotnet test

# executar a aplicação console
dotnet run --project SistemaEleicao.Console/SistemaEleicao.Console.csproj
```

## 🚧 Roadmap

- [x] **Etapa 1** — entidade `Candidato` com cadastro e validação de número duplicado
- [x] **Etapa 2** — eleitores, votação (nominal/branco/nulo) e apuração
- [x] **Testes unitários** — 25 métodos cobrindo as regras de negócio
- [ ] **Etapa 3** — menu interativo no `Program.cs` *(hoje ainda é o template "Hello, World!")*
- [ ] **Etapa 4** — refatoração final e documentação complementar

> 🤝 Este projeto foi desenvolvido em dupla. O documento de passagem de bastão entre os desenvolvedores está em [`HANDOFF.md`](HANDOFF.md), com o histórico de commits em [Conventional Commits](https://www.conventionalcommits.org/) e a referência completa da API.

---

<div align="center">

<sub>Desenvolvido por <a href="https://github.com/rafaellfsouza"><b>Rafael Luiz Ferreira de Souza</b></a> · <code>C#</code> <code>.NET 8</code> <code>xUnit</code> <code>TDD</code></sub>

</div>
