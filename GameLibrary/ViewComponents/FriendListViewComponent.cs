using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameLibrary.ViewComponents;

// ViewComponent to mini-kontroler wielokrotnego użytku.
// Można go osadzić w dowolnym widoku za pomocą: <vc:friend-list ... />
public class FriendListViewComponent : ViewComponent
{
    // Metoda InvokeAsync jest wywoływana przy renderowaniu komponentu
    public IViewComponentResult Invoke(int playerId, ICollection<Friend> friends)
    {
        return View((playerId, friends));
    }
}
