using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OpenRA.MasterServer.Legacy;

namespace OpenRA.MasterServer;

public class MasterServerContext : DbContext
{
    public MasterServerContext(DbContextOptions<MasterServerContext> options) : base(options)
    {
    }

    public DbSet<Server> Servers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var converter = new ValueConverter<int[], string>(
            v => string.Join(";", v),
            v => v.Split(";", StringSplitOptions.RemoveEmptyEntries).Select(val => int.Parse((string) val)).ToArray());

        modelBuilder.Entity<Server>(p =>
        {
            p.ToTable("Servers");
            p.HasKey(d => d.Address);

            p.Property(d => d.Address).ValueGeneratedNever();

            p.Property(p => p.DisabledSpawnPoints).HasConversion(converter);

            p.OwnsMany<GameClient>(l => l.Clients, a =>
            {
                a.WithOwner().HasForeignKey(g => g.Address);
                a.Property(p => p.Id);
                a.HasKey(p => p.Id);

                a.ToTable("Clients");
            });
        });

        //modelBuilder.Entity<GameClient>(p =>
        //{
        //    p.ToTable("GameClient");
        //    p.HasKey(d => d.Id);


        //});
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {

        options.UseSqlite(Settings.DbContext);

    }
}