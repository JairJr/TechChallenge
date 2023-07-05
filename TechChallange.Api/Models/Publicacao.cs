using System.ComponentModel.DataAnnotations;

namespace TechChallangeApi.Models
{
    public class Publicacao
    {
        public Publicacao()
        {
        }
        public Publicacao(string nome, Guid usuarioId, int fotoID)
        {
            Nome = nome;
            DataEnvio = DateTime.Now;
            UsuarioId = usuarioId;
            FotoId = fotoID;
            Ativa = true;
        }

        [Key]
        [Required]
        public int Id { get; set; }
        public string Nome { get; set; }
        public virtual Foto Foto { get; set; }
        public DateTime DataEnvio { get; set; }
        public Guid UsuarioId { get; set; }
        public int FotoId { get; set; }
        public bool Ativa { get; private set; }

        public void Desativar()
        {
            Ativa = false;
        }
}
}
