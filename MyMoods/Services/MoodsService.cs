using MyMoods.Contracts;
using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyMoods.Services
{
    public class MoodsService : IMoodsService
    {
        public IList<MoodDTO> GetMoods()
        {
            var moods = new List<MoodDTO>();

            foreach (var item in Enum.GetValues(typeof(MoodType)).Cast<MoodType>())
            {
                moods.Add(new MoodDTO(item, GetTagsHelpText(item)));
            }

            return moods;
        }

        public string GetTagsHelpText(MoodType mood)
        {
            switch (mood)
            {
                case MoodType.sad:
                    return "Nossa! Conta pra gente o que tá te deixando triste.";
                case MoodType.unsatisfied:
                    return "Isso não é legal. Nos diga com o que você está insatisfeito.";
                case MoodType.normal:
                    return "Talvez dê pra melhorar. Nos ajude a identificar onde ou como.";
                case MoodType.happy:
                    return "Que ótimo! Diz aí pra gente o motivo da sua felicidade.";
                case MoodType.loving:
                    return "Perfeito. Compartilha com a gente o que te deixou tão apaixonado.";
                default:
                    return string.Empty;
            }
        }
    }
}
