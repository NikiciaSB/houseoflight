using Microsoft.EntityFrameworkCore;

namespace HouseOfLight.Models
{
    public class HouseContext:DbContext
    {
        public HouseContext(DbContextOptions<HouseContext> options) : base(options) { }
        public DbSet<Message> Messages {get;set;}
        public DbSet<Reg> Users {get;set;}
        public DbSet<Comment> Comments {get;set;}
    }
}