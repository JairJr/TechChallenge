using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechChallangeApi.Models;

namespace TechChallangeApi.Data
{
    public class PublicacaoRepository : IPublicacaoRepository
    {

        public ApplicationContext context;

        public PublicacaoRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<Publicacao> AddPublicacao(Publicacao publicacao)
        {
            try
            {
                if (publicacao == null) throw new ArgumentNullException(nameof(publicacao));
                await context.Set<Publicacao>().AddAsync(publicacao);
                await context.SaveChangesAsync();
                return publicacao;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void DeletePublicacao(int id)
        {
            try
            {
                Publicacao publicacaoResultado = GetPublicacaoById(id);
                if (publicacaoResultado == null) throw new ArgumentNullException(nameof(id));
                context.Set<Publicacao>().Remove(publicacaoResultado);
                context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

		public void DeletePublicacaoAnalogicamente(int id)
		{
            var publicacao = context.Publicacoes.Where(x => x.Id == id).FirstOrDefault();

            if (publicacao == null)
                throw new ArgumentException("Náo foi possivel encontrar a publicação"); 

            publicacao.Desativar();

            context.Update(publicacao);

            context.SaveChanges();
		}

		public Publicacao GetPublicacaoById(int id)
        {
            try
            {

                Publicacao publicacaoResultado = context.Publicacoes.FirstOrDefault(p => p.Id == id);
                if (publicacaoResultado == null) throw new Exception("Publicação não encontrada");
                return publicacaoResultado;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //public Publicacao GetPublicacaoByUsuarioEData(/*Usuario usuario, */DateTime dateTime)
        //{
        //    try
        //    {

        //        //Publicacao publicacaoResultado = context.Publicacoes.FirstOrDefault(p => p.Usuario == usuario && p.DataEnvio == dateTime);
        //        //if (publicacaoResultado == null) throw new Exception("Publicação não encontrada");
        //        //return publicacaoResultado;

        //        return null;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public IEnumerable<object> GetPublicacoes(Guid? usuarioId)
        {
            try
            {
                var publicacoesResultado = context.Publicacoes
                                                  .Include(x => x.Foto)
                                                  .Where(p => p.UsuarioId == usuarioId && p.Ativa)
                                                  .Select(x => new { id = x.Id,  nome =  x.Nome, urlPerfil = x.Foto.Url})
                                                  .ToList();

                if (publicacoesResultado == null) throw new Exception("Publicações não encontradas para esse usuário");
                return publicacoesResultado;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

		public IEnumerable<Publicacao> GetPublicacoes(Guid usuarioId)
		{
			try
			{
				var publicacoesResultado = context.Publicacoes
												  .Include(x => x.Foto)
												  .Where(p => p.UsuarioId == usuarioId)
												  .ToList();

				if (publicacoesResultado == null) throw new Exception("Publicações não encontradas para esse usuário");
				return publicacoesResultado;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}
