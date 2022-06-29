using AlivelyMVC.Models;
using AlivelyMVC.Services;
using AlivelyMVC.ViewModels;
using Ardalis.GuardClauses;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AlivelyMVC.Controllers
{
    public class UserController : Controller
    {
        public readonly IMapper _mapper; 

        public UserService _userService;

        public UserController(IMapper mapper)
        {
            _mapper = mapper;

            _userService = new UserService();
        }

        public async Task<IActionResult> Index()
        {
            var currentUserUuid = Guid.Parse(HttpContext.Session.GetString("CurrentUserUuid"));

            if(currentUserUuid == Guid.Empty)
            {
                return BadRequest();
            }

            var httpResponseMessage = await _userService.GetUser(currentUserUuid).ConfigureAwait(false);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                TempData["Error"] = "There was an error when attempting to retrieve your account. If the error persists, contact support.";

                return View();
            }

            var user = await httpResponseMessage.Content.ReadFromJsonAsync<User>().ConfigureAwait(false);

            return View(_mapper.Map<UserViewModel>(user));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserViewModel userViewModel)
        {
            Guard.Against.Null(userViewModel);

            var userUuid = Guid.Parse(HttpContext.Session.GetString("CurrentUserUuid"));

            var httpResponseMessage = await _userService.GetUser(userUuid).ConfigureAwait(false);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                TempData["Error"] = "Profile update was unsuccessful. If the error persist, please contact support.";

                return View(new UserViewModel());
            }

            var userToUpdate = await httpResponseMessage.Content.ReadFromJsonAsync<User>().ConfigureAwait(false);

            Guard.Against.Null(userToUpdate);

            userToUpdate.FirstName = userViewModel.FirstName;

            userToUpdate.LastName = userViewModel.LastName;

            userToUpdate.Age = userViewModel.Age;

            httpResponseMessage = await _userService.UpdateUser(userToUpdate).ConfigureAwait(false);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                TempData["Error"] = "Profile update was unsuccessful. If the error persist, please contact support.";

                return RedirectToAction("Index");
            }

            TempData["Success"] = "Profile was updated successfully!";

            var user = await httpResponseMessage.Content.ReadFromJsonAsync<User>().ConfigureAwait(false);

            return RedirectToAction( "Index", _mapper.Map<UserViewModel>(user));
        }

        public async Task<IActionResult> UpdateUsernameAndEmail(UserViewModel userViewModel)
        {
            Guard.Against.Null(userViewModel);

            var userUuid = Guid.Parse(HttpContext.Session.GetString("CurrentUserUuid"));

            var httpResponseMessage = await _userService.GetUser(userUuid).ConfigureAwait(false);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                TempData["Error"] = "Profile update was unsuccessful. If the error persist, please contact support.";

                return View(new UserViewModel());
            }

            var userToUpdate = await httpResponseMessage.Content.ReadFromJsonAsync<User>().ConfigureAwait(false);

            Guard.Against.Null(userToUpdate);

            userToUpdate.UserName = userViewModel.UserName;

            userToUpdate.Email = userViewModel.Email;

            httpResponseMessage = await _userService.UpdateUser(userToUpdate).ConfigureAwait(false);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                TempData["Error"] = "Account update was unsuccessful. If the error persist, please contact support.";

                return RedirectToAction("Index");
            }

            TempData["Success"] = "Account was updated successfully!";

            var user = await httpResponseMessage.Content.ReadFromJsonAsync<User>().ConfigureAwait(false);

            return RedirectToAction("Index", _mapper.Map<UserViewModel>(user));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(Guid userUuid)
        {
            if(userUuid == Guid.Empty)
            {
                return BadRequest();
            }

            var httpResponseMessage = await _userService.DeleteUser(userUuid).ConfigureAwait(false);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                TempData["Error"] = "Something went wrong when attempting to delete your account. If the error persist, please contact support.";

                return RedirectToAction("Index");
            }

            TempData["Success"] = "Account was deleted successfully!";

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword( string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return BadRequest();
            }

            var userUuid = Guid.Parse(HttpContext.Session.GetString("CurrentUserUuid"));

            var httpResponseMessage = await _userService.ChangePassword(userUuid, password).ConfigureAwait(false);

            if(!httpResponseMessage.IsSuccessStatusCode)
            {
                TempData["Error"] = "Password change was unsuccessful. If the error persist, please contact support.";

                return RedirectToAction("Index");
            }

            TempData["Success"] = "Password was changed successfully!";

            return RedirectToAction("Index");
        }
    }
}
