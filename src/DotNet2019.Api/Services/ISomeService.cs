using System.Threading.Tasks;

namespace DotNet2019.Api.Services
{
    public interface ISomeService
    {
        Task<string> Ping();
    }
}
