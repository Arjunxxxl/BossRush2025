using System;
using UnityEngine;

public class GameplayMenu : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private WeaponUi weaponUi;

    #region Properties

    public WeaponUi WeaponUi => weaponUi;

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
