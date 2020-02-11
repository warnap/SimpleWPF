using Microsoft.EntityFrameworkCore;

namespace SimpleWPF.Models
{
    public class PrehPesonsContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=PrehPersons.db");
    }

    public enum Gender
    {
        Male,
        Female
    }

    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
    }
}
