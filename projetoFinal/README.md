# 📋 Gerenciador de Chamados

## 👤 Aluna
**Maria Luiza Guarese Sasseti**

---

## 📝 Descrição do Projeto

Este projeto foi desenvolvido para a disciplina de **Programação Orientada a Objetos I**. 

O sistema simula um **gerenciador de chamados** utilizado por equipes de suporte técnico em empresas. Cada chamado possui:
- Identificador único (gerado automaticamente)
- Descrição do problema
- Status (Aberto ou Concluído)
- Data de abertura (registrada automaticamente)
- Data de conclusão (quando o chamado é finalizado)

O projeto implementa as camadas de **domínio**, **persistência em JSON** e uma **interface de console** com menu interativo para criação, consulta e conclusão de chamados. Todos os dados são persistidos automaticamente ao sair do programa.

---

## 🚀 Como Compilar e Executar

### Pré-requisitos
- **.NET 10 SDK** instalado

### Método 1: Terminal (recomendado)
Abra o terminal PowerShell na pasta do projeto e execute:
```powershell
dotnet run
```

### Método 2: Visual Studio
1. Abra o Visual Studio Community 2026
2. Abra a solução `projetoFinal.slnx`
3. Clique em **Build** > **Build Solution** (ou pressione Ctrl+Shift+B)
4. Pressione **F5** para executar

---

## 📚 Descrição das Classes

### **Camada de Domínio**

#### `Chamado.cs`
Representa um chamado de suporte técnico.

**Propriedades:**
- `Id` (int): Identificador único do chamado (somente leitura)
- `Descricao` (string): Descrição do problema reportado (somente leitura)
- `Status` (StatusChamado): Status atual do chamado (somente leitura)
- `DataAbertura` (DateTime): Data/hora de criação do chamado (somente leitura)
- `DataConclusao` (DateTime?): Data/hora de conclusão (null se aberto)

**Métodos:**
- `Chamado(int id, string descricao)`: Construtor que valida a descrição e inicializa o chamado
- `EstaAberto()`: Retorna true se o chamado está com status Aberto
- `Concluir()`: Muda o status para Concluído e registra a data de conclusão

**Características:**
- Todos os setters são privados, garantindo encapsulamento robusto
- Valida se a descrição é vazia (lança ArgumentException)
- Impede encerramento duplo (lança InvalidOperationException)

---

#### `StatusChamado.cs`
Enumeração que define os possíveis estados de um chamado.

**Valores:**
- `Aberto`: Chamado aguardando resolução
- `Concluido`: Chamado já resolvido

---

#### `GerenciadorChamados.cs`
Gerencia a coleção de chamados e as operações sobre eles.

**Propriedades:**
- `_chamados` (List<Chamado>): Lista interna de chamados

**Métodos:**
- `GerenciadorChamados(List<Chamado> chamados)`: Construtor que clona a lista para proteger o estado interno
- `AbrirChamado(string descricao)`: Cria um novo chamado com ID automático e o adiciona à lista
- `ListarTodos()`: Retorna uma cópia da lista de chamados
- `ObterPorId(int id)`: Busca e retorna um chamado específico por ID
- `ConcluirChamado(int id)`: Localiza um chamado e muda seu status para Concluído

**Características:**
- Gera IDs sequenciais automaticamente
- Protege o estado interno retornando cópias das coleções

---

### **Camada de Persistência**

#### `RepositorioChamados.cs`
Responsável por carregar e salvar chamados em arquivo JSON.

**Propriedades:**
- `_caminhoArquivo` (string): Caminho para o arquivo chamados.json

**Métodos:**
- `RepositorioChamados(string caminhoArquivo)`: Construtor que define o caminho do arquivo
- `Carregar()`: Lê o arquivo JSON e retorna a lista de chamados (cria lista vazia se arquivo não existe)
- `Salvar(List<Chamado> chamados)`: Escreve a lista de chamados no arquivo JSON

**Características:**
- Usa `System.Text.Json` para serialização/desserialização
- Trata erros graciosamente (JSON malformado, registros nulos)
- Cria o arquivo automaticamente se não existir

---

### **Camada de Apresentação (Console)**

#### `MenuConsole.cs`
Interface interativa em console para o usuário.

**Propriedades:**
- `_gerenciador` (GerenciadorChamados): Instância do gerenciador
- `_repositorio` (RepositorioChamados): Instância do repositório

**Métodos:**
- `Executar()`: Inicia o menu principal e mantém o loop interativo
- `ExibirMenu()`: Exibe o menu principal com opções
- `ExibirNovoChamado()`: Tela para criar novo chamado
- `ExibirConsulta()`: Tela de consulta de chamados
- `ExibirDetalhes(Chamado)`: Exibe detalhes de um chamado específico

**Características:**
- Usa `Console.Clear()` em cada tela para limpeza visual
- Bordas decorativas com caracteres Unicode (box-drawing)
- Formatação consistente de datas

---

## 🎨 Descrição das Telas

### **Tela 1: Menu Principal**
```
┌──────────────────────────────────────────────────────────┐
│           GERENCIADOR DE CHAMADOS                        │
│              === MENU PRINCIPAL ===                      │
├──────────────────────────────────────────────────────────┤
│                                                           │
│   [1] Abrir novo chamado                                 │
│   [2] Consultar chamados                                 │
│   [0] Sair                                               │
│                                                           │
└──────────────────────────────────────────────────────────┘

Escolha uma opção: 
```
**Funcionalidade:** Menu inicial com três opções para o usuário navegar entre as funcionalidades principais.

---

