using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int AttackBuffNumber;
    private int healthBuffNumber;
    private int DashBuffNumber;

    [Header("Attack Buffs")] public bool burnDamage;
    [Range(1f, 3f)] public float attackRange = 1;
    public bool swordRangedAttack;

    [Header("Health Buffs")] [Range(1f, 10f)]
    public float maxHealthScaler = 1;

    public bool monsterKillRegeneration;
    public bool staticRegeneration;

    [Header("Dash Buffs")] public bool iceDash;
    public bool secondWind;
    public bool cantFall;


    public void ActivateUpgrade(int upgrade)
    {
        switch (upgrade)
        {
            case 1:
                UpgradeAttack();
                break;
            case 2:
                UpgradeHealth();
                break;
            case 3:
                UpgradeDash();
                break;
        }
    }

    private void UpgradeAttack()
    {
        switch (AttackBuffNumber)
        {
            case 0:
                attackRange *= 1.4f;
                break;
            case 1:
                burnDamage = true;
                break;
            case 2:
                swordRangedAttack = true;
                break;
        }

        AttackBuffNumber++;
    }

    private void UpgradeHealth()
    {
        switch (healthBuffNumber)
        {
            case 0:
                maxHealthScaler *= 1.5f;
                break;
            case 1:
                monsterKillRegeneration = true;
                break;
            case 2:
                staticRegeneration = true;
                break;
        }

        healthBuffNumber++;
    }

    private void UpgradeDash()
    {
        switch (DashBuffNumber)
        {
            case 0:
                iceDash = true;
                break;
            case 1:
                secondWind = true;
                break;
            case 2:
                cantFall = true;
                break;
        }

        DashBuffNumber++;
    }
}