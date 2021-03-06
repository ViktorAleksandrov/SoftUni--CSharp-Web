﻿using Microsoft.AspNetCore.Identity;

namespace Eventures.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UniqueCitizenNumber { get; set; }
    }
}
