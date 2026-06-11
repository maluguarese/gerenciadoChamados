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

            return JsonSerializer.Deserialize<List<Chamado>>(json)
                   ?? new List<Chamado>();
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
