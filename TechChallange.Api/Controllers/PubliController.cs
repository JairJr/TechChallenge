using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using TechChallangeApi.Data;
using TechChallangeApi.Models;
using TechChallengeApi.Models;

namespace TechChallangeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PubliController : ControllerBase
    {

        private readonly ILogger<PubliController> _logger;
        private readonly IPublicacaoRepository _publicacaoRepository;
        private readonly FotoController _fotoController;

        public PubliController(ILogger<PubliController> logger, IPublicacaoRepository publicacaoRepository, FotoController fotoController)
        {
            _logger = logger;
            _publicacaoRepository = publicacaoRepository;
            _fotoController = fotoController;
        }

        [HttpGet()]
        [SwaggerOperation(Summary = "Publicações do usuário", Description = "Retorna Lista com todas as publicações do usuário que possue o ID informado")]
        public IActionResult PublicacoesByUserId()
        {
            try
            {
                var idUsuario = ObterUsuarioId();

                if (idUsuario == Guid.Empty)
                    return Unauthorized();

                var publicacoes = _publicacaoRepository.GetPublicacoes(idUsuario);

                return Ok(publicacoes);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("{usuarioId}/Date")]
        [SwaggerOperation(Summary = "Publicação do usuário em data específica", Description = "Retorna Lista com todas as publicações na data informada e publicadas pelo usuário que possue o ID informado")]
        public IActionResult PublicacoesByUserIdAndDate(Guid usuarioId, [FromBody] string date)
        {
            DateTime dataComparacao = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            IEnumerable<Publicacao> publicacoes = _publicacaoRepository.GetPublicacoes(usuarioId).Where(p => p.DataEnvio.Date.CompareTo(dataComparacao.Date) == 0);

            return Ok(publicacoes);

        }
        //string nome, Usuario usuario, string urlPerfil, Foto foto
        [HttpPost("New")]
        [SwaggerOperation(Summary = "Fazer uma publicação", Description = "Envia uma foto para ser registrada no storage e no DB com as informações do usuário")]
        public async Task<IActionResult> NewPublicacao(PuclicacaoInsert puclicacao)
        {
            try
            {
                if (string.IsNullOrEmpty(puclicacao.Nome)) return BadRequest();

                var idUsuario = ObterUsuarioId();

                if (idUsuario == Guid.Empty)
                    return Unauthorized();

                Publicacao publicacao = await _publicacaoRepository.AddPublicacao(new Publicacao(puclicacao.Nome, idUsuario, puclicacao.FotoId));

                return Ok(publicacao);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{publicacaoId}")]
		public async Task<IActionResult> ExcluirPublicacao(int publicacaoId)
		{
			try
			{
				var idUsuario = ObterUsuarioId();

				if (idUsuario == Guid.Empty)
					return Unauthorized();

				_publicacaoRepository.DeletePublicacaoAnalogicamente(publicacaoId);

				return Ok();
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		private Guid ObterUsuarioId()
		{
            try
            {
				var usuarioId = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

				if (usuarioId == null) return Guid.Empty;

                

				return new Guid(usuarioId);
			}
            catch (Exception)
            {
                return Guid.Empty;
            }
		}
	}
}