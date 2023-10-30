using System.Data.Entity;

namespace MvcModel.Models
{
  public class MvcModelContext : DbContext
  {
    //public MvcModelContext() : base("MyContext") { }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<Member> Members { get; set; }

    //protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //{
    //  modelBuilder.Entity<Article>()
    //    .ToTable("Contents")
    //    .HasKey(a => a.Url)
    //    .Property(a => a.Url)
    //    .HasColumnName("Address")
    //    .HasMaxLength(200);

    //}
  }
}