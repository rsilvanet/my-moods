using MyMoods.Shared.Contracts;
using MyMoods.Shared.Domain.DTO;
using MyMoods.Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyMoods.Services
{
    public class MoodsService : IMoodsService
    {
        public IList<MoodDTO> Get()
        {
            return Enum.GetValues(typeof(MoodType))
                .OfType<MoodType>()
                .Select(x => new MoodDTO(x, Evaluate(x), GetImage(x), GetTagsHelpText(x)))
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

        public MoodType GetFromPoints(double points)
        {
            var rounded = Math.Round(points);

            if (points < 1.25)
                return MoodType.angry;
            if (points < 3.75)
                return MoodType.unsatisfied;
            if (points < 6.25)
                return MoodType.normal;
            if (points < 8.75)
                return MoodType.happy;

            return MoodType.loving;
        }

        public double Evaluate(MoodType mood)
        {
            switch (mood)
            {
                case MoodType.angry:
                    return 0;
                case MoodType.unsatisfied:
                    return 2.5;
                case MoodType.normal:
                    return 5;
                case MoodType.happy:
                    return 7.5;
                case MoodType.loving:
                    return 10;
                default:
                    throw new NotImplementedException("Método não preparado para obter o valor do mood informado.");
            }
        }
    }
}
