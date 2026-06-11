using System;
using System.IO;
using gerenciadorChamados.Dominio;
using gerenciadorChamados.Persistencia;

string caminhoArquivo =
    Path.Combine(AppContext.BaseDirectory, "chamados.json");

RepositorioChamados repositorio =
    new RepositorioChamados(caminhoArquivo);

var chamados = repositorio.Carregar();

GerenciadorChamados gerenciador =
    new GerenciadorChamados(chamados);

Console.WriteLine("Criando chamados...");

var chamado1 =
    gerenciador.AbrirChamado("Impressora não funciona.");

var chamado2 =
    gerenciador.AbrirChamado("Erro ao acessar portal.");

gerenciador.ConcluirChamado(chamado1.Id);

repositorio.Salvar(gerenciador.ListarTodos());

Console.WriteLine("Dados salvos.");

Console.WriteLine("\nRecarregando dados...\n");

var lista = repositorio.Carregar();

foreach (var chamado in lista)
{
    Console.WriteLine($"ID: {chamado.Id}");
    Console.WriteLine($"Descrição: {chamado.Descricao}");
    Console.WriteLine($"Status: {chamado.Status}");
    Console.WriteLine($"Abertura: {chamado.DataAbertura}");

    if (chamado.DataConclusao != null)
    {
        Console.WriteLine($"Conclusão: {chamado.DataConclusao}");
    }

    Console.WriteLine("--------------------------");
}
