using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Almoxarifado.Models;

namespace Almoxarifado.repo
{
    public interface IProdutoRepository
    {
        Produtos addProduto(Produtos produto);
        List<Produtos> listarTodos();
        Produtos obterPorId(int id);
        Produtos atualizarProduto(Produtos produto);
        void deletarProduto(int id);
        bool darEntrada(int id, int quantidade);
        bool darSaida(int id, int quantidade);
        List<Produtos> listarProdutosCriticos();
    }
}