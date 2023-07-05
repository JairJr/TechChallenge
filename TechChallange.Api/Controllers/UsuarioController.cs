using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;
using TechChallangeApi.Data;
using TechChallangeApi.Models;
using TechChallenge.Api.Models;

namespace TechChallenge.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UsuarioController : ControllerBase
	{
		private readonly IUsuarioRepository _usuarioRepository;

		public UsuarioController(IUsuarioRepository usuarioRepository)
		{
			_usuarioRepository = usuarioRepository;
		}

		[HttpPost()]
		public IActionResult PublicacoesByUserIdAndDate(usuarioInsert command)
		{
			try
			{
				var usuario = new Usuario(command.Nome, command.Email, command.Id);

				var result = _usuarioRepository.AddUsuario(usuario);

				return Ok();
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
