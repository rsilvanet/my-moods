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
            return Enum.GetValues(typeof(MoodType))
                .Cast<MoodType>()
                .Select(x => new MoodDTO(x, GetImage(x), GetTagsHelpText(x)))
                .ToList();
        }

        public string GetImage(MoodType mood)
        {
            return $"/assets/emojis/{mood.ToString()}.png";
        }

        public string GetTagsHelpText(MoodType mood)
        {
            switch (mood)
            {
                case MoodType.angry:
                    return "Nossa! Conta pra gente o que tá te irritando.";
                case MoodType.unsatisfied:
                    return "Nada legal. Nos diga com o que você está insatisfeito.";
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

        public MoodType GetMoodByPoints(double points)
        {
            var rounded = Math.Round(points);

            if (rounded < 1)
            {
                return MoodType.angry;
            }
            else if (rounded > 5)
            {
                return MoodType.loving;
            }

            return (MoodType)rounded;
        }
    }
}
