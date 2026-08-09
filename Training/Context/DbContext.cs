using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using UserManagementSystem.Models;

namespace UserManagementSystem.Context;

public partial class DbContext : IdentityDbContext<User>
{
    public DbContext()
    {
    }

    public DbContext(DbContextOptions<DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    
}
