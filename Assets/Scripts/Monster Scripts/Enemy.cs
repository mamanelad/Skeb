using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Private Fields

    private GameObject _player;
    private EnemySpawnerDots _enemySpawnerDots;
    private Animator _fireMonsterAnimator;
    private Animator _iceMonsterAnimator;
    private GameManager.WorldState _state;

    #endregion

    #region Inspector Control

    [Header("Monsters alone Settings")]
    [SerializeField] private GameObject fireMonster;
    [SerializeField] private GameObject iceMonster;
    
    [Header("Health Settings")]
    [SerializeField] private float currHealth;
    [SerializeField] private float startHealth = 100;

    #endregion

    #region Animator Labels

    private static readonly int Damage = Animator.StringToHash("Demage");

    #endregion

    private void Start()
    {
        _fireMonsterAnimator = fireMonster.GetComponent<Animator>();
        _iceMonsterAnimator = iceMonster.GetComponent<Animator>();
        currHealth = startHealth;
        _player = GameObject.FindGameObjectWithTag("Player");
        _enemySpawnerDots = FindObjectOfType<EnemySpawnerDots>();
        ChangeState();
    }


    private void Update()
    {
        if (_state != GameManager.Shared.CurrentState)
            ChangeState();

        SideHandler();
    }


    /**
     * Make the enemy direction towards the player.
     */
    private void SideHandler()
    {
        if (_player.transform.position.x < transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);

        else if (_player.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
    }


    /**
     * A getter function for the health amount.
     */
    public float GetHealth()
    {
        return currHealth;
    }

    /**
     * Decreasing the enemy health and trigger the right damage animation.
     */
    public void DamageEnemy(int damage)
    {
        currHealth -= damage;
        switch (_state)
        {
            case GameManager.WorldState.Fire:
                _fireMonsterAnimator.SetTrigger(Damage);
                break;

            case GameManager.WorldState.Ice:
                _iceMonsterAnimator.SetTrigger(Damage);
                break;
        }

        if (currHealth <= 0)
            KillEnemy();
    }


    /**
     * Decreasing the amount of enemies in the enemies counter spawner and lock the enemy movement.
     */
    private void KillEnemy()
    {
        GetComponent<EnemyAI>().enabled = false;
        _enemySpawnerDots.DecreaseMonster();
    }

    /**
     * This function on charge of changing the enemy state,
     * that means switch between which monster is shown.
     */
    private void ChangeState()
    {
        _state = GameManager.Shared.CurrentState;
        switch (_state)
        {
            case GameManager.WorldState.Fire:
                iceMonster.SetActive(false);
                fireMonster.SetActive(true);
                break;

            case GameManager.WorldState.Ice:
                iceMonster.SetActive(true);
                fireMonster.SetActive(false);
                break;
        }
    }
}