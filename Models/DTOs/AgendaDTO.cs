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

        //1 - PRIMEIRO ATENDIMENTO
        //2 - CONSULTA ONLINE
        //3 - CONSULTA PRESENCIAL
        public int TipoConsulta { get; set; }

        //1- ATENDIDOD  2 - FALTOU 3 - DESMARTCAO
        public int StatusConsulta { get; set; }

        public DateTime InicioSessao { get; set; }
        public DateTime FimSessao { get; set; }

        public double ValorSessao { get; set; }

    }
}
