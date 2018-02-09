using System.Threading.Tasks;
using historianbigtableservice.Model;

namespace historianbigtableservice.Service.Interface
{
    public interface IHistorianService
    {
        Task<(ThingOutput,string)> GetHistorian(int thingId, long startDate, long endDate);
        Task<(bool,string)> AddHistorian(TagInput tag);
    }
}