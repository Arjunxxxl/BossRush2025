using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/AbilityData", fileName = "Ability Data")]
public class AbilityData : ScriptableObject
{
    [System.Serializable]
    public class Data
    {
        [Header("General Data")]
        public AbilityType abilityType;
        public string abilityName;
        public Sprite abilityUiIcon;

        [Header("Duration Data")]
        public float abilityActiveDuration;
        public float abilityCooldownDuration;
        
        [Header("Ability Intensity")]
        public float abilityEffectiveness;
    }

    [SerializeField] private List<Data> abilityDatas;

    internal Data CalcAbilityData(AbilityType abilityType)
    {
        for (int i = 0; i < abilityDatas.Count; i++)
        {
            if (abilityDatas[i].abilityType == abilityType)
            {
                return abilityDatas[i];
            }
        }

        return null;
    }
}
