using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CodeFirstStoreFunctions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OpenSourceTees.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string UserRole { get; set; }

        public ApplicationUser()
        {
            UserImages = new HashSet<Image>();
        }
        public ICollection<Image> UserImages { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Image> Images { get; set; }
        public DbSet<KeyRank> KeyRanks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Add(new FunctionsConvention<ApplicationDbContext>("dbo"));
        }

        [DbFunction(nameof(ApplicationDbContext), nameof(udf_imageSearch))]
        public IQueryable<KeyRank> udf_imageSearch(string keywords, int? skipN, int? takeN)
        {
            //throw new NotSupportedException();
            var querykeywordsParameter = keywords != null ?
                                              new ObjectParameter("keywords", keywords) :
                                              new ObjectParameter("keywords", typeof(string));
            var queryskipNParameter = skipN != null ?
                                              new ObjectParameter("SkipN", skipN) :
                                              new ObjectParameter("SkipN", typeof(int?));
            var querytakeNParameter = takeN != null ?
                                              new ObjectParameter("TakeN", takeN) :
                                              new ObjectParameter("TakeN", typeof(int?));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<KeyRank>(
              $"[{nameof(ApplicationDbContext)}].[udf_imageSearch](@keywords, @SkipN, @TakeN)",
              new ObjectParameter[] { querykeywordsParameter, queryskipNParameter, querytakeNParameter });
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}