namespace ConsultorioUI.Models
{
    public class CategoriaAgendamento
    {
        public int? Id { get; set; }
        public string? Nome { get; set; }

        public static List<CategoriaAgendamento> GetCategoriaAgendamento()
        {
            return new List<CategoriaAgendamento>
            {
                new CategoriaAgendamento() { Id = 1, Nome = "Médico" },
                new CategoriaAgendamento() { Id = 2, Nome = "Terapia" },
                new CategoriaAgendamento() { Id = 3, Nome = "Supervisão" },
                new CategoriaAgendamento() { Id = 4, Nome = "Grupo de Estudos" },
                new CategoriaAgendamento() { Id = 5, Nome = "Clube do livro psicanálise" },
                new CategoriaAgendamento() { Id = 6, Nome = "Clube do livro" },
                new CategoriaAgendamento() { Id = 7, Nome = "Textos de Freud" },
                new CategoriaAgendamento() { Id = 8, Nome = "Confraria" },
                new CategoriaAgendamento() { Id = 9, Nome = "Psicanálise" },
                new CategoriaAgendamento() { Id = 10, Nome = "Francês" },
                new CategoriaAgendamento() { Id = 11, Nome = "Outros" },
            };
        }
    }

    public static class CategoriaAgendamentoExtensions
    {
        public static string ObterNomePorIdObrigatorio(this List<CategoriaAgendamento> categorias, int id)
        {
            var item = categorias.FirstOrDefault(c => c.Id == id);
            if (item is null)
                throw new ArgumentException($"Categoria com Id {id} não encontrada.");
            return item.Nome;
        }
    }
}
