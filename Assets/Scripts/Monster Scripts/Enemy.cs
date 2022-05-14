using System;
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

	public CircleCollider2D areaCollider2D;
	private float _currHealth;
	[SerializeField] private float startHealth = 100;

	[SerializeField]
	private EnemySpawnerDots _enemySpawnerDots;

	private void Start()
	{
		_currHealth = startHealth; 
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			AttackPlayer();
		}

		if (other.CompareTag("Enemy"))
		{
			AnotherEnemyInteraction(other);
		}
	}



	public void DamageEnemy (int damage) {
		_currHealth -= damage;
		if (_currHealth <= 0)
		{
			KillEnemy();
		}
	}


	private void KillEnemy()
	{
		Destroy(this.gameObject);
		_enemySpawnerDots.DecreaseMonster();
	}

	private void AttackPlayer()
	{
		
	}

	private void AnotherEnemyInteraction(Collider2D other)
	{
		
	}
}
