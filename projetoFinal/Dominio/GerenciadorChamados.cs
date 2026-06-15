using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gerenciadorChamados.Dominio
{
    public class GerenciadorChamados
    {
        private readonly List<Chamado> _chamados;

        public GerenciadorChamados(List<Chamado> chamados)
        {
            // Clona a lista recebida para proteger o estado interno de modificações externas
            _chamados = chamados != null
                ? new List<Chamado>(chamados)
                : new List<Chamado>();
        }

        public Chamado AbrirChamado(string descricao)
        {
            int novoId = 1;

            if (_chamados.Count > 0)
            {
                novoId = _chamados.Max(c => c.Id) + 1;
            }

            Chamado chamado = new Chamado(novoId, descricao);

            _chamados.Add(chamado);

            return chamado;
        }

        public List<Chamado> ListarTodos()
        {
            // Retorna uma cópia para evitar que chamadores alterem diretamente a coleção interna
            return new List<Chamado>(_chamados);
        }

        public Chamado ObterPorId(int id)
        {
            return _chamados.FirstOrDefault(c => c.Id == id);
        }

        public bool ConcluirChamado(int id)
        {
            Chamado chamado = ObterPorId(id);

            if (chamado == null)
            {
                return false;
            }

            if (!chamado.EstaAberto())
            {
                return false;
            }

            chamado.Concluir();

            return true;
        }
    }
}
    