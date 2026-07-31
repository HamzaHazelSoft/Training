using System;
using System.Collections.Generic;

namespace Training.Models;

public partial class Student
{
    public int RollNumber { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public DateOnly? Dob { get; set; }

    public string? PhoneNumber { get; set; }
}
