using UnityEngine;

[System.Serializable]
public class Wave
{
    
    [Range(0, 1)] [SerializeField] public float bigPercentage = .33f;
    [Range(0, 1)] [SerializeField] public float middlePercentage = .33f;
    [Range(0, 1)] [SerializeField] public float smallPercentage = .33f;

    [SerializeField] public int monsterAmount = 8;
    [SerializeField] public float timeToSpawnStep = 1f;

}