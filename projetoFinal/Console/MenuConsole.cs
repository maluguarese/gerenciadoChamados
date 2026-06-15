using System;
using System.Collections.Generic;
using System.Globalization;
using gerenciadorChamados.Dominio;
using gerenciadorChamados.Persistencia;

namespace gerenciadorChamados.UI
{
    internal class MenuConsole
    {
        private readonly GerenciadorChamados _gerenciador;
        private readonly RepositorioChamados _repositorio;

        public MenuConsole(GerenciadorChamados gerenciador, RepositorioChamados repositorio)
        {
            _gerenciador = gerenciador;
            _repositorio = repositorio;
        }

        public void Executar()
        {
            bool sair = false;

            while (!sair)
            {
                ExibirMenu();

                Console.Write("Escolha uma opção: ");
                string? opc = Console.ReadLine();

                switch (opc)
                {
                    case "1":
                        ExibirNovoChamado();
                        break;
                    case "2":
                        ExibirConsulta();
                        break;
                    case "0":
                        sair = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        AguardarTecla();
                        break;
                }
            }
        }

        private void ExibirMenu()
        {
            Console.Clear();
            Console.WriteLine("┌──────────────────────────────────────────────┐");
            Console.WriteLine("│           GERENCIADOR DE CHAMADOS            │");
            Console.WriteLine("│              === MENU PRINCIPAL ===          │");
            Console.WriteLine("├──────────────────────────────────────────────┤");
            Console.WriteLine("│                                              │");
            Console.WriteLine("│   [1] Abrir novo chamado                     │");
            Console.WriteLine("│   [2] Consultar chamados                     │");
            Console.WriteLine("│   [0] Sair                                   │");
            Console.WriteLine("│                                              │");
            Console.WriteLine("├──────────────────────────────────────────────┤");

            var lista = _gerenciador.ListarTodos();
            int total = lista.Count;
            int abertos = lista.Count(c => c.Status == StatusChamado.Aberto);

            Console.WriteLine($"│  Total de chamados: {total}  |  Abertos: {abertos}         │");
            Console.WriteLine("│                                              │");
            Console.WriteLine("│  Escolha uma opção: _                        │");
            Console.WriteLine("└──────────────────────────────────────────────┘");
        }

        private void ExibirNovoChamado()
        {
            Console.Clear();
            Console.WriteLine("┌──────────────────────────────────────────────┐");
            Console.WriteLine("│           GERENCIADOR DE CHAMADOS            │");
            Console.WriteLine("│            === NOVO CHAMADO ===              │");
            Console.WriteLine("├──────────────────────────────────────────────┤");
            Console.WriteLine("│                                              │");
            Console.WriteLine("│  Descreva o problema ou solicitação:         │");
            Console.WriteLine("│                                              │");
            Console.WriteLine("└──────────────────────────────────────────────┘");
            Console.Write("\n> ");

            string? descricao;
            while (true)
            {
                descricao = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(descricao))
                    break;

                Console.WriteLine("Descrição inválida. Digite uma descrição não vazia ou pressione Ctrl+C para cancelar.");
                Console.Write("\n> ");
            }

            try
            {
                var chamado = _gerenciador.AbrirChamado(descricao ?? string.Empty);

                // Salva imediatamente
                _repositorio.Salvar(_gerenciador.ListarTodos());

                Console.Clear();
                Console.WriteLine("┌──────────────────────────────────────────────┐");
                Console.WriteLine("│           GERENCIADOR DE CHAMADOS            │");
                Console.WriteLine("│            === NOVO CHAMADO ===              │");
                Console.WriteLine("├──────────────────────────────────────────────┤");
                Console.WriteLine("│                                              │");
                Console.WriteLine("│  Chamado criado com sucesso!                 │");
                Console.WriteLine("│                                              │");
                var abertura = chamado.DataAbertura.ToString("dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture);
                var descricaoTrunc = Truncar(chamado.Descricao, 40);
                Console.WriteLine($"│  ID...........: {chamado.Id,-28}│");
                Console.WriteLine($"│  Status.......: {chamado.Status,-28}│");
                Console.WriteLine($"│  Abertura.....: {abertura,-28}│");
                Console.WriteLine($"│  Descrição....: {descricaoTrunc,-28}│");
                Console.WriteLine("│                                              │");
                Console.WriteLine("├──────────────────────────────────────────────┤");
                Console.WriteLine("│  Pressione qualquer tecla para voltar...     │");
                Console.WriteLine("└──────────────────────────────────────────────┘");

                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                AguardarTecla();
            }
        }

        private void ExibirConsulta()
        {
            Console.Clear();
            Console.WriteLine("┌──────────────────────────────────────────────┐");
            Console.WriteLine("│           GERENCIADOR DE CHAMADOS            │");
            Console.WriteLine("│         === CONSULTA DE CHAMADOS ===         │");
            Console.WriteLine("├──────────────────────────────────────────────┤");
            Console.WriteLine("│                                              │");

            var lista = _gerenciador.ListarTodos();

            if (lista.Count == 0)
            {
                Console.WriteLine("│  Nenhum chamado cadastrado.                  │");
                Console.WriteLine("│                                              │");
                Console.WriteLine("│  [0] Voltar ao menu                          │");
                Console.WriteLine("│                                              │");
                Console.WriteLine("└──────────────────────────────────────────────┘");
                Console.Write("\n> ");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("│  ID  │ Status    │ Abertura   │ Descrição                │");
            Console.WriteLine("│  ----+-----------+------------+------------------------- │");

            foreach (var c in lista)
            {
                Console.WriteLine($"│  {c.Id,-3} │ {c.Status,-9} │ {c.DataAbertura.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture)} │ {Truncar(c.Descricao,30),-25}│");
            }

            Console.WriteLine("│                                              │");
            Console.WriteLine("├──────────────────────────────────────────────┤");
            Console.WriteLine("│  Digite o ID para ver detalhes (0 = voltar): │");
            Console.Write("\n> ");

            string? entrada = Console.ReadLine();

            if (!int.TryParse(entrada, out int id))
            {
                return;
            }

            if (id == 0)
                return;

            ExibirDetalhe(id);
        }

        private void ExibirDetalhe(int id)
        {
            Console.Clear();
            var chamado = _gerenciador.ObterPorId(id);

            if (chamado == null)
            {
                Console.WriteLine("┌──────────────────────────────────────────────┐");
                Console.WriteLine($"│  Chamado com ID {id} não encontrado.           │");
                Console.WriteLine("│  Pressione qualquer tecla para voltar...     │");
                Console.WriteLine("└──────────────────────────────────────────────┘");
                Console.ReadKey(true);
                return;
            }

            Console.WriteLine("┌──────────────────────────────────────────────┐");
            Console.WriteLine("│           GERENCIADOR DE CHAMADOS            │");
            Console.WriteLine("│          === DETALHE DO CHAMADO ===          │");
            Console.WriteLine("├──────────────────────────────────────────────┤");
            Console.WriteLine("│                                              │");
            Console.WriteLine($"│  ID...........: {chamado.Id,-28}│");
            Console.WriteLine($"│  Status.......: {chamado.Status,-28}│");
                string abertura = chamado.DataAbertura.ToString("dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture);
                string conclusao = chamado.DataConclusao != null ? chamado.DataConclusao.Value.ToString("dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture) : "—";
                Console.WriteLine($"│  Abertura.....: {abertura,-28}│");
                Console.WriteLine($"│  Conclusão....: {conclusao,-28}│");
            Console.WriteLine("│                                              │");
            Console.WriteLine("│  Descrição:                                  │");
            foreach (var linha in DividirLinhas(chamado.Descricao, 48))
            {
                Console.WriteLine($"│  {linha,-44}│");
            }
            Console.WriteLine("│                                              │");
            Console.WriteLine("├──────────────────────────────────────────────┤");

            if (chamado.EstaAberto())
            {
                Console.WriteLine("│  [1] Concluir chamado                        │");
                Console.WriteLine("│  [0] Voltar                                  │");
                Console.WriteLine("│                                              │");
                Console.WriteLine("│  Escolha uma opção: _                        │");
                Console.WriteLine("└──────────────────────────────────────────────┘");
                Console.Write("\n> ");
                string? op = Console.ReadLine();

                if (op == "1")
                {
                    bool ok = _gerenciador.ConcluirChamado(chamado.Id);
                    if (ok)
                    {
                        _repositorio.Salvar(_gerenciador.ListarTodos());
                        Console.WriteLine("Chamado concluído com sucesso.");
                    }
                    else
                    {
                        Console.WriteLine("Não foi possível concluir o chamado.");
                    }
                }
            }
            else
            {
                Console.WriteLine("│  Este chamado já está concluído.             │");
                Console.WriteLine("│                                              │");
                Console.WriteLine("│  [0] Voltar                                  │");
                Console.WriteLine("└──────────────────────────────────────────────┘");
                Console.ReadKey(true);
            }

            AguardarTecla();
        }

        private void AguardarTecla()
        {
            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadKey(true);
        }

        private static string Truncar(string? texto, int tamanho)
        {
            if (string.IsNullOrEmpty(texto))
                return string.Empty;

            if (texto.Length <= tamanho)
                return texto;

            return texto.Substring(0, tamanho - 1) + "…";
        }

        private static IEnumerable<string> DividirLinhas(string? texto, int largura)
        {
            if (string.IsNullOrEmpty(texto))
            {
                yield return string.Empty;
                yield break;
            }

            int idx = 0;
            while (idx < texto.Length)
            {
                int len = Math.Min(largura, texto.Length - idx);
                yield return texto.Substring(idx, len);
                idx += len;
            }
        }
    }
}
