using Microsoft.AspNetCore.Mvc;
using MyPass.Core.Entities;
using MyPass.Core.Patterns.Proxy;
using MyPass.Core.Services.Vault;
using MyPass.Core.Session;
using MyPass.Models.ViewModels.Vault;
using MyPass.Utilities;

namespace MyPass.Controllers
{
    public class VaultController : Controller
    {
        private readonly IVaultService _vaultService;
        private readonly WebSessionManager _session;

        public VaultController(IVaultService vaultService, WebSessionManager session)
        {
            _vaultService = vaultService;
            _session = session;
        }

        // ----------------- HELPER -----------------
        /// Ensures user is authenticated. Returns redirect if not.
        private IActionResult? EnsureAuthenticated(out int userId)
        {
            userId = 0;

            if (!_session.IsAuthenticated || _session.CurrentUserId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            userId = _session.CurrentUserId.Value;
            return null;
        }


        // ------ INDEX (LIST VAULT ITEMS) -------------
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var auth = EnsureAuthenticated(out var userId);
            if (auth != null) return auth;

            var items = await _vaultService.GetVaultItemsForUserAsync(userId);

            var vm = items.Select(item =>
            {
                var proxy = new SensitiveFieldProxy("••••••••");

                return new VaultItemViewModel
                {
                    Id = item.Id,
                    ItemType = item.GetType().Name,
                    DisplayName = item.Name,
                    MaskedPreview = proxy.GetMasked()
                };
            }).ToList();

            return View(vm);
        }


