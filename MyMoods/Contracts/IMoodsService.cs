using MyMoods.Domain.DTO;
using System.Collections.Generic;

namespace MyMoods.Contracts
{
    public interface IMoodsService
    {
        IList<MoodDTO> GetMoods();
    }
}
