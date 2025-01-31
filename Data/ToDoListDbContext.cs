using Microsoft.EntityFrameworkCore;
using ToDoListApi.Models;

namespace ToDoListApi.Data
{
    public class ToDoListDbContext(DbContextOptions<ToDoListDbContext> options) : DbContext(options)
    {
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<TaskListModel> TaskLists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<TaskModel>()
                .HasOne(e => e.TaskList)
                .WithMany(e => e.TaskModels)
                .HasForeignKey(e => e.TaskListId);
        }
    }
}
