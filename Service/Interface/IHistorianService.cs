using System.Threading.Tasks;
using historianbigtableservice.Model;

namespace historianbigtableservice.Service.Interface
{
    public interface IHistorianService
    {
         Task<(bool,string)> addHistorian(Tag tag);
    }
}