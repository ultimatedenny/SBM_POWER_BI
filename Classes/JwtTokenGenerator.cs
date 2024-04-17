using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SBM_POWER_BI.Classes
{
    public class JwtTokenGenerator
    {
        private const string Secret = "0nQJNTz7IPk/NZ3BuYnNHehDvobbNB2GrDzwIxx4QkM=";
        public string GenerateToken(string USERID, string DEPARTMENT, string GROUPS_PBI)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, USERID),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var token = new JwtSecurityToken(
                issuer: DEPARTMENT,
                audience: GROUPS_PBI,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), 
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public bool ValidateToken(string token, string DEPARTMENT, string GROUPS_PBI)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = DEPARTMENT,
                ValidateAudience = true,
                ValidAudience = GROUPS_PBI,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (SecurityTokenException)
            {
                return false;
            }
        }
        public bool IsAuth()
        {
            try
            {
                JwtTokenGenerator jwtTokenGenerator = new JwtTokenGenerator();
                var USERID = COOKIES.GetCookies("USERID") ?? "";
                var WINDOWSID = COOKIES.GetCookies("WINDOWSID") ?? "";
                var NAME = COOKIES.GetCookies("NAME") ?? "";
                var EMAIL = COOKIES.GetCookies("EMAIL") ?? "";
                var DEPARTMENT = COOKIES.GetCookies("DEPARTMENT") ?? "";
                var GROUPS_MDM = COOKIES.GetCookies("GROUPS_MDM") ?? "";
                var GROUPS_PBI = COOKIES.GetCookies("GROUPS_PBI") ?? "";
                var KEY = GenerateStrongKey(USERID, WINDOWSID, NAME, EMAIL, DEPARTMENT, GROUPS_MDM, GROUPS_PBI);
                GROUPS_PBI = (KEY + GROUPS_PBI + KEY);
                DEPARTMENT = (KEY + DEPARTMENT + KEY);
                var ISVALIDATE = jwtTokenGenerator.ValidateToken(COOKIES.GetCookies("TOKEN"), DEPARTMENT, GROUPS_PBI);
                return ISVALIDATE;
            }
            catch (Exception)
            {
                return false;
            }
        }
        static string GenerateStrongKey(params string[] values)
        {
            var concatenatedValues = string.Join("|", values);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(concatenatedValues));
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}



        
