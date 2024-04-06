﻿using System.ComponentModel.DataAnnotations;

namespace BPM.BLL.Models.User;

public class UserRegisterModel
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }
}