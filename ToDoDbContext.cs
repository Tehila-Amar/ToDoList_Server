// using System;
// using System.Collections.Generic;
// using Microsoft.EntityFrameworkCore;
// using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

// namespace TodoApi;

// public partial class ToDoDbContext : DbContext
// {
//     public ToDoDbContext()
//     {
//     }

//     public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
//         : base(options)
//     {
//     }

//     public virtual DbSet<Item> Items { get; set; }

//     // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//     // => optionsBuilder.UseMySql(
//     //     "server=be7s8bagknqyvcd69kvz-mysql.services.clever-cloud.com;port=3306;database=be7s8bagknqyvcd69kvz;user=u1mdlwzfmorieody;password=ylFYzvqNwoL5TloE52gt;", 
//     //     Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.35-mysql"));


//     // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//     //     => optionsBuilder.UseMySql("name=ToDoDB", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.35-mysql"));

//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         modelBuilder
//             .UseCollation("utf8mb4_0900_ai_ci")
//             .HasCharSet("utf8mb4");

//         modelBuilder.Entity<Item>(entity =>
//         {
//             entity.HasKey(e => e.Id).HasName("PRIMARY");

//             entity.ToTable("Items");

//             entity.Property(e => e.Name).HasMaxLength(100);

//             entity.Property(e => e.IsComplete).HasColumnName("IsComplete").HasColumnType("tinyint(1)");
//         });

//         OnModelCreatingPartial(modelBuilder);
//     }

//     partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
// }

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TodoApi
{
    public partial class ToDoDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ToDoDbContext(DbContextOptions<ToDoDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Item> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("ToDoDB");
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("items");

                entity.Property(e => e.Name).HasMaxLength(100);
                
                // הוספת העמודה IsComplete
                entity.Property(e => e.IsComplete)
                      .HasColumnType("BOOLEAN")
                      .HasDefaultValue(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
    
}

