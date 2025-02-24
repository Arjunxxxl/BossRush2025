using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    [Header("Hp Data")]
    private int HpLeft;
    private bool isShieldActivated;

    private Player player;
    
    #region SetUp

    internal void SetUp(Player player)
    {
        this.player = player;

        HpLeft = Constants.Player.MaxHp;
        isShieldActivated = false;
        
        GameplayMenu.Instance.PlayerHpUi.SetUp(HpLeft, Constants.Player.MaxHp);
    }

    #endregion

    #region Dmg

    internal void TakeDmg()
    {
        if (isShieldActivated)
        {
            return;
        }

        HpLeft--;
        
        if (HpLeft <= 0)
        {
            // TODO: Implement player death
        }
        
        GameplayMenu.Instance.PlayerHpUi.UpdateHpUi(HpLeft);
    }

    #endregion

    #region Hp

    internal void AddHp(int val)
    {
        HpLeft += val;

        if (HpLeft >= Constants.Player.MaxHp)
        {
            HpLeft = Constants.Player.MaxHp;
        }
        
        GameplayMenu.Instance.PlayerHpUi.UpdateHpUi(HpLeft);
    }

    #endregion

    #region Shield

    internal void SetShieldActivated(bool isActivated)
    {
        isShieldActivated = isActivated;
    }

    #endregion
}
