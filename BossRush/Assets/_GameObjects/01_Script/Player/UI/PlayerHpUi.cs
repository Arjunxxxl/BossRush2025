using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUi : MonoBehaviour
{
    [Header("Ui Data")]
    [SerializeField] private TMP_Text hpTxt;
    [SerializeField] private Slider hpSlider;

    internal void SetUp(int curHp, int maxHp)
    {
        hpSlider.minValue = 0;
        hpSlider.maxValue = maxHp;
        hpSlider.value = curHp;

        hpTxt.text = curHp.ToString();
    }

    internal void UpdateHpUi(int curHp)
    {
        hpSlider.value = curHp;
        hpTxt.text = curHp.ToString();
    }
}
