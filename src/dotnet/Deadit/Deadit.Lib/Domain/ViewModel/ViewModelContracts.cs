namespace Deadit.Lib.Domain.ViewModel;

public class ViewModelContracts
{

    public interface ILoggedIn
    {
        public bool IsLoggedIn { get; set; }
    }

    public interface IAuthor
    {
        public bool IsAuthor { get; set; }
    }

    public interface IClientId
    {
        public uint? ClientId { get; set; }
    }

}
