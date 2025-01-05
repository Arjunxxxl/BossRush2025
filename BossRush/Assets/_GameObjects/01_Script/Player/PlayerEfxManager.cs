using UnityEngine;

public class PlayerEfxManager : MonoBehaviour
{
    [Header("Dash")]
    [SerializeField] private ParticleSystem dashEfx;

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
