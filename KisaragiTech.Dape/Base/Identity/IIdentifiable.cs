namespace KisaragiTech.Dape.Base.Identity;

public interface IIdentifiable<out TIdentifier>
{
    TIdentifier GetIdentifier();
}
