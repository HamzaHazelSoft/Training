using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Training.Models;

public  class Student
{

    [Key]
    public int RollNumber { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Gender { get; set; }

    public DateOnly Dob { get; set; }

    public string PhoneNumber { get; set; }
}
