using System;
using System.Collections.Generic;
using System.Text;

namespace gerenciadorChamados.Dominio
{
    public class GerenciadorChamados
    {
        private readonly List<Chamado> _chamados;

        public GerenciadorChamados(List<Chamado> chamados)
        {
            _chamados = chamados;
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
            return _chamados;
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
    