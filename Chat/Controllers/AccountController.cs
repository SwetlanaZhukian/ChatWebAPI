using AutoMapper;
using Chat.BLL.Infrastructure;
using Chat.BLL.Interfaces;
using Chat.BLL.Services;
using Chat.Hubs;
using Chat.Models.Entities;
using Chat.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Chat.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private readonly IOptions<EmailConfig> _options;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly ApplicationSettings _appsettings;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IOptions<EmailConfig> options, IEmailSender emailSender, IMapper mapper, IOptions<ApplicationSettings> appsettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _options = options;
            _emailSender = emailSender;
            _mapper = mapper;
            _appsettings = appsettings.Value;
         
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            try
            {
                User user = new User
                { Name = model.Name,
                    Email = model.Email,
                    UserName = model.PhoneNumber,
                    PhoneNumber = model.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // генерация токена для пользователя
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "confirmEmail",
                        "Account",
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    EmailSender emailService = new EmailSender();
                    await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>", _options);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        [Route("email/confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest(new { message = "userId или code отсутствуют в теле запроса" });
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest();
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return this.Redirect(_appsettings.Client_URL+"/user/login");
            else
                return BadRequest();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(model.Email);
            }

            if (user != null && user.EmailConfirmed == false)
            {
                return StatusCode(401, new { message = "Вы не подтвердили Email" });
            }
       else  if (user != null &&  await _userManager.CheckPasswordAsync(user,model.Password))
            {
                
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("Id" +
                        "",user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appsettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);

                return Ok(new { token });
            }

            
            else
                return BadRequest(new { message = "Логин или пароль не верны." });
        }

        //    [Route("signInWithGoogle")]
        //    public IActionResult SignInWithGoogle()
        //    {
        //        var authenticationProperties = _signInManager.ConfigureExternalAuthenticationProperties("Google", Url.Action(nameof(HandleExternalLogin)));
        //        return Challenge(authenticationProperties, "Google");
        //    }
        //    public async Task<IActionResult> HandleExternalLogin()
        //    {
        //        var info = await _signInManager.GetExternalLoginInfoAsync();

        //        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

        //        if (!result.Succeeded) //user does not exist yet
        //        {
        //            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        //           // var phone = info.Principal.FindFirstValue(ClaimTypes.MobilePhone);
        //            var newUser = new User
        //            {
        //                UserName = email,
        //                Email = email,
        //                EmailConfirmed = true
        //            };
        //            var createResult = await _userManager.CreateAsync(newUser);
        //            if (!createResult.Succeeded)
        //                throw new Exception(createResult.Errors.Select(e => e.Description).Aggregate((errors, error) => $"{errors}, {error}"));

        //            await _userManager.AddLoginAsync(newUser, info);
        //            var newUserClaims = info.Principal.Claims.Append(new Claim("userId", newUser.Id));
        //            await _userManager.AddClaimsAsync(newUser, newUserClaims);
        //            await _signInManager.SignInAsync(newUser, isPersistent: false);
        //            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        //        }

        //        return Redirect("_appsettings.Client_URL+"/home");
        //    }
        //public async Task<IActionResult> LogOff()
        //{
        //    string Id = User.Claims.First(c => c.Type == "Id").Value;

        //    await chat.OnDisconnected(Id);
        //}
    }
}
