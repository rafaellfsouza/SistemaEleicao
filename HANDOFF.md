# 🤝 HANDOFF — Passagem de bastão para o Dev 2

> Documento de transição da equipe. O **Dev 1** concluiu a metade do roteiro
> (estrutura + Etapas 1 e 2). O **Dev 2** deve dar segmento a partir daqui,
> executando as **Etapas 3 e 4** da lista de exercícios.

---

## 📦 Estado atual do projeto (pronto e testado)

| Item | Status |
|---|---|
| Solução `SistemaEleicao.sln` com Console + xUnit | ✅ feito (Dev 1) |
| Etapa 1 — `Candidato`, cadastro com validação de número duplicado | ✅ feito (Dev 1) |
| Etapa 2 — Eleitores, votação (nominal/branco/nulo) e apuração | ✅ feito (Dev 1) |
| Testes unitários | ✅ **30/30 passando** (`dotnet test`) |
| Etapa 3 — Menu interativo no `Program.cs` | ⬜ **PENDENTE (Dev 2)** |
| Etapa 4 — Refatoração + `README.md` | ⬜ **PENDENTE (Dev 2)** |

Histórico de commits até aqui (Conventional Commits):

```text
chore: initial project structure with console and xunit apps
test(candidato): add unit tests for candidate registration rules
feat(candidato): implement candidate entity and validation logic
test(votacao): add unit tests for duplicate voting prevention and invalid votes
feat(votacao): implement voting logic and voter verification
docs: add HANDOFF.md with roadmap and API reference for dev 2
```

## ▶️ Como rodar

Pré-requisito: [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

```bash
dotnet test                                                        # roda os 30 testes unitários
dotnet run --project SistemaEleicao.Console/SistemaEleicao.Console.csproj   # roda o console (ainda é o "Hello, World!" do template — é o seu trabalho na Etapa 3)
```

## 🧩 API do domínio (o que já existe para você usar)

Namespace `SistemaEleicao.Core` (arquivos em `SistemaEleicao.Console/Core/`):

```csharp
public class EleicaoManager
{
    public const int NumeroVotoBranco = 0;   // nº 0 = branco; nº inexistente = nulo

    public IReadOnlyCollection<Candidato> Candidatos { get; }
    public IReadOnlyCollection<Eleitor>   Eleitores  { get; }
    public IReadOnlyCollection<Voto>      Votos      { get; }
    public int TotalVotosRegistrados { get; }

    public Candidato CadastrarCandidato(string nome, int numero); // lança CandidatoDuplicadoException
    public Candidato? BuscarCandidatoPorNumero(int numero);
    public Eleitor   RegistrarEleitor(string titulo);             // lança EleitorDuplicadoException
    public Voto      Votar(string tituloEleitor, int numeroCandidato);
    // lança EleitorNaoRegistradoException / EleitorJaVotouException
    public ResultadoEleicao Apurar();
}
```

Exceções de negócio em `SistemaEleicao.Core.Excecoes`:
`CandidatoDuplicadoException`, `EleitorDuplicadoException`,
`EleitorNaoRegistradoException`, `EleitorJaVotouException`.

`ResultadoEleicao` (retorno do `Apurar()`):
`VotosPorCandidato` (dicionário, inclui candidatos com zero voto), `VotosBrancos`,
`VotosNulos`, `TotalVotos`, `Vencedor` (null se não houver votos válidos ou se houver empate) e `HouveEmpate`.

## 🛠️ Tarefas do Dev 2

### Etapa 3 — Interface de Console (`Program.cs`)

Substitua o `Console.WriteLine("Hello, World!")` do template por um menu interativo
em loop, por exemplo:

```text
===== SISTEMA DE ELEIÇÃO =====
1 - Cadastrar candidato
2 - Registrar eleitor
3 - Votar
4 - Apuração (relatório final)
0 - Sair
```

Requisitos:
- [ ] Instanciar um único `EleicaoManager` e usá-lo em todas as opções.
- [ ] Ler dados com `Console.ReadLine()` + `int.TryParse` (não deixar o app quebrar com texto inválido).
- [ ] Tratar as exceções de negócio com `try/catch` exibindo a mensagem amigável (`ex.Message`).
- [ ] Na opção 4, imprimir o relatório: votos por candidato (ordenado do mais votado),
      brancos, nulos, total e o vencedor (ou mensagem de empate).
- [ ] Encerrar com a opção 0.

⚠️ **Armadilha conhecida:** o projeto se chama `SistemaEleicao.Console`.
**Não** declare `namespace SistemaEleicao.Console` em nenhum arquivo — isso collide
com `System.Console` (erro CS0118). O `Program.cs` usa *top-level statements* (sem
namespace), onde `Console.WriteLine(...)` funciona normalmente. Mantenha o domínio em
`SistemaEleicao.Core`.

Commit sugerido (padrão do professor):

```bash
git add .
git commit -m "feat(console): implement interactive menu for election management"
```

### Etapa 4 — Refatoração e Documentação

1. Rode `dotnet test` — os 30 testes precisam continuar verdes após qualquer limpeza.
2. Refatore: extraia métodos do menu se ficarem grandes, padronize nomes, revise
   namespaces/`using`, remova código morto.
3. Crie o `README.md` na raiz com: descrição do projeto, requisitos, como compilar,
   como rodar os testes, como executar o console, estrutura da solução e o fluxo do menu.

Commits sugeridos (padrão do professor):

```bash
git add .
git commit -m "refactor(core): improve code readability and organize namespaces"
git add README.md
git commit -m "docs: add comprehensive instructions in README.md"
```

## ✅ Entrega final (item 4 da lista)

```bash
git remote add origin https://github.com/rafaellfsouza/SistemaEleicao.git
git branch -M main
git push -u origin main
git log --oneline   # conferir: histórico 100% Conventional Commits
```

Regras de ouro da equipe: **um commit por etapa concluída**, mensagens estritamente
no padrão Conventional Commits (`feat`, `fix`, `test`, `refactor`, `docs`, `chore`)
e **testes passando antes de cada commit**.
