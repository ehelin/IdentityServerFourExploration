using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using IdentityModel;
using System.Linq;
using IdentityServer4;
using IdentityServer4.Test;

namespace IdentityServerFour.Models
{
    public class UserModel
    {
        public UserModel() { }

        public UserModel(TestUser user)
        {
            this.SubjectId = user.SubjectId;
            this.UserName = user.Username;
            this.Password = user.Password;
            this.Name = user.Claims.Where(x => x.Type == "name").Select(x => x.Value).FirstOrDefault();
            this.GivenName = user.Claims.Where(x => x.Type == "given_name").Select(x => x.Value).FirstOrDefault();
            this.FamilyName = user.Claims.Where(x => x.Type == "family_name").Select(x => x.Value).FirstOrDefault();
            this.Email = user.Claims.Where(x => x.Type == "email").Select(x => x.Value).FirstOrDefault();
            this.Website = user.Claims.Where(x => x.Type == "website").Select(x => x.Value).FirstOrDefault();
        }

        public bool CreateUser { get; set; }
        public string SubjectId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
    }
}
