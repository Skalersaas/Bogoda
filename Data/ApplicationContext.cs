using Bogoda.Models;
using Microsoft.EntityFrameworkCore;

namespace Bogoda.Data
{
    // Класс ApplicationContext наследуется от DbContext, который используется для управления базой данных
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
    {
        // DbSet для таблиц Category, Venue, Tag
        public DbSet<Category> Categories { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Tag> Tags { get; set; }


        // Метод для настройки подключения к базе данных, включая использование прокси для ленивой загрузки
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();   // Включение ленивой загрузки
        }

        // Метод для настройки моделей при создании базы данных
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка модели Category
            modelBuilder.Entity<Category>(category =>
            {
                category.Property(c => c.Id).ValueGeneratedOnAdd();  // Автоматическая генерация Id

                category.Property(c => c.Name).IsRequired();  // Поле Name обязательно для заполнения

                // Связь один ко многим: одна категория может иметь много заведений
                category.HasMany(c => c.Venues)
                        .WithOne(v => v.Category)
                        .HasForeignKey(v => v.CategoryId);  // Связывание по внешнему ключу CategoryId
            });

            // Настройка модели Tag
            modelBuilder.Entity<Tag>(tag =>
            {
                tag.Property(c => c.Id).ValueGeneratedOnAdd();  // Автоматическая генерация Id

                tag.Property(c => c.Name).IsRequired();  // Поле Name обязательно для заполнения

                // Много ко многим: связь между Tag и Venue
                tag.HasMany(t => t.Venues)
                   .WithMany(v => v.Tags)
                   .UsingEntity(
                       t => t.HasOne(typeof(Venue)).WithMany().HasForeignKey("VenueId"),  // Связь с Venue по ключу VenueId
                       v => v.HasOne(typeof(Tag)).WithMany().HasForeignKey("TagId"));    // Связь с Tag по ключу TagId
            });

            // Настройка модели Venue
            modelBuilder.Entity<Venue>(venue =>
            {
                venue.Property(v => v.Id).ValueGeneratedOnAdd();  // Автоматическая генерация Id

                venue.Property(v => v.Name).IsRequired();    // Поле Name обязательно для заполнения
                venue.Property(v => v.Address).IsRequired(); // Поле Address обязательно для заполнения
                venue.Property(v => v.CategoryId).IsRequired();  // Внешний ключ CategoryId обязателен
            });
        }
    }
}
