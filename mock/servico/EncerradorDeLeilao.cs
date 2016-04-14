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
        private IRepositorioDeLeiloes dao;

        public int total { get; private set; }


        public EncerradorDeLeilao(IRepositorioDeLeiloes dao)
        {
            this.dao = dao;
            total = 0;
        }

        public virtual void encerra()
        {
            List<Leilao> todosLeiloesCorrentes = dao.correntes();

            foreach (var l in todosLeiloesCorrentes)
            {

                if (comecouSemanaPassada(l))
                {

                    l.encerra();
                    total++;
                    dao.atualiza(l);

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
