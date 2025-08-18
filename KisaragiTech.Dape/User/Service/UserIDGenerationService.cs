using System;
using KisaragiTech.Dape.User.Model;

namespace KisaragiTech.Dape.User.Service;

public static class UserIDGenerationService
{
    public static UserID Generate()
    {
        return new UserID(Guid.NewGuid());
    }
}
