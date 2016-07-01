using mock.dominio;
using mock.infra;
using mock.servico;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mock.testes
{
    [TestFixture]
    public class GeradorDePagamentoTeste
    {
        [Test]
        public void DeveGerarPagamentoParaLeilaoEncerradoComOMaiorValor()
        {
            var leilaoDao = new Mock<LeilaoDaoFalso>();
            var avaliador = new Mock<Avaliador>();
            var pagamentoDao = new Mock<PagamentoDao>();

            var leilao1 = new Leilao("Playstation");
            leilao1.propoe(new Lance(new Usuario("Fulano"), 100));
            leilao1.propoe(new Lance(new Usuario("Ciclano"), 150));
            leilao1.propoe(new Lance(new Usuario("Beltrano"), 250));

            leilao1.naData(DateTime.Today.AddDays(-7));

            var leiloes = new List<Leilao>();
            leiloes.Add(leilao1);

            leilaoDao.Setup(m => m.encerrados()).Returns(leiloes);
            avaliador.Setup(a => a.maiorValor).Returns(250);

            Pagamento retorno = null;
            pagamentoDao.Setup(p => p.Salva(It.IsAny<Pagamento>())).
                Callback<Pagamento>(r => retorno = r);

            var gerador = new GeradorDePagamento(leilaoDao.Object, avaliador.Object, 
                pagamentoDao.Object);

            gerador.Gera();

            Assert.AreEqual(250, retorno.valor);
        }

        [Test]
        public void DeveGerarPagamentoParaLeilaoEncerradoComOMaiorValorSemMockarAvaliador()
        {
            var leilaoDao = new Mock<LeilaoDaoFalso>();
            var pagamentoDao = new Mock<PagamentoDao>();

            var leilao1 = new Leilao("Playstation");
            leilao1.propoe(new Lance(new Usuario("Fulano"), 100));
            leilao1.propoe(new Lance(new Usuario("Ciclano"), 150));
            leilao1.propoe(new Lance(new Usuario("Beltrano"), 250));

            leilao1.naData(DateTime.Today.AddDays(-7));

            var leiloes = new List<Leilao>();
            leiloes.Add(leilao1);

            leilaoDao.Setup(m => m.encerrados()).Returns(leiloes);

            Pagamento retorno = null;
            pagamentoDao.Setup(p => p.Salva(It.IsAny<Pagamento>())).
                Callback<Pagamento>(r => retorno = r);

            var gerador = new GeradorDePagamento(leilaoDao.Object, new Avaliador(), 
                pagamentoDao.Object);

            gerador.Gera();

            Assert.AreEqual(250, retorno.valor);
        }

        [Test]
        public void DeveEmpurrarSabadoParaOProximoDiaUtil()
        {
            var leilaoDao = new Mock<LeilaoDaoFalso>();
            var pagamentoDao = new Mock<PagamentoDao>();
            var relogio = new Mock<IRelogio>();

            relogio.Setup(r => r.Hoje()).Returns(new DateTime(2016, 07, 02));

            Leilao leilao1 = new Leilao("Playstation");
            leilao1.propoe(new Lance(new Usuario("Renan"), 500));
            leilao1.propoe(new Lance(new Usuario("Felipe"), 1500));

            List<Leilao> leiloes = new List<Leilao>();
            leiloes.Add(leilao1);

            leilaoDao.Setup(l => l.encerrados()).Returns(leiloes);

            Pagamento pagamento = null;
            pagamentoDao.Setup(p => p.Salva(It.IsAny<Pagamento>())).Callback<Pagamento>(r => pagamento = r);

            GeradorDePagamento gerador = new GeradorDePagamento(leilaoDao.Object, new Avaliador(), pagamentoDao.Object, relogio.Object);
            gerador.Gera();

            Assert.AreEqual(DayOfWeek.Monday, pagamento.data.DayOfWeek);
        }

        [Test]
        public void DeveEmpurrarDomingoParaOProximoDiaUtil()
        {
            var leilaoDao = new Mock<LeilaoDaoFalso>();
            var pagamentoDao = new Mock<PagamentoDao>();
            var relogio = new Mock<IRelogio>();

            relogio.Setup(r => r.Hoje()).Returns(new DateTime(2016, 07, 03));

            Leilao leilao1 = new Leilao("Playstation");
            leilao1.propoe(new Lance(new Usuario("Renan"), 500));
            leilao1.propoe(new Lance(new Usuario("Felipe"), 1500));

            List<Leilao> leiloes = new List<Leilao>();
            leiloes.Add(leilao1);

            leilaoDao.Setup(l => l.encerrados()).Returns(leiloes);

            Pagamento pagamento = null;
            pagamentoDao.Setup(p => p.Salva(It.IsAny<Pagamento>())).Callback<Pagamento>(r => pagamento = r);

            GeradorDePagamento gerador = new GeradorDePagamento(leilaoDao.Object, new Avaliador(), pagamentoDao.Object, relogio.Object);
            gerador.Gera();

            Assert.AreEqual(DayOfWeek.Monday, pagamento.data.DayOfWeek);
        }


    }
}
