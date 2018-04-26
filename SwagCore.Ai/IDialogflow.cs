using System.Threading.Tasks;
using SwagCore.Ai.Models;

namespace SwagCore.Ai
{
    public interface IDialogflow
    {
        void Connect(string clientAccessToken);
        Task<AiResponseModel> SendMessage(string message);
    }
}