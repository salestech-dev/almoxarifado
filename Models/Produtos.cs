using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Almoxarifado.Models
{
    public class Produtos
    {
        public int id { get; set; }
        public string nome { get; set; }
        public int quantidade { get; set; }
        public DateTime dataEntrada { get; set; }
        public string peso { get; set; }
        public int estoqueMinimo { get; set; } = 10; 
        
       
        public string StatusEstoque 
        { 
            get 
            {
                if (quantidade == 0)
                    return "Esgotado";
                else if (quantidade <= estoqueMinimo)
                    return "Critico";
                else if (quantidade <= estoqueMinimo * 2)
                    return "Atencao";
                else
                    return "Normal";
            }
        }
        
       
        public string CorAlerta
        {
            get
            {
                return StatusEstoque switch
                {
                    "Esgotado" => "danger",
                    "Critico" => "danger",
                    "Atencao" => "warning",
                    "Normal" => "success",
                    _ => "secondary"
                };
            }
        }
    }
}