using HARRepo.FileManager.Logic.DTOs;
using HARRepo.FileManager.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace HARRepo.FileManager.API.Logic
{
    public class UserResolver : IUserResolver
    {
        private readonly HttpContext _context;
        private readonly IConfiguration _config;

        public UserResolver(IHttpContextAccessor context, IConfiguration configuration)
        {
            _context = context?.HttpContext;
            _config = configuration;
        }

        public async Task<UserDTO> GetUserAsync()
        {
            var token = GetToken();
            if (token != null)
            {
                var domain = _config["Auth0Domain"];
                var profileUrl = $"{domain}userinfo";
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(profileUrl);
                if (response.IsSuccessStatusCode)
                {
                    var responseText = await response.Content.ReadAsStringAsync();
                    var userProps = JsonSerializer.Deserialize<Dictionary<string, object>>(responseText);
                    return new UserDTO()
                    {
                        Name = userProps["name"].ToString(),
                        Email = userProps["email"].ToString()
                    };
                }
            }

            return null;
        }

        public string GetUserId()
        {
            if (_context != null && _context.Request != null && _context.Request.Headers.ContainsKey("UserId"))
            {
                return _context.Request.Headers["UserId"].ToString();
            }
            return null;
        }

        private string GetToken()
        {
            var authHeader = _context.Request?.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authHeader))
            {
                var splitHeader = authHeader.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (splitHeader.Length > 1 && splitHeader[0] == "Bearer")
                {
                    return splitHeader[1];
                }
            }

            return null;
        }
    }
}
