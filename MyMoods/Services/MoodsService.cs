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
                moods.Add(new MoodDTO(item));
            }

            return moods;
        }

        public string GetTitle(MoodType mood)
        {
            switch (mood)
            {
                case MoodType.sad:
                    return "Nossa! Deixe-nos saber o que lhe deixa triste.";
                case MoodType.unsatisfied:
                    return "Isso não é legal. Nos diga com o que você está insatisfeito.";
                case MoodType.normal:
                    return "Talvez dê pra melhorar. Só nos ajude a identificar onde.";
                case MoodType.happy:
                    return "Que ótimo! Diz aí pra gente o motivo da sua felicidade.";
                case MoodType.inLove:
                    return "Perfeito. Aponta pra gente o que te deixou tão apaixonado.";
                default:
                    return string.Empty;
            }
        }
    }
}
