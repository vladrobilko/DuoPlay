namespace DuoPlay.Application.Services.Interfaces
{
    public interface IPlayerService
    {
        Task CreatePlayer(string name);

        Task<bool> IsPlayerExist(string name);
    }
}