        // -------- CREATE LOGIN -----------------
        [HttpGet]
        public IActionResult CreateLogin()
        {
            var auth = EnsureAuthenticated(out _);
            return auth ?? View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLogin(LoginItemViewModel vm)
        {
            var auth = EnsureAuthenticated(out var userId);
            if (auth != null) return auth;

            if (!ModelState.IsValid)
                return View(vm);

            await _vaultService.CreateLoginAsync(
                userId,
                vm.SiteName,
                vm.Username,
                vm.Password,
                vm.Url
            );

            return RedirectToAction("Index");
        }


        // ----------------- CREATE CREDIT CARD -----------------
        [HttpGet]
        public IActionResult CreateCreditCard()
        {
            var auth = EnsureAuthenticated(out _);
            return auth ?? View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCreditCard(CreditCardViewModel vm)
        {
            var auth = EnsureAuthenticated(out var userId);
            if (auth != null) return auth;

            if (!ModelState.IsValid)
                return View(vm);

            await _vaultService.CreateCreditCardAsync(
                userId,
                vm.CardHolder,
                vm.CardHolder,
                vm.CardNumber,
                vm.CVV,
                vm.ExpMonth,
                vm.ExpYear
            );

            return RedirectToAction("Index");
        }


        // ------------ CREATE IDENTITY -----------------
        [HttpGet]
        public IActionResult CreateIdentity()
        {
            var auth = EnsureAuthenticated(out _);
            return auth ?? View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateIdentity(IdentityItemViewModel vm)
        {
            var auth = EnsureAuthenticated(out var userId);
            if (auth != null) return auth;

            if (!ModelState.IsValid)
                return View(vm);

            await _vaultService.CreateIdentityAsync(
                userId,
                vm.FullName,
                vm.FullName,
                vm.PassportNumber,
                vm.LicenseNumber,
                vm.SSN
            );

            return RedirectToAction("Index");
        }


        // ------------- CREATE SECURE NOTE -----------------
        [HttpGet]
        public IActionResult CreateSecureNote()
        {
            var auth = EnsureAuthenticated(out _);
            return auth ?? View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSecureNote(SecureNoteViewModel vm)
        {
            var auth = EnsureAuthenticated(out var userId);
            if (auth != null) return auth;

            if (!ModelState.IsValid)
                return View(vm);

            await _vaultService.CreateSecureNoteAsync(
                userId,
                vm.NoteTitle,
                vm.NoteContent
            );

            return RedirectToAction("Index");
        }


        // ----------- EDIT ITEMS -----------------
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var auth = EnsureAuthenticated(out var userId);
            if (auth != null) return auth;

            var item = await _vaultService.GetItemByIdAsync(id, userId);
            if (item == null) return NotFound();

            return item switch
            {
                LoginItem login => View("EditLogin", new LoginItemViewModel
                {
                    Id = login.Id,
                    SiteName = login.Name,
                    Username = login.Username,
                    Password = "",
                    Url = login.Url
                }),

                CreditCardItem cc => View("EditCreditCard", new CreditCardViewModel
                {
                    Id = cc.Id,
                    CardHolder = cc.CardHolderName,
                    CardNumber = "",
                    CVV = "",
                    ExpMonth = cc.ExpMonth,
                    ExpYear = cc.ExpYear
                }),

                IdentityItem ident => View("EditIdentity", new IdentityItemViewModel
                {
                    Id = ident.Id,
                    FullName = ident.FullName,
                    PassportNumber = "",
                    LicenseNumber = "",
                    SSN = ""
                }),

                SecureNoteItem note => View("EditSecureNote", new SecureNoteViewModel
                {
                    Id = note.Id,
                    NoteTitle = note.Name,
                    NoteContent = note.NoteContentDecrypted ?? ""
                }),

                _ => NotFound()
            };
        }

        // ------------- SAVE EDITS -----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(LoginItemViewModel vm)
        {
            var authResult = EnsureAuthenticated(out var userId);
            if (authResult != null) return authResult;

            await _vaultService.UpdateLoginAsync(
                vm.Id,
                userId,
                vm.SiteName,
                vm.Username,
                vm.Password,
                vm.Url
            );

            return RedirectToAction("Index");
        }

        // ------------- SAVE EDIT CREDIT CARD -----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEditCreditCard(CreditCardViewModel vm)
        {
            var auth = EnsureAuthenticated(out var userId);
            if (auth != null) return auth;

            await _vaultService.UpdateCreditCardAsync(
                vm.Id, userId,
                vm.CardHolder,
                vm.CardNumber,
                vm.CVV,
                vm.ExpMonth,
                vm.ExpYear
            );

            return RedirectToAction("Index");
        }

        // ------------- SAVE EDIT IDENTITY -----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEditIdentity(IdentityItemViewModel vm)
        {
            var auth = EnsureAuthenticated(out var userId);
            if (auth != null) return auth;

            await _vaultService.UpdateIdentityAsync(
                vm.Id, userId,
                vm.FullName,
                vm.PassportNumber,
                vm.LicenseNumber,
                vm.SSN
            );

            return RedirectToAction("Index");
        }

        // ------------- SAVE EDIT SECURE NOTE -----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEditSecureNote(SecureNoteViewModel vm)
        {
            var auth = EnsureAuthenticated(out var userId);
            if (auth != null) return auth;

            await _vaultService.UpdateSecureNoteAsync(
                vm.Id, userId,
                vm.NoteTitle,
                vm.NoteContent
            );

            return RedirectToAction("Index");
        }


        // ----------------- DELETE ---------------
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var auth = EnsureAuthenticated(out var userId);
            if (auth != null) return auth;

            var item = await _vaultService.GetItemByIdAsync(id, userId);
            if (item == null) return NotFound();

            ViewBag.ItemName = item.Name;
            ViewBag.Id = id;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var auth = EnsureAuthenticated(out var userId);
            if (auth != null) return auth;

            await _vaultService.DeleteItemAsync(id, userId);

            return RedirectToAction("Index");
        }

        // --------------- VIEW ITEM DETAILS -----------------
        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var authResult = EnsureAuthenticated(out var userId);
            if (authResult != null) return authResult;

            var item = await _vaultService.GetItemByIdAsync(id, userId);
            if (item == null)
                return NotFound();

            return item switch
            {
                LoginItem login => View("ViewLogin", new LoginItemViewModel
                {
                    Id = login.Id,
                    SiteName = login.Name,
                    Username = login.Username,
                    Password = EncryptionHelper.Decrypt(login.PasswordEncrypted),
                    Url = login.Url
                }),

                CreditCardItem card => View("ViewCreditCard", new CreditCardViewModel
                {
                    Id = card.Id,
                    CardHolder = card.CardHolderName,
                    CardNumber = EncryptionHelper.Decrypt(card.CardNumberEncrypted),
                    CVV = EncryptionHelper.Decrypt(card.CvvEncrypted),
                    ExpMonth = card.ExpMonth,
                    ExpYear = card.ExpYear
                }),

                IdentityItem ident => View("ViewIdentity", new IdentityItemViewModel
                {
                    Id = ident.Id,
                    FullName = ident.FullName,
                    PassportNumber = EncryptionHelper.Decrypt(ident.PassportNumberEncrypted),
                    LicenseNumber = EncryptionHelper.Decrypt(ident.LicenseNumberEncrypted),
                    SSN = EncryptionHelper.Decrypt(ident.SocialSecurityNumberEncrypted)
                }),

                SecureNoteItem note => View("ViewSecureNote", new SecureNoteViewModel
                {
                    Id = note.Id,
                    NoteTitle = note.Name,
                    NoteContent = EncryptionHelper.Decrypt(note.NoteEncrypted)
                }),

                _ => NotFound()
            };
        }

        //------------ COPY-TO-CLIPBOARD API-----------------
        [HttpPost]
        public async Task<IActionResult> CopyField(int id, string field)
        {
            var authResult = EnsureAuthenticated(out var userId);
            if (authResult != null) return Unauthorized();

            var item = await _vaultService.GetItemByIdAsync(id, userId);
            if (item == null) return NotFound();

            string value = field.ToLower() switch
            {
                "username" => (item as LoginItem)?.Username ?? "",
                "password" => item is LoginItem login
                    ? EncryptionHelper.Decrypt(login.PasswordEncrypted)
                    : "",
                "url" => (item as LoginItem)?.Url ?? "",

                "cardnumber" => item is CreditCardItem cc
                    ? EncryptionHelper.Decrypt(cc.CardNumberEncrypted)
                    : "",
                "cvv" => item is CreditCardItem cc2
                    ? EncryptionHelper.Decrypt(cc2.CvvEncrypted)
                    : "",

                _ => ""
            };

            if (string.IsNullOrWhiteSpace(value))
                return BadRequest("Invalid field.");

            return Json(new { value });
        }


        // ----------- LOGOUT -----------------
        [HttpGet]
        public IActionResult Logout()
        {
            _session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}

