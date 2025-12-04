using Microsoft.AspNetCore.Mvc;
using MyPass.Core.Services.Account;
using MyPass.Core.Session;
using MyPass.Models.ViewModels.Account;
using MyPass.Core.Exceptions;
using MyPass.Core.Patterns.Builder;

namespace MyPass.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly WebSessionManager _session;
        private readonly PasswordGeneratorService _passwordGenerator;
        private readonly PasswordRecoveryService _passwordRecovery;
        public AccountController(
            IAccountService accountService,
            WebSessionManager session,
            PasswordGeneratorService passwordGenerator,
            PasswordRecoveryService passwordRecovery)
        {
            _accountService = accountService;
            _session = session;
            _passwordGenerator = passwordGenerator;
            _passwordRecovery = passwordRecovery;
        }

        // LOGIN (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // LOGIN (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            var user = await _accountService.LoginAsync(vm.Email, vm.MasterPassword);

            if (user == null)
            {
                ViewBag.Error = "Invalid email or password.";
                return View(vm);
            }

            _session.SetCurrentUser(user.Id);
            return RedirectToAction("Index", "Vault");
        }

        // REGISTER (GET)
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // REGISTER (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var questions = new List<(string question, string answer)>
                {
                    (model.Question1, model.Answer1),
                    (model.Question2, model.Answer2),
                    (model.Question3, model.Answer3)
                };

                await _accountService.RegisterAsync(model.Email, model.MasterPassword, questions);
                return RedirectToAction("Login");
            }
            catch (WeakPasswordException ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
            catch
            {
                ViewBag.Error = "Something went wrong. Please try again.";
                return View(model);
            }
        }

        // Strong password generator
        [HttpPost]
        public IActionResult GenerateStrongPassword()
        {
            var password = _passwordGenerator.GenerateStrongPassword();
            return Json(new { password });
        }

        [HttpGet]
        public IActionResult Logout()
        {
            _session.Clear();
            return RedirectToAction("Login", "Account");
        }

       
        // FORGOT PASSWORD (STEP 1)
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                ViewBag.Error = "Please enter your email.";
                return View(model);
            }

            var questions = await _passwordRecovery.GetQuestionsForEmailAsync(model.Email);
            if (questions == null)
            {
                ViewBag.Error = "We couldn't find an account with that email.";
                return View(model);
            }

            var vm = new VerifySecurityQuestionsViewModel
            {
                Email = model.Email,
                Question1 = questions[0],
                Question2 = questions[1],
                Question3 = questions[2]
            };

            return View("VerifySecurityQuestions", vm);
        }
       
        // FORGOT PASSWORD (STEP 2)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifySecurityQuestions(VerifySecurityQuestionsViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.Answer1) ||
                string.IsNullOrWhiteSpace(model.Answer2) ||
                string.IsNullOrWhiteSpace(model.Answer3) ||
                string.IsNullOrWhiteSpace(model.NewPassword))
            {
                ViewBag.Error = "Please answer all questions and choose a new password.";
                return View(model);
            }

            if (model.NewPassword != model.ConfirmNewPassword)
            {
                ViewBag.Error = "New password and confirmation do not match.";
                return View(model);
            }

            var success = await _passwordRecovery.TryRecoverAsync(
                model.Email,
                model.Answer1,
                model.Answer2,
                model.Answer3,
                model.NewPassword
            );

            if (!success)
            {
                ViewBag.Error = "The answers did not match our records.";
                return View(model);
            }
           

            TempData["Message"] = "Your master password was reset. Please log in with your new password.";
            return RedirectToAction("Login");
        }

    }
}
