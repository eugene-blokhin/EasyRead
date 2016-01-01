using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using EasyRead.Core.Model;

namespace EasyRead.Core.DataAccess
{
    public class EasyReadDbContext : DbContext
    {
        public EasyReadDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<LoginAuthentication> LoginsAuthentication { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var users = modelBuilder.Entity<User>();

            users.ToTable("Users").HasKey(user => user.Id);
            users.Property(user => user.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            users.Property(user => user.Name).IsRequired();
            users.Property(user => user.Email).IsRequired();

            var logins = modelBuilder.Entity<LoginAuthentication>();
            logins.ToTable("Logins").HasKey(authentication => authentication.Id);
            logins.Property(authentication => authentication.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            logins.Property(authentication => authentication.UserId).HasColumnAnnotation("IX_Login_UserId", new IndexAttribute("IX_Login_UserId"));

            //TODO : Think how I can check referential integrity here without need to work with navigation properties.
//            Database.ExecuteSqlCommand(@"
//                if object_id('FK_Logins_Users') is null
//	            alter table [Logins]
//	            add constraint [FK_Logins_Users] 
//	            foreign key ([UserId])	references [Users]([Id])");
        }
    }
}

