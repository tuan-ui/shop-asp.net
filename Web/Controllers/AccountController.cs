using BotDetect.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.App_Start;
using Web.Models;
using Service.Common;
using Web.Common;
using AutoMapper;
using Data;
using System.Configuration;
using System.Net;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public AccountController()
        {

        }
        public async Task<ActionResult> Profile()
        {
            var manager = HttpContext.GetOwinContext().Get<ShopDbContext>();
            var user = manager.Users.Where(a=> a.UserName == User.Identity.Name).Single();
            ApplicationUserViewModel item = new ApplicationUserViewModel();
            item.Email = user.Email;
            item.FullName = user.FullName;
            if (user.BirthDay != null)
                item.BirthDay = user.BirthDay ?? DateTime.Now;  
            item.PhoneNumber = user.PhoneNumber;
            item.UserName = user.UserName;
          
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Profile(ApplicationUserViewModel req)
        {
            if (req.BirthDay > DateTime.Now)
            {
                ModelState.AddModelError("BirthDay", "Ngày sinh không thể là một ngày trong tương lai.");
                return View(req);
            }
            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await manager.FindByEmailAsync(req.Email);
            user.Email = req.Email;
            user.FullName = req.FullName;
           
            user.PhoneNumber = req.PhoneNumber;
            user.BirthDay = req.BirthDay;
            var rs = await manager.UpdateAsync(user);

            if (rs.Succeeded)
            {
                TempData["SuccessMessage"] = "Thông tin đã được thay đổi thành công.";
                return RedirectToAction("Profile");
            }
            TempData["ErrorMessage"] = "Có lỗi xảy ra. Vui lòng thử lại.";
            return View(req);
        }
        // GET: Account
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = _userManager.Find(model.UserName, model.Password);
                Session.Add(CommonConstants.USER_SESSION, model.UserName);
                if (user != null)
                {
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    ClaimsIdentity identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationProperties props = new AuthenticationProperties();
                    props.IsPersistent = model.RememberMe;
                    authenticationManager.SignIn(props, identity);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            return View(model);
        }


        public ActionResult LoginGoogle()
        {
            string authorizationRequest = string.Format("{0}?response_type={1}&scope={2}&redirect_uri={3}&client_id={4}",
        authorizationEndpoint, responseType, scope, Uri.EscapeDataString(redirection_url), clientid);

            return Redirect(authorizationRequest);
        }
        public ActionResult ExternalLogin()
        {
            if (Request.QueryString["code"] != null)
            {
                GoogleCallback(Request.QueryString["code"].ToString());
            }

            if (Session[CommonConstants.USER_SESSION] == null) { 
                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction("Index", "Home");
        }

        public void GoogleCallback(string code)
        {
            string postString = $"grant_type=authorization_code&code={code}&client_id={clientid}&client_secret={clientsecret}&redirect_uri={redirection_url}";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            byte[] bytes = Encoding.UTF8.GetBytes(postString);

            try
            {
                using (var outputStream = request.GetRequestStream())
                {
                    outputStream.Write(bytes, 0, bytes.Length);
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string responseFromServer = streamReader.ReadToEnd();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Tokenclass tokenResponse = js.Deserialize<Tokenclass>(responseFromServer);
                    GetLoginAsync(tokenResponse.access_token).Wait();
                }
            }
            catch (WebException ex)
            {
                
            }
        }

        string scope = ConfigurationManager.AppSettings["scope"];
        string clientid = ConfigurationManager.AppSettings["GgAppId"];
        string responseType = ConfigurationManager.AppSettings["responseType"];
        string clientsecret = ConfigurationManager.AppSettings["GgAppSecret"];
        string redirection_url = ConfigurationManager.AppSettings["redirectUri"];
        string url = ConfigurationManager.AppSettings["url"];
        string authorizationEndpoint = ConfigurationManager.AppSettings["authorizationEndpoint"];
        public async Task<ActionResult> GetLoginAsync(string token)
        {
            string url3 = $"https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token={token}";
            WebRequest request2 = WebRequest.Create(url3);
            request2.Credentials = CredentialCache.DefaultCredentials;

            using (WebResponse response2 = request2.GetResponse())
            using (Stream dataStream = response2.GetResponseStream())
            using (StreamReader reader = new StreamReader(dataStream))
            {
                string responseFromServer2 = reader.ReadToEnd();
                JavaScriptSerializer js2 = new JavaScriptSerializer();
                ApplicationUser userInfo = js2.Deserialize<ApplicationUser>(responseFromServer2);
                var resultUser = _userManager.FindByEmail(userInfo.Email);
   
                if (resultUser != null)
                {
                    var userSession = new ApplicationUser
                    {
                        UserName = resultUser.UserName,
                        FullName = resultUser.FullName
                    };
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                }
                else
                {

                    var user = new ApplicationUser { UserName = userInfo.Email.Split('@')[0], Email = userInfo.Email,FullName = userInfo.Email.Split('@')[0] };
                    var result =  _userManager.Create(user);
                    resultUser = _userManager.FindByEmail(userInfo.Email);
                    Session.Add(CommonConstants.USER_SESSION, user);
                    

                }
                IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                ClaimsIdentity identity = _userManager.CreateIdentity(resultUser , DefaultAuthenticationTypes.ApplicationCookie);
                AuthenticationProperties props = new AuthenticationProperties();
                authenticationManager.SignIn(props, identity);
            }
            return RedirectToAction("Index", "Home");
        }   


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [CaptchaValidation("CaptchaCode", "registerCaptcha", "Mã xác nhận không đúng")]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userByEmail = await _userManager.FindByEmailAsync(model.Email);
                if (userByEmail != null)
                {
                    ModelState.AddModelError("email", "Email đã tồn tại");
                    return View(model);
                }
                var userByUserName = await _userManager.FindByNameAsync(model.UserName);
                if (userByUserName != null)
                {
                    ModelState.AddModelError("email", "Tài khoản đã tồn tại");
                    return View(model);
                }
                var user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = true,
                    BirthDay = DateTime.Now,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address

                };

                await _userManager.CreateAsync(user, model.Password);


                var adminUser = await _userManager.FindByEmailAsync(model.Email);
                if (adminUser == null)
                    await _userManager.AddToRolesAsync(adminUser.Id, new string[] { "User" });

                string content = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/template/newuser.html"));
                content = content.Replace("{{UserName}}", adminUser.FullName);
                content = content.Replace("{{Link}}", ConfigHelper.GetByKey("CurrentLink") + "dang-nhap.html");

                Common.MailHelper.SendMail(adminUser.Email, "Đăng ký thành công", content);


                ViewData["SuccessMsg"] = "Đăng ký thành công";
            }

            return View();
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Email))
            {
                var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = manager.FindByName(model.Email);
                if (user != null)
                {
                    string code = manager.GeneratePasswordResetToken(user.Id);
                    string email = user.Email;
                    string callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code, email = email }, protocol: Request.Url.Scheme);
                    MailHelper.SendMail(email, "TuanPC", "Bạn click vào <a href='" + callbackUrl + "'>link này</a> để reset mật khẩu");
                    return RedirectToAction("ForgotPasswordConfirmation", "Account", new { email = email });
                }
                else
                {
                    return RedirectToAction("ForgotPasswordError", "Account");
                }
            }

            return View(model);
        }
    
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation(string email)
        {
            ViewBag.email = email;
            return View();
        }
        [AllowAnonymous]
        public ActionResult ForgotPasswordError()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code,string email)
        {
            ViewBag.gmail = email;
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
             if (model.Email == null)
            {
                return RedirectToAction("ResetPasswordError", "Account");
            }
            var user = manager.FindByEmail(model.Email);
            if (!ModelState.IsValid)
            {
                return View(model);
            }  
            var result = await manager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult ResetPasswordError()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (String.Compare(model.NewPassword, model.ConfirmPassword, true) != 0)
                {
                    ModelState.AddModelError("NewPassword", "Mật khâu mới không khớp");
                    return View(model);
                }
                ApplicationUser user = _userManager.Find(model.UserName, model.OldPassword);
                if (user == null)
                {
                    ModelState.AddModelError("OldPassword", "Mật khâu cũ không đúng");
                    return View(model);
                }
                var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                string code = manager.GeneratePasswordResetToken(user.Id);
                var result = await manager.ResetPasswordAsync(user.Id, code, model.NewPassword);
                if (result.Succeeded == true){ 
                    return RedirectToAction("ChangePasswordConfirmation", "Account");
                } 
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult ChangePasswordConfirmation()
        {
            return View();
        }
    }
}