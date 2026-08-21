using PFCRepository.DatabaseContext.Models.CustomTables;
using PFCRepository.Repositories.PriceList.Models.IntermediateModels;

namespace PFCRepository.Repositories.PriceList.Models
{
    public class PriceFileCalucationRequest
    {
        public UserConfigSetting ObjUserConfigSetting { get; set; }
    }
}
