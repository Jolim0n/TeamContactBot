using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamContactTelegramBot.Data.Tasks
{
    [Table("Tasks")]
    public class Task
    {
        [Key]
        [Column("Task_Rcd")]
        public int Rcd { get; set; }

        [MaxLength(50)]
        [Column("Task_Code")]
        public string Code { get; set; }

        [MaxLength(255)]
        [Column("Task_Name")]
        public string Name { get; set; }

        [Column("Task_ProgRcd")]
        public int ProgRcd { get; set; }

        [Column("Task_AnalystRcd")]
        public int AnalystRcd { get; set; }

        [Column("Task_Desc")]
        public string Description { get; set; }

        [Column("Task_DtCreate")]
        public DateTime DateCreate { get; set; }

        [Column("Task_DtEnd")]
        public DateTime DateEnd { get; set; }

        [Column("Task_State")]
        public byte State { get; set; }
    }
}
