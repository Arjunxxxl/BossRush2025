using TMPro;
using UnityEngine;

public class WeaponUi : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TMP_Text clipAmmoTxt;
    [SerializeField] private TMP_Text carryingAmmoTxt;

    [Header("Tween")]
    [SerializeField] private Tween bulletIconPulsateTween;

    internal void UpdateCarryingAmmoTxt(int val)
    {
        carryingAmmoTxt.text = val.ToString();
    }

    internal void UpdateClipAmmoTxt(int val)
    {
        clipAmmoTxt.text = val.ToString();
        
        bulletIconPulsateTween.PlayTween("PulsateUp");
    }
}
