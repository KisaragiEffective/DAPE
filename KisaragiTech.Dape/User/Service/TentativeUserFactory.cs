using KisaragiTech.Dape.User.Model;

namespace KisaragiTech.Dape.User.Service;

public static class TentativeUserFactory
{
    public static LocalTentativeUser Create(string initialPreferredUsername)
    {
        return new LocalTentativeUser(UserIDGenerationService.Generate(), initialPreferredUsername);
    }
}
