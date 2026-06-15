using System;
using System.IO;
using System.Globalization;
using gerenciadorChamados.Dominio;
using gerenciadorChamados.Persistencia;

string caminhoArquivo =
    Path.Combine(AppContext.BaseDirectory, "chamados.json");

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");

RepositorioChamados repositorio =
    new RepositorioChamados(caminhoArquivo);

var chamados = repositorio.Carregar();

GerenciadorChamados gerenciador =
    new GerenciadorChamados(chamados);

// Inicia menu interativo
var repositorioInst = repositorio;
var menu = new gerenciadorChamados.UI.MenuConsole(gerenciador, repositorioInst);
menu.Executar();

// Ao sair, salva o estado atual
repositorio.Salvar(gerenciador.ListarTodos());
Console.WriteLine("Dados salvos.");
