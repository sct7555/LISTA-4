using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using LISTA_4;
using System.Diagnostics.CodeAnalysis;

namespace LISTA_4.Controllers
{
    [ApiController]
    [Route("API/PESSOA")]
    public class PessoaController : ControllerBase
    {
        // Static para não criar nova lista toda vez que chamar a função
        private static List<Pessoa> listaPessoas = new List<Pessoa>();

        [HttpPost]
        [Route("AdicionarPessoa")]
        public IActionResult AdicionarPessoa(Pessoa pessoa)
        {
            listaPessoas.Add(pessoa);
            return Ok();
        }

        [HttpGet]
        [Route("BuscarTodos")]
        public IActionResult BuscarTodos()
        {
            return Ok(listaPessoas);
        }

        [HttpGet]
        [Route("BuscarPessoaPorCPF")]
        public IActionResult BuscarPessoaCPF(string cpf)
        {
            var pessoaPesquisada = listaPessoas.FirstOrDefault(a => a.CPF == cpf);

            if (pessoaPesquisada == null)
            {
                return NotFound($"{cpf} não encontrado!");
            }
            return Ok(pessoaPesquisada);
        }

        [HttpDelete]
        [Route("RemoverPessoa")]
        public IActionResult RemoverPessoa(string cpf)
        {
            var pessoaRemover = listaPessoas.FirstOrDefault(a => a.CPF == cpf);

            if (pessoaRemover == null)
            {
                return NotFound($"{cpf} não encontrado!");
            }
            listaPessoas.Remove(pessoaRemover);

            return Ok("Pessoa removida");
        }

        [HttpPut]
        [Route("AtualizarPessoa")]
        public IActionResult AtualizarPessoa(string cpfBusca, Pessoa pessoaAtualizada)
        {
            var resultadoBusca = listaPessoas.FirstOrDefault(p => p.CPF == cpfBusca);

            if (resultadoBusca == null)
            {
                return NotFound($"O CPF {cpfBusca} não foi encontrado");
            }
            resultadoBusca.Nome = pessoaAtualizada.Nome;
            resultadoBusca.Peso = pessoaAtualizada.Peso;
            resultadoBusca.Altura = pessoaAtualizada.Altura;

            return Ok();
        }
        [HttpGet]
        [Route("PessoaComIMCBom")]
        public IActionResult MostrarIMC()
        {
            var pessoasComIMCBom = listaPessoas
                .Select(pessoa =>
                {
                    double alturaConvertida = pessoa.Altura / 100.0;
                    double imc = pessoa.Peso / (alturaConvertida * alturaConvertida);
                    return new
                    {
                        pessoa.Nome,
                        pessoa.Peso,
                        pessoa.Altura,
                        pessoa.CPF,
                        IMC = imc
                    };
                })
                .Where(pessoaImc => pessoaImc.IMC >= 18 && pessoaImc.IMC <= 24)
                .ToList();

            if (!pessoasComIMCBom.Any())
            {
                return NotFound("Nenhuma pessoa com IMC dentro da faixa recomendada.");
            }

            return Ok(pessoasComIMCBom);
        }

        [HttpGet]
        [Route("BuscarPorNome")]
        public IActionResult BuscarPorNome(string nome)
        {
            var pessoasEncontradas = listaPessoas
                .Where(p => p.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!pessoasEncontradas.Any())
            {
                return NotFound($"Nenhuma pessoa encontrada com o nome '{nome}'.");
            }

            return Ok(pessoasEncontradas);
        }
    }
}