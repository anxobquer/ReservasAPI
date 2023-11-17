using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MI_API_REST.Entities;
using MI_API_REST.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MI_API_REST.Controllers
{
    /// <summary>
    /// Controlador para obtener token de seguridad.
    /// </summary>
    [Route("api/Token")]
    [ApiController]
    public class ApiUsuarios : ControllerBase
    {
        public IConfiguration _configuration;

        public ApiUsuarios(IConfiguration cofig)
        {
            _configuration = cofig;
        }
        
        /// <summary>
        /// Método para validar usuario y obtener token de seguridad.
        /// </summary>
        /// <param name="modUsuarios"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LoginApi(ModUsuariosApi modUsuarios)
        {
            RepUsuariosApi repUsuarios = new();
            bool usuario = repUsuarios.VerificarUsuario(modUsuarios.Usuario, modUsuarios.Pass);

            if (usuario)
            {
                Claim[] claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("usuario", modUsuarios.Usuario)
                };

                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                SigningCredentials signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                JwtSecurityToken token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(3),
                    signingCredentials: signIn
                );

                //ModTokenResponse response = new ModTokenResponse();
                //response.TokenResponse = Convert.ToString(new JwtSecurityTokenHandler().WriteToken(token));

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            else
            {
                return BadRequest("Usuario o contraseña es incorrecta.");
            }
        }

        //public int? ValidateToken(string token)
        //{
        //    if (token == null)
        //        return null;

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        //    try
        //    {
        //        tokenHandler.ValidateToken(token, new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
        //            // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
        //            ClockSkew = TimeSpan.Zero
        //        }, out SecurityToken validatedToken);

        //        var jwtToken = (JwtSecurityToken)validatedToken;
        //        var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

        //        // return user id from JWT token if validation successful
        //        return userId;
        //    }
        //    catch
        //    {
        //        // return null if validation fails
        //        return null;
        //    }
        //}
    }
}

