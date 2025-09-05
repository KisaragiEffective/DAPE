using KisaragiTech.Dape.Base.Identity;
using KisaragiTech.Dape.User.Model;

namespace KisaragiTech.Dape.User.Interface;

public interface IUser : IIdentifiable<UserID>
{
    string GetPreferredHandle();
}
