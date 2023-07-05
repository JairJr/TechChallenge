using Azure.Core;
using Azure.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Security.AccessControl;
using TechChallangeApi.Data;
using TechChallangeApi.Models;
using TechChallenge.Api.Factory;
using static System.Reflection.Metadata.BlobBuilder;

namespace TechChallangeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
	//[Authorize]
	public class FotoController : ControllerBase
    {
        private readonly ILogger<FotoController> _logger;
        private readonly IFotosRepository _fotoRepository;
        private readonly ILogFactory _logFactory;
		private readonly HttpClient _httpClient;
        private readonly Guid _usuarioId;
        private readonly Serilog.ILogger _serilog;

        public FotoController(ILogger<FotoController> logger, IFotosRepository fotoRepository, HttpClient httpClient, ILogFactory logFactory)
        {
            _logger = logger;
            _fotoRepository = fotoRepository;
            _httpClient = httpClient;
            _logFactory = logFactory;
		}

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Foto por Id", Description = "Retorna a foto respectiva do Id")]
        public IActionResult FotoById(int id)
        {
            Foto publicacoes = _fotoRepository.GetFoto(id);

            return Ok(publicacoes);

        }

        [HttpPost]
        [SwaggerOperation(Summary = "Salvar foto", Description = "Solicita a criação da foto no Storage e salva informações no DB")]
        public async Task<IActionResult> NewFoto(IFormFile formFile)
		{
            var usuarioId = obterUsuarioId();

            if (usuarioId == null) return Unauthorized();
            // Enviar a requisição e obter a resposta

            // Adicione o conteúdo do arquivo ao HttpContent
            var fileContent = new StreamContent(formFile.OpenReadStream());
			var content = new MultipartFormDataContent();
			content.Add(fileContent, "formFile", formFile.FileName);

			// Defina o cabeçalho "Content-Type" usando o HttpContent
			fileContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);

			var resultPost = await Post(formFile, content);

            if (resultPost == null) return NoContent();

            var fotoResult = await GetFoto(resultPost, fileContent, usuarioId);


			if (fotoResult == null) return NoContent();
            return Ok(fotoResult);

        }

        private async Task<Foto> GetFoto(string resultPost, StreamContent fileContent, Guid usuarioId)
        {
            try
            {
			    var requestURLGet = "http://localhost:5012/GetSpecific";

			    var requestURLGetString = requestURLGet + "?blobName=" + resultPost.Substring(1, resultPost.Length - 2);

				LogRaw("Integração minimal API - Buscar foto", requestURLGetString, true);

				HttpResponseMessage responseGet = await _httpClient.GetAsync(requestURLGetString);
    			if (!responseGet.IsSuccessStatusCode) return null;
	    		string responseGetString = await responseGet.Content.ReadAsStringAsync();
				LogRaw("Integração minimal API - Buscar foto", responseGetString, true);
		    	string url = responseGetString.Substring(1, responseGetString.IndexOf('?'));
				string extensao = Path.GetExtension(fileContent.Headers.ContentDisposition.FileName);

                return await _fotoRepository.AddFoto(new Foto(true, url, extensao, usuarioId));
			}
            catch (Exception e)
            {
				LogRaw("Integração minimal API - Publicar foto", e.Message);
				return null;
			}
}

        private async Task<string> Post(IFormFile formFile, MultipartFormDataContent content)
        {
			try
			{

				// Criar uma requisição HTTP POST com o corpo e cabeçalhos desejados
				var requestURLPost = "http://localhost:5012/Upload";

				LogRaw("Integração minimal API - Publicar foto", JsonConvert.SerializeObject(content), true);
				HttpResponseMessage responsePost = await _httpClient.PostAsync(requestURLPost, content);

				if (!responsePost.IsSuccessStatusCode) return null;
				string resultPost = await responsePost.Content.ReadAsStringAsync();

				LogRaw("Integração minimal API - Publicar foto", resultPost);

                return resultPost;
			}
			catch (Exception e)
			{
				LogRaw("Integração minimal API - Publicar foto", e.Message);
                return null;
			}
		}

        private Guid obterUsuarioId()
        {
            var usuarioId = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            if (usuarioId == null) return Guid.Empty;

			return new Guid(usuarioId);
		}

        private void LogRaw(string title, string log, bool request = false)
		{
            var metodo = "integração Minimal API";
            var mensagem = $"- [{title}] - {(request ? "[REQUEST]" : "[RESPONSE]}")} - {log}";

            Log.Information(string.Format("Process: {0} Message: {1}", metodo, mensagem));
        }

    }
}