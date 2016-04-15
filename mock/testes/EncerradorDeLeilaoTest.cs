﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using mock.dominio;
using mock.infra;
using mock.servico;
using mock.interfaces;

namespace mock.testes
{
    [TestFixture]
    public class EncerradorDeLeilaoTest
    {
        [Test]
        public void Deve_Encerrar_Leiloes_Que_Comecaram_Uma_Semana_Antes()
        {
            var data = DateTime.Today.AddDays(-7);

            var leilao1 = new Leilao("Xbox One");
            leilao1.naData(data);
            var leilao2 = new Leilao("Playstation 4");
            leilao2.naData(data);

            var listaLeiloes = new List<Leilao>();
            listaLeiloes.Add(leilao1);
            listaLeiloes.Add(leilao2);

            //criando mock
            var dao = new Mock<IRepositorioDeLeiloes>();
            var carteiro = new Mock<Carteiro>();
            dao.Setup(m => m.correntes()).Returns(listaLeiloes);

            var encerrador = new EncerradorDeLeilao(dao.Object, carteiro.Object);
            encerrador.encerra();

            Assert.AreEqual(2, listaLeiloes.Count);
            Assert.IsTrue(listaLeiloes[0].encerrado);
            Assert.IsTrue(listaLeiloes[1].encerrado);
        }

        [Test]
        public void Nao_Deve_Encerrar_Leiloes_Que_Comecaram_Hoje()
        {
            var data = DateTime.Today;

            var leilao1 = new Leilao("Xbox One");
            leilao1.naData(data);
            var leilao2 = new Leilao("Playstation 4");
            leilao2.naData(data);

            var listaLeiloes = new List<Leilao>();
            listaLeiloes.Add(leilao1);
            listaLeiloes.Add(leilao2);

            //criando mock
            var dao = new Mock<IRepositorioDeLeiloes>();
            var carteiro = new Mock<Carteiro>();
            dao.Setup(m => m.correntes()).Returns(listaLeiloes);

            var encerrador = new EncerradorDeLeilao(dao.Object, carteiro.Object);
            encerrador.encerra();

            Assert.AreEqual(2, listaLeiloes.Count);
            Assert.IsFalse(listaLeiloes[0].encerrado);
            Assert.IsFalse(listaLeiloes[1].encerrado);
        }

        [Test]
        public void Nao_Deve_Encerrar_Nada()
        {
            var dao = new Mock<IRepositorioDeLeiloes>();
            var carteiro = new Mock<Carteiro>();
            dao.Setup(m => m.correntes()).Returns(new List<Leilao>());

            var encerrador = new EncerradorDeLeilao(dao.Object, carteiro.Object);
            encerrador.encerra();

            Assert.AreEqual(0, encerrador.total);
        }

        [Test]
        public void Deve_Verificar_Se_O_Metodo_Atualiza_Foi_Executado()
        {
            var data = DateTime.Today.AddDays(-7);
            var listaLeiloes = new List<Leilao>();

            var leilao = new Leilao("Playstation 4");
            leilao.naData(data);

            listaLeiloes.Add(leilao);

            var dao = new Mock<LeilaoDaoFalso>();
            var carteiro = new Mock<Carteiro>();
            dao.Setup(m => m.correntes()).Returns(listaLeiloes);

            var encerrador = new EncerradorDeLeilao(dao.Object, carteiro.Object);
            encerrador.encerra();

            dao.Verify(m => m.atualiza(leilao), Times.Once);
        }

        [Test]
        public void NaoDeveAtualizaOsLeiloesEncerrados()
        {
            DateTime data = DateTime.Today;

            Leilao leilao1 = new Leilao("Tv 20 polegadas");
            leilao1.naData(data);

            List<Leilao> listaRetorno = new List<Leilao>();
            listaRetorno.Add(leilao1);

            var dao = new Mock<LeilaoDaoFalso>();
            var carteiro = new Mock<Carteiro>();
            dao.Setup(m => m.correntes()).Returns(listaRetorno);

            var encerrador = new EncerradorDeLeilao(dao.Object, carteiro.Object);
            encerrador.encerra();

            dao.Verify(m => m.atualiza(leilao1), Times.Never());
        }
    }
}
