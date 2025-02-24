using System;
using UnityEngine;

public class GameplayMenu : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private WeaponUi weaponUi;
    [SerializeField] private PlayerHpUi playerHpUi;

    #region Properties

    public WeaponUi WeaponUi => weaponUi;
    public PlayerHpUi PlayerHpUi => playerHpUi;

    #endregion

    #region Singleton

    public static GameplayMenu Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    #endregion
}
