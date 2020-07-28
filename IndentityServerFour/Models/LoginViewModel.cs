using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServerFour.Models
{
    public class LoginViewModel : LoginInputModel
    {
        public bool AllowRememberLogin { get; set; } = true;
        public bool EnableLocalLogin { get; set; } = true;
    }
}