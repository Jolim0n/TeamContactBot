
namespace TeamContactTelegramBot.Domain.Task
{
    public class TaskDTO
    {
        public int Rcd { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ProgRcd { get; set; }
        public string ProgName { get; set; }
        public int AnalystRcd { get; set; }
        public string AnalystName { get; set; }
        public string Description { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateEnd { get; set; }
        public byte State { get; set; }
        public string StateName { get; set; }
    }
}
