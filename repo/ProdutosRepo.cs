using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Almoxarifado.Db;
using Almoxarifado.Models;
using Almoxarifado.repo;
namespace Almoxarifado.repo
{
    public class ProdutosRepo : IProdutoRepository
    {

        private readonly Context _context;

        public ProdutosRepo(Context context)
        {
            _context = context;
        }

        public Produtos addProduto(Produtos produto)
        {
            _context.produtos.Add(produto);
            _context.SaveChanges();
            return produto;
        }

        public List<Produtos> listarTodos()
        {
           return _context.produtos.ToList();
        }

        public Produtos obterPorId(int id)
        {
            return _context.produtos.Find(id);
        }

        public Produtos atualizarProduto(Produtos produto)
        {
            _context.produtos.Update(produto);
            _context.SaveChanges();
            return produto;
        }

        public void deletarProduto(int id)
        {
            var produto = _context.produtos.Find(id);
            if (produto != null)
            {
                _context.produtos.Remove(produto);
                _context.SaveChanges();
            }
        }

        public bool darEntrada(int id, int quantidade)
        {
            var produto = _context.produtos.Find(id);
            if (produto == null || quantidade <= 0)
            {
                return false;
            }
            
            produto.quantidade += quantidade;
            produto.dataEntrada = DateTime.Now;
            _context.SaveChanges();
            return true;
        }

        public bool darSaida(int id, int quantidade)
        {
            var produto = _context.produtos.Find(id);
            if (produto == null || quantidade <= 0 || produto.quantidade < quantidade)
            {
                return false;
            }
            
            produto.quantidade -= quantidade;
            _context.SaveChanges();
            return true;
        }

        public List<Produtos> listarProdutosCriticos()
        {
            return _context.produtos
                .Where(p => p.quantidade <= p.estoqueMinimo)
                .OrderBy(p => p.quantidade)
                .ToList();
        }
    }
}