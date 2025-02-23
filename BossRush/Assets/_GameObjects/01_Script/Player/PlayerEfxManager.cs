using UnityEngine;

public class PlayerEfxManager : MonoBehaviour
{
    [Header("Dash")]
    [SerializeField] private ParticleSystem dashEfx;

    private Player player;
    
    #region SetUp

    internal void SetUp(Player player)
    {
        this.player = player;
    }

    #endregion
    
    #region Dash

    internal void PlayDashEfx()
    {
        dashEfx.Play();
    }

    internal void StopDashEfx()
    {
        dashEfx.Stop();
    }
    
    #endregion
}
