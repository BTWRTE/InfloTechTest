using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Logging;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly ILogService _logService;
    public UsersController(IUserService userService, ILogService logService)
    {
        _userService = userService;
        _logService = logService;
    }

    [HttpGet]
    [Route("List")]
    public async Task<ViewResult> List(bool? isActive)
    {
        var items = (await _userService
            .GetAllAsync(isActive))
            .Select(u => new UserListItemViewModel
            {
                Id = u.Id,
                Forename = u.Forename,
                Surname = u.Surname,
                Email = u.Email,
                IsActive = u.IsActive,
                DateOfBirth = u.DateOfBirth
            });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View("List", model);
    }

    [HttpGet]
    [Route("ViewUser/{id}")]
    public async Task<ViewResult> ViewUser(long id)
    {
        var user = await _userService
            .Get(id);

        if (user != null)
        {
            return View("UserView", new EditUserViewModel
            {
                User = new UserListItemViewModel
                {
                    Id = user.Id,
                    Forename = user.Forename,
                    Surname = user.Surname,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    DateOfBirth = user.DateOfBirth
                },
                Logs = new LogListViewModel
                {
                    Items = (await _logService
                        .GetForEntity(user))
                        .Select(l => new LogViewModel
                        {
                            Id = l.Id,
                            EntityId = l.EntityId,
                            EntityName = l.EntityName,
                            Type = l.Type,
                            TypeName = l.Type.ToString(),
                            Date = l.Date,
                            PropertyName = l.PropertyName,
                            OriginalValue = l.OriginalValue,
                            CurrentValue = l.CurrentValue
                        })
                },
                IsEditable = false
            });
        }
        else
        {
            return View("Error");
        }
    }

    [HttpGet]
    [Route("NewUser")]
    [Route("EditUser/{id}")]
    public async Task<ViewResult> EditUser(long? id)
    {
        var model = new UserListItemViewModel();

        if (id != null)
        {
            var user = await _userService
                .Get(id.Value);

            if (user != null)
            {
                model.Id = user.Id;
                model.Forename = user.Forename;
                model.Surname = user.Surname;
                model.Email = user.Email;
                model.IsActive = user.IsActive;
                model.DateOfBirth = user.DateOfBirth;
            }
            else
            {
                return View("Error");
            }
        }

        return View("UserView", new EditUserViewModel
        {
            User = model,
            IsEditable = true
        });
    }

    [HttpPost]
    [Route("SetUser")]
    public async Task<ViewResult> SetUser(EditUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.User.Id == null)
            {
                var user = await _userService
                    .Create(ApplyModelToUser(model.User, new User()));

                model.User.Id = user.Id;
            }
            else
            {
                var user = await _userService
                    .Get(model.User.Id.Value);

                if (user != null)
                {
                    await _userService
                        .Update(ApplyModelToUser(model.User, user));
                }
                else
                {
                    return View("Error");
                }
            }
        }

        return View("UserView", new EditUserViewModel
        {
            User = model.User,
            IsEditable = true
        });
    }

    [HttpPost]
    [Route("DeleteUser/{id}")]
    public async Task<ViewResult> Delete(long id)
    {
        var user = await _userService
            .Get(id);

        if (user != null)
        {
            await _userService
                .Delete(user);
        }
        else
        {
            return View("Error");
        }

        return await List(null);
    }

    private static User ApplyModelToUser(UserListItemViewModel model, User user)
    {
        user.Forename = model.Forename ?? user.Forename;
        user.Surname = model.Surname ?? user.Surname;
        user.Email = model.Email ?? user.Email;
        user.IsActive = model.IsActive;
        user.DateOfBirth = model.DateOfBirth ?? user.DateOfBirth;

        return user;
    }
}
