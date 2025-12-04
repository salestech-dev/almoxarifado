using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Almoxarifado.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Almoxarifado.repo;

namespace Almoxarifado.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ProdutosController : Controller
    {
        private readonly ILogger<ProdutosController> _logger;
        private readonly IProdutoRepository _produtoRepository;

        public ProdutosController(ILogger<ProdutosController> logger, IProdutoRepository produtoRepository)
        {
            _logger = logger;
            _produtoRepository = produtoRepository;
        }


        [HttpPost]
        [Route("Add")]
        public IActionResult AddProduto(Produtos produto)
        {
            _produtoRepository.addProduto(produto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Criar")]
        public IActionResult Criar()
        {
            return View();
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var produto = _produtoRepository.obterPorId(id);
            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult EditProduto(Produtos produto)
        {
            _produtoRepository.atualizarProduto(produto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            _produtoRepository.deletarProduto(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public IActionResult Index(string busca)
        {
            var produtos = _produtoRepository.listarTodos();
            
            // Aplicar filtro de busca se fornecido
            if (!string.IsNullOrWhiteSpace(busca))
            {
                produtos = produtos.Where(p => 
                    p.nome.Contains(busca, StringComparison.OrdinalIgnoreCase) ||
                    p.id.ToString().Contains(busca)
                ).ToList();
            }
            
            // Ordenar alfabeticamente por nome
            produtos = produtos.OrderBy(p => p.nome).ToList();
            
            // Contar produtos críticos para exibir alerta
            var produtosCriticos = produtos.Count(p => p.StatusEstoque == "Critico" || p.StatusEstoque == "Esgotado");
            ViewBag.ProdutosCriticos = produtosCriticos;
            ViewBag.BuscaAtual = busca;
            
            return View(produtos);
        }

        [HttpPost]
        [Route("Entrada/{id}")]
        public IActionResult DarEntrada(int id, int quantidade)
        {
            if (quantidade <= 0)
            {
                TempData["Erro"] = "Quantidade precisa ser maior que zero!";
                return RedirectToAction("Index");
            }

            var sucesso = _produtoRepository.darEntrada(id, quantidade);
            
            if (sucesso)
            {
                TempData["Sucesso"] = $"Entrada de {quantidade} unidades registrada com sucesso!";
            }
            else
            {
                TempData["Erro"] = "Erro ao dar entrada no produto.";
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Saida/{id}")]
        public IActionResult DarSaida(int id, int quantidade)
        {
            if (quantidade <= 0)
            {
                TempData["Erro"] = "Quantidade precisa ser maior que zero!";
                return RedirectToAction("Index");
            }

            var produto = _produtoRepository.obterPorId(id);
            
            if (produto == null)
            {
                TempData["Erro"] = "Produto não encontrado!";
                return RedirectToAction("Index");
            }

            if (produto.quantidade < quantidade)
            {
                TempData["Erro"] = $"Estoque insuficiente! Disponível: {produto.quantidade} unidades.";
                return RedirectToAction("Index");
            }

            var sucesso = _produtoRepository.darSaida(id, quantidade);
            
            if (sucesso)
            {
                var produtoAtualizado = _produtoRepository.obterPorId(id);
                TempData["Sucesso"] = $"Saída de {quantidade} unidades registrada!";
                
                // Alerta se ficou crítico após a saída
                if (produtoAtualizado.StatusEstoque == "Critico")
                {
                    TempData["Alerta"] = $"ATENÇÃO: O estoque de {produto.nome} está CRÍTICO! Apenas {produtoAtualizado.quantidade} unidades restantes.";
                }
                else if (produtoAtualizado.StatusEstoque == "Esgotado")
                {
                    TempData["Alerta"] = $"ATENÇÃO: O estoque de {produto.nome} está ESGOTADO!";
                }
            }
            else
            {
                TempData["Erro"] = "Erro ao dar saída no produto.";
            }
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Criticos")]
        public IActionResult ProdutosCriticos()
        {
            var produtos = _produtoRepository.listarProdutosCriticos();
            return View(produtos);
        }

        [HttpGet]
        [Route("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}