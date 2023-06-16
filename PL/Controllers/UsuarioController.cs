using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        private IHostingEnvironment environment;
        private IConfiguration configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UsuarioController(IWebHostEnvironment hostingEnvironment, IHostingEnvironment _environment, IConfiguration _configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            environment = _environment;
            configuration = _configuration;
        }

        public IActionResult Login()
        {
            ML.Usuario usuario = new ML.Usuario();
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Login(ML.Usuario usuario, string password)
        {
            var bcrypt = new Rfc2898DeriveBytes(password, new byte[0], 10000, HashAlgorithmName.SHA256);
            // Obtener el hash resultante para la contraseña ingresada 
            var passwordHash = bcrypt.GetBytes(20);

            if (usuario.Username != null)
            {
                // Insertar usuario en la base de datos
                usuario.Password = passwordHash;
                ML.Result result = BL.Usuario.Add(usuario);
                return View();
            }
            else
            {
                // Proceso de login
                ML.Result result = BL.Usuario.GetByEmail(usuario.Email);
                usuario = (ML.Usuario)result.Object;

                if (usuario.Password.SequenceEqual(passwordHash))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult CambiarPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CambiarPassword(string email)
            {
            ML.Result result = BL.Usuario.FindByEmail(email);
            if (result.Correct)
            {
                string emailOrigen = configuration["emailOrigen"];

                MailMessage mailMessage = new MailMessage(emailOrigen, email, "Recuperar Contraseña", "<p>Correo para recuperar contraseña</p>");
                mailMessage.IsBodyHtml = true;
                //string contenidoHTML = System.IO.File.ReadAllText(configuration["contenidoHTML"]);
                string contenidoHTML = System.IO.File.ReadAllText(Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "Templates", "Email.html"));
                mailMessage.Body = contenidoHTML;
                string url = configuration["url"] + HttpUtility.UrlEncode(email);
                mailMessage.Body = mailMessage.Body.Replace("{link}", url);
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, configuration["appPassword"]);

                smtpClient.Send(mailMessage);
                smtpClient.Dispose();

                ViewBag.Modal = "show";
                ViewBag.Message = "Se ha enviado un correo de confirmación a tu correo electronico";
                return View();
            }
            else
            {
                ViewBag.Modal = "show";
                ViewBag.Mensaje = "El correo es incorrecto";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Email()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NewPassword(string email)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Email = email;

            return View(usuario);
        }

        [HttpPost]
        public ActionResult NewPassword(ML.Usuario usuario, string password)
        {
            var bcrypt = new Rfc2898DeriveBytes(password, new byte[0], 10000, HashAlgorithmName.SHA256);

            var passwordHash = bcrypt.GetBytes(20);
            usuario.Password = passwordHash;

            ML.Result result = BL.Usuario.Update(usuario);

            if (result.Correct)
            {
                ViewBag.Modal = "show";
                ViewBag.Message = "Se ha actualizado correctamente";
                return RedirectToAction("Login", "Usuario");
            }
            else
            {
                ViewBag.Modal = "show";
                ViewBag.Mensaje = "Error al actualizar la contraseña";
                return View();
            }
        }
    }
}
