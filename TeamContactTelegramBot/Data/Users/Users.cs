using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TeamContactTelegramBot.Data.Users
{
    [Table("Users")]
    public class Users
    {
        [Column("Users_Id")]
        public int UsersId { get; set; }

        [MaxLength(255)]
        [Column("Users_Code")]
        public string Code { get; set; }

        [MaxLength(255)]
        [Column("Users_Name")]
        public string Name { get; set; }

        [MaxLength(255)]
        [Column("Users_Login")]
        public string Login { get; set; }

        [MaxLength(255)]
        [Column("Users_Password")]
        public string Password { get; set; }

        [Column("Users_Role")]
        public byte Role { get; set; }

        //public static void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Users>()
        //        .ToTable("Users")
        //        .Property(e => e.UsersId)
        //        .HasColumnName("Users_Id");
        //}
    }
}
