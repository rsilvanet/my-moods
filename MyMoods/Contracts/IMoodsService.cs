using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System.Collections.Generic;

namespace MyMoods.Contracts
{
    public interface IMoodsService
    {
        IList<MoodDTO> Get();
        string GetImage(MoodType mood);
        string GetTagsHelpText(MoodType mood);
        MoodType GetFromPoints(double points);
        double Evaluate(MoodType mood);
    }
}
