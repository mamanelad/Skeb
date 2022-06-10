using UnityEngine;
using UnityEngine.InputSystem;

public class CheatManager : MonoBehaviour
{
    private GameControls _cheatControls;
    
    private void Awake()
    {
        _cheatControls = new GameControls();
        InitializeControls();
    }

    #region Input Actions

    private void InitializeControls()
    {
        _cheatControls.CheatControl.Action1.performed += FillLifePoints;
        _cheatControls.CheatControl.Action2.performed += KillAllMonsters;
        _cheatControls.CheatControl.Action3.performed += InfinityUpgrades;
    }


    private void OnEnable()
    {
        _cheatControls.CheatControl.Enable();
    }

    private void OnDisable()
    {
        _cheatControls.CheatControl.Disable();
    }

    #endregion

    private static void FillLifePoints(InputAction.CallbackContext context)
    {
        var playerHealth = FindObjectOfType<PlayerHealth>();
        playerHealth.UpdateHealth(Mathf.Infinity, Vector3.zero);
    }

    private void KillAllMonsters(InputAction.CallbackContext context)
    {
        var monstersList = FindObjectsOfType<Enemy>();
        foreach (var monster in monstersList)
        {
            monster.DamageEnemy(10000);
            
        }
    }

    private void InfinityUpgrades(InputAction.CallbackContext context)
    {
        FindObjectOfType<StoreManager>().GetUnlimitedUpgrades();
    }
}
