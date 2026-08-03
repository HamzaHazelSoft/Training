using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Training.Models;

public partial class TrainingContext : IdentityDbContext<User>
{
    public TrainingContext()
    {
    }

    public TrainingContext(DbContextOptions<TrainingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    
}
