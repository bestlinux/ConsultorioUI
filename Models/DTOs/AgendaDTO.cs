using ConsultorioUI.Pages;

namespace ConsultorioUI.Models.DTOs
{
    public class AgendaDTO
    {
        public int Id { get; set; }

        //LISTA DE PACIENTES
        //DATA INICIO E DATA FIM
        //MODALIDADE: PRIMEIRA CONSULTA, ONLINE E PRESENCIAL
        //LINK ATENDIMENTOS - APARECER SOMENTE QUANDO FOR ONLINE
        //STATUS CONSULTA 1- ATENDIDOD = 2 - FALTOU 3 - DESMARTCAO
        //VALOR DA CONSULTA
        //ao cadastrar o paciente, deve ser inserido também a data de aniversario na agenda
        public int? PacienteId { get; set; }

        public string? PacienteNome { get; set; }

        public string? CPFPagador { get; set; }

        public string? CPF { get; set; }

        //1 - PRIMEIRO ATENDIMENTO
        //2 - CONSULTA ONLINE
        //3 - CONSULTA PRESENCIAL
        //4 - ANIVERSARIO
        public int TipoConsulta { get; set; }

        //Id = 1, Nome = "Médico" },
        //Id = 2, Nome = "Terapia" },
        //Id = 3, Nome = "Supervisão" },
        //Id = 4, Nome = "Grupo de Estudos" },
        //Id = 5, Nome = "Clube do livro psicanálise" }
        //Id = 6, Nome = "Clube do livro" },
        //Id = 7, Nome = "Textos de Freud" },
        //Id = 8, Nome = "Confraria" },
        //Id = 9, Nome = "Psicanálise" },
        //Id = 10, Nome = "Francês" },
        //Id = 11, Nome = "Outros" },
        public int? CategoriaAgendamento { get; set; }

        //1- ATENDIDOD  2 - FALTOU 3 - DESMARTCAO
        public int StatusConsulta { get; set; }

        public DateTime InicioSessao { get; set; }
        public DateTime FimSessao { get; set; }

        public double ValorSessao { get; set; }

        public int TipoRecorrencia { get; set; }

        public int NumeroRecorrencias { get; set; }

        public bool EmailEnviado { get; set; }

        public bool EmailAgendamento { get; set; }

        public string? TextoAgenda { get; set; }

        public string? Observacoes { get; set; }

    }
}
