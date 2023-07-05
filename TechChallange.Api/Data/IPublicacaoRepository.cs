using TechChallangeApi.Models;

namespace TechChallangeApi.Data
{
    public interface IPublicacaoRepository
    {
        Task<Publicacao> AddPublicacao(Publicacao publicacao);
        void DeletePublicacao(int id);
		void DeletePublicacaoAnalogicamente(int id);
		Publicacao GetPublicacaoById(int id);
        //Publicacao GetPublicacaoByUsuarioEData(Usuario usuario, DateTime dateTime);
        IEnumerable<Publicacao> GetPublicacoes(Guid usuarioId);
		IEnumerable<object> GetPublicacoes(Guid? usuarioId);
	}
}
