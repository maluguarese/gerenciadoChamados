using System;
using System.Collections.Generic;
using System.Text;

namespace gerenciadorChamados.Dominio
{
    public class Chamado
    {
        public int Id { get; private set; }

        public string Descricao { get; private set; }

        public StatusChamado Status { get; private set; }

        public DateTime DataAbertura { get; private set; }

        public DateTime? DataConclusao { get; private set; }

        public Chamado()
        {
        }

        public Chamado(int id, string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao))
            {
                throw new ArgumentException("A descrição não pode ser vazia.");
            }

            Id = id;
            Descricao = descricao;
            Status = StatusChamado.Aberto;
            DataAbertura = DateTime.Now;
            DataConclusao = null;
        }

        public bool EstaAberto()
        {
            return Status == StatusChamado.Aberto;
        }

        public void Concluir()
        {
            if (Status == StatusChamado.Concluido)
            {
                throw new InvalidOperationException("O chamado já está concluído.");
            }

            Status = StatusChamado.Concluido;
            DataConclusao = DateTime.Now;
        }
    }
}
    