### **Tela 2: Abrir Novo Chamado**
```
┌──────────────────────────────────────────────────────────┐
│           GERENCIADOR DE CHAMADOS                        │
│           === ABRIR NOVO CHAMADO ===                     │
├──────────────────────────────────────────────────────────┤

Digite a descrição do chamado: 
```
**Funcionalidade:** O usuário digita uma descrição do problema. Um novo chamado é criado com ID automático e registra a data/hora de abertura. Após criar, retorna ao menu principal.

---

### **Tela 3: Consultar Chamados**
```
┌──────────────────────────────────────────────────────────┐
│           GERENCIADOR DE CHAMADOS                        │
│          === LISTAR TODOS OS CHAMADOS ===                │
├──────────────────────────────────────────────────────────┤

Chamado #1 - Aberto
Chamado #2 - Concluído
Chamado #3 - Aberto

[0] Voltar ao menu
[Digite o ID do chamado] Para ver detalhes: 
```
**Funcionalidade:** Lista todos os chamados com ID e status. O usuário pode:
- Pressionar **0** para voltar ao menu
- Digitar um **ID** para ver detalhes do chamado

---

### **Tela 4: Detalhes do Chamado**
```
┌──────────────────────────────────────────────────────────┐
│           GERENCIADOR DE CHAMADOS                        │
│           === DETALHES DO CHAMADO ===                    │
├──────────────────────────────────────────────────────────┤

ID do chamado: 1
Descrição: Printer não funciona
Status: Aberto
Aberto em: 15/06/2024 10:30:45
Concluído em: -

[1] Concluir chamado
[0] Voltar
Escolha uma opção: 
```
**Funcionalidade:** Exibe informações completas do chamado. O usuário pode:
- Pressionar **1** para marcar como concluído (muda status e registra data de conclusão)
- Pressionar **0** para voltar à lista

---

## 💾 Persistência de Dados

Os dados são salvos automaticamente em `chamados.json` quando o usuário sai do programa (opção [0] no menu principal).

**Formato do arquivo:**
```json
[
  {
	"Id": 1,
	"Descricao": "Printer não funciona",
	"Status": 0,
	"DataAbertura": "2024-06-15T10:30:45.1234567",
	"DataConclusao": null
  },
  {
	"Id": 2,
	"Descricao": "Monitor com defeito",
	"Status": 1,
	"DataAbertura": "2024-06-15T09:15:00.1234567",
	"DataConclusao": "2024-06-15T14:20:30.1234567"
  }
]
```

Ao iniciar o programa, os dados são carregados automaticamente do arquivo JSON.

---

## 📂 Estrutura do Projeto

```
projetoFinal/
│
├── Console/
│   └── MenuConsole.cs           # Interface de console interativa
│
├── Dominio/
│   ├── Chamado.cs              # Entidade Chamado
│   ├── GerenciadorChamados.cs  # Lógica de negócio
│   └── StatusChamados.cs       # Enumeração de status
│
├── Persistencia/
│   └── RepositorioChamados.cs  # Acesso aos dados em JSON
│
├── Program.cs                  # Ponto de entrada da aplicação
├── chamados.json               # Arquivo de persistência
├── README.md                   # Este arquivo
└── gerenciadorChamados.csproj  # Arquivo de projeto
```

---

## ✨ Funcionalidades Implementadas

✅ Abrir novo chamado com ID automático  
✅ Gerar ID sequencial automaticamente  
✅ Listar todos os chamados registrados  
✅ Buscar chamado por ID e exibir detalhes completos  
✅ Concluir chamados e registrar data de conclusão  
✅ Persistência em arquivo JSON (carregar/salvar)  
✅ Carregamento automático dos dados ao iniciar  
✅ `Console.Clear()` em todas as telas  
✅ Menu interativo com validações  
✅ Tratamento de erros e exceções  

---

## 🔒 Decisões de Implementação

1. **Encapsulamento Robusto**: Todas as propriedades de `Chamado` possuem setters privados, evitando alterações diretas ao estado do objeto.

2. **Proteção de Estado Interno**: 
   - `GerenciadorChamados` clona a lista recebida no construtor
   - `ListarTodos()` retorna uma cópia da coleção

3. **Validações**: 
   - Descrição vazia é rejeitada com `ArgumentException`
   - Tentativa de concluir chamado já concluído lança `InvalidOperationException`

4. **IDs Automáticos**: Cada novo chamado recebe um ID incremental baseado no valor máximo de IDs existentes.

5. **Datas com Precisão**: Utiliza `DateTime.Now` para capturar precisamente o momento de abertura e conclusão.

---

## 🛠️ Tecnologias Utilizadas

- **Linguagem**: C# 11+
- **Framework**: .NET 10
- **Persistência**: System.Text.Json
- **Paradigma**: Programação Orientada a Objetos

---

## 📋 Requisitos do Sistema

- **.NET 10 SDK** ou superior
- **Windows 10/11** (ou Linux/macOS com .NET instalado)
- **Terminal/PowerShell** ou **Visual Studio 2026+**

---

## 📖 Como Usar o Sistema

1. **Executar o programa** com `dotnet run`
2. **Menu principal** aparecerá com 3 opções
3. **Opção [1]**: Digite a descrição do chamado e pressione Enter
4. **Opção [2]**: Digita o ID de um chamado para ver detalhes ou [0] para voltar
5. **Digite [1]** na tela de detalhes para concluir o chamado
6. **Opção [0]**: Sair do programa (dados são salvos automaticamente)

---

## 📝 Observações Finais

- Todos os chamados são armazenados permanentemente em `chamados.json`
- As datas são exibidas no formato DD/MM/YYYY HH:MM:SS
- O ID de cada chamado é gerado sequencialmente
- Não é possível alterar ou deletar chamados existentes
- A conclusão de um chamado registra automaticamente a data/hora

---

**Entrega Final - 29/06/2024**
