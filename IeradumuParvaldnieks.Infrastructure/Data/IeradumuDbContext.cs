using IeradumuParvaldnieks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IeradumuParvaldnieks.Infrastructure.Data
{
    public class IeradumuDbContext : DbContext
    {
        public IeradumuDbContext(DbContextOptions<IeradumuDbContext> options) : base(options)
        {
        }

        public DbSet<Ieradums> Ieradumi => Set<Ieradums>(); //Datubāzes tabulas

        public DbSet<IeradumaIzpilde> IeradumuIzpildes => Set<IeradumaIzpilde>();
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Ieradumu tabula
            modelBuilder.Entity<Ieradums>(entity =>
            {
                entity.HasKey(i => i.Id);

                entity.Property(i => i.Nosaukums).IsRequired().HasMaxLength(100);

                entity.OwnsOne(i => i.Atkartosana); 
            });

            modelBuilder.Entity<IeradumaIzpilde>(entity =>//Ieradumu izpildes tabula
            {
                entity.HasKey(i => i.Id);

                entity.Property(i => i.Datums).HasColumnType("date");//Glabā tikai datumu

                entity.HasOne<Ieradums>().WithMany(i => i.Izpildes).HasForeignKey(i => i.IeradumaId).OnDelete(DeleteBehavior.Cascade); //Vienam ieradumam var būt vairākas izpildes

                entity.HasIndex(i => new { i.IeradumaId, i.Datums }).IsUnique(); // Ieradumam nedrīkst būt divas vienādas izpildes
            });
        }
    }
}