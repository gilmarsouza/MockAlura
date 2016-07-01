using mock.dominio;
using mock.infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mock.servico
{
    public class GeradorDePagamento
    {
        private Avaliador avaliador;
        private LeilaoDaoFalso leilaoDao;
        private PagamentoDao pagamentoDao;
        private IRelogio relogio;

        public GeradorDePagamento(LeilaoDaoFalso leilaoDao, Avaliador avaliador, 
            PagamentoDao pagamentoDao)
        {
            this.leilaoDao = leilaoDao;
            this.avaliador = avaliador;
            this.pagamentoDao = pagamentoDao;
            this.relogio = new RelogioDoSistema();
        }

        public GeradorDePagamento(LeilaoDaoFalso leilaoDao, Avaliador avaliador, 
            PagamentoDao pagamentoDao, IRelogio relogio)
        {
            this.leilaoDao = leilaoDao;
            this.avaliador = avaliador;
            this.pagamentoDao = pagamentoDao;
            this.relogio = relogio;
        }

        public virtual void Gera()
        {
            var encerrados = leilaoDao.encerrados();

            foreach (var l in encerrados)
            {
                avaliador.avalia(l);
                var pagamento = new Pagamento(this.avaliador.maiorValor, ProximoDiaUtil());
                pagamentoDao.Salva(pagamento);
            }
        }

        public virtual DateTime ProximoDiaUtil()
        {
            var data = relogio.Hoje();
            var diaDaSemana = data.DayOfWeek;

            switch (diaDaSemana)
            {
                case DayOfWeek.Sunday:
                    data = data.AddDays(1);
                    break;
                case DayOfWeek.Saturday:
                    data = data.AddDays(2);
                    break;
                default:
                    break;
            }

            return data;
        }
    }
}
