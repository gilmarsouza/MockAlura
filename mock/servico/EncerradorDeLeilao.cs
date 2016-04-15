using mock.dominio;
using mock.infra;
using mock.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mock.servico
{
    public class EncerradorDeLeilao
    {
        public int total { get; private set; }
        private IRepositorioDeLeiloes dao;
        private Carteiro carteiro;


        public EncerradorDeLeilao(IRepositorioDeLeiloes dao, Carteiro carteiro)
        {
            this.dao = dao;
            this.carteiro = carteiro;
            total = 0;
        }

        public virtual void encerra()
        {
            List<Leilao> todosLeiloesCorrentes = dao.correntes();
            Console.WriteLine(todosLeiloesCorrentes.Count);

            foreach (var l in todosLeiloesCorrentes)
            {

                if (comecouSemanaPassada(l))
                {

                    l.encerra();
                    total++;
                    dao.atualiza(l);
                    carteiro.envia(l);
                }
            }
        }


        private bool comecouSemanaPassada(Leilao leilao)
        {

            return diasEntre(leilao.data, DateTime.Now) >= 7;

        }

        private int diasEntre(DateTime inicio, DateTime fim)
        {
            int dias = (int)(fim - inicio).TotalDays;

            return dias;
        }

    }
}
