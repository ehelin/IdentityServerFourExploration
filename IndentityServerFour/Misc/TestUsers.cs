﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Test;
using IdentityServerFour.Models;
using System.Linq;

namespace IdentityServerFour.Misc
{
    public class TestUsers
    {
        private static List<TestUser> _users = TestUsers.InitializeUsers();

        public static List<TestUser> Users
        {
            get { return _users; }
        }

        public static void AddUser(UserModel model)
        {
            if(model != null)
            {
                var user = new TestUser();

                user.SubjectId = model.SubjectId;
                user.Username = model.UserName;
                user.Password = model.Password;

                user.Claims.Add(new Claim("Name", model.Name));
                user.Claims.Add(new Claim("GivenName", model.GivenName));
                user.Claims.Add(new Claim("FamilyName", model.FamilyName));
                user.Claims.Add(new Claim("Email", model.Email));
                user.Claims.Add(new Claim("Website", model.Website));

                TestUsers.Users.Add(user);
            }
        }

        public static void EditUser(UserModel model)
        {
            if(model != null)
            {
                var user = _users.Where(x => x.SubjectId == model.SubjectId).FirstOrDefault();

                if(user != null)
                {
                    user.SubjectId = model.SubjectId;
                    user.Username = model.UserName;
                    user.Password = model.Password;

                    user.Claims.Clear();

                    user.Claims.Add(new Claim("Name", model.Name));
                    user.Claims.Add(new Claim("GivenName", model.GivenName));
                    user.Claims.Add(new Claim("FamilyName", model.FamilyName));
                    user.Claims.Add(new Claim("Email", model.Email));
                    user.Claims.Add(new Claim("Website", model.Website));
                }
            }
        }

        private static List<TestUser> InitializeUsers()
        {            
            var address = new
            {
                street_address = "One Hacker Way",
                locality = "Heidelberg",
                postal_code = 69118,
                country = "Germany"
            };
                
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "818727",
                    Username = "alice",
                    Password = "alice",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Alice Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Alice"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
                    }
                },
                new TestUser
                {
                    SubjectId = "88421113",
                    Username = "bob",
                    Password = "bob",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Bob Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Bob"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                        new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
                    }
                }
            };
        }
    }
}