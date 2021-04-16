using DataAccessLayer.Entities;
using System.Threading.Tasks;

namespace Utility.Service
{
    public interface ITokenService
    {
        string GenerateNewToken(Entry entry);
    }
}
