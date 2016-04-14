using mock.dominio;
using System.Collections.Generic;


namespace mock.interfaces
{
    public interface IRepositorioDeLeiloes
    {
        void salva(Leilao leilao);
        List<Leilao> encerrados();
        List<Leilao> correntes();
        void atualiza(Leilao leilao);
    }
}
