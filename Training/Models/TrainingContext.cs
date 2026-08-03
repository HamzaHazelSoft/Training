using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Training.Models;

public partial class TrainingContext : DbContext
{
    public TrainingContext()
    {
    }

    public TrainingContext(DbContextOptions<TrainingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Student> Students { get; set; }

    
}
