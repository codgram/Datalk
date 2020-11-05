using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datalk.Server.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Datalk.Server.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.Extensions.Options;
using Datalk.Shared.Models;

namespace Datalk.Server.Data
{
    public class DatalkContext : ApiAuthorizationDbContext<DatalkUser>
    {
        public DatalkContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
    

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    

        public DbSet<Datalk.Server.Areas.Identity.Data.DatalkUser> DatalkUser { get; set; }
        public DbSet<Datalk.Shared.Models.Chatroom> Chatroom { get; set; }
    

        public DbSet<Datalk.Shared.Models.ChatroomMember> ChatroomMember { get; set; }

        public DbSet<Datalk.Shared.Models.Message> Message { get; set; }
    }
}
