______Gerenciador de Chamados______

Aluna: Maria Luiza Guarese Sasseti

Descrição do Projeto:

- Este projeto foi desenvolvido para a disciplina de Programação Orientada a Objetos I.

- O sistema simula um gerenciador de chamados utilizado por equipes de suporte técnico. Cada chamado possui um identificador único, descrição, status, data de abertura e data de conclusão.

- Nesta primeira entrega foram implementadas as classes do domínio, a persistência em JSON e um teste manual no Program.cs.

Diagrama de Classes:

classDiagram

class Chamado {
    +int Id
    +string Descricao
    +StatusChamado Status
    +DateTime DataAbertura
    +DateTime? DataConclusao
    +Concluir()
    +bool EstaAberto()
}

class GerenciadorChamados {
    -List~Chamado~ _chamados
    +AbrirChamado(string descricao) Chamado
    +ListarTodos() List~Chamado~
    +ObterPorId(int id) Chamado
    +ConcluirChamado(int id) bool
}

class RepositorioChamados {
    -string _caminhoArquivo
    +Carregar() List~Chamado~
    +Salvar(List~Chamado~ chamados)
}

class StatusChamado {
    <<enumeration>>
    Aberto
    Concluido
}

GerenciadorChamados --> Chamado
RepositorioChamados --> Chamado
Chamado --> StatusChamado

Estrutura do Projeto:

gerenciadorChamados
│
├── Console
│   └── MenuConsole.cs
│
├── Dominio
│   ├── Chamado.cs
│   ├── GerenciadorChamados.cs
│   └── StatusChamado.cs
│
├── Persistencia
│   └── RepositorioChamados.cs
│
├── chamados.json
├── Program.cs
└── README.md

Funcionalidades Implementadas:
- Criar chamados
- Gerar ID automático
- Listar chamados
- Buscar chamado por ID
- Concluir chamados
- Persistência em arquivo JSON
- Carregamento automático dos dados

Como Executar:
- Abrir o projeto no Visual Studio.
- Compilar a solução.
- Executar o projeto.
- O teste manual presente no Program.cs será executado automaticamente.

    Ou pelo terminal:
- dotnet run


Tecnologias Utilizadas:
- C#
- .NET
- System.Text.Json
- Programação Orientada a Objetos