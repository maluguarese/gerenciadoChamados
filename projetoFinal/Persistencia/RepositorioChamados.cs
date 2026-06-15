using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;
using gerenciadorChamados.Dominio;

namespace gerenciadorChamados.Persistencia
{
    public class RepositorioChamados    
    {
        private readonly string _caminhoArquivo;

        public RepositorioChamados(string caminhoArquivo)
        {
            _caminhoArquivo = caminhoArquivo;
        }

        public List<Chamado> Carregar()
        {
            if (!File.Exists(_caminhoArquivo))
            {
                return new List<Chamado>();
            }

            string json = File.ReadAllText(_caminhoArquivo);

            try
            {
                var carregados = JsonSerializer.Deserialize<List<Chamado>>(json)
                                ?? new List<Chamado>();

                var validados = new List<Chamado>();

                foreach (var c in carregados)
                {
                    if (c == null)
                    {
                        Console.WriteLine("Aviso: encontrado registro nulo em chamados.json e foi descartado.");
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(c.Descricao))
                    {
                        Console.WriteLine($"Aviso: chamado com Id {c.Id} possui descrição vazia e foi descartado.");
                        continue;
                    }

                    // Observação: não alteramos dados desserializados aqui para manter fidelidade ao arquivo;
                    // apenas removemos registros claramente inválidos.
                    validados.Add(c);
                }

                return validados;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erro ao ler {Path.GetFileName(_caminhoArquivo)}: {ex.Message}");
                return new List<Chamado>();
            }
        }

        public void Salvar(List<Chamado> chamados)
        {
            var opcoes = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(chamados, opcoes);

            File.WriteAllText(_caminhoArquivo, json);
        }
    }
}
