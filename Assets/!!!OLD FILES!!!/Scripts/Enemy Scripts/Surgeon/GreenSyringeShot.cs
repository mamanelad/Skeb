using UnityEngine;

public class GreenSyringeShot : MonoBehaviour
{
    [SerializeField] private GameObject greenSyringeShot;
    [SerializeField] private float timeBetweenShots = 0.35f;
    [SerializeField] private int amountOfShots = 3;
    private float _shotTimer;
    private GameObject _basePos;
    private GameObject _player;
    
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _shotTimer = timeBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        if (amountOfShots <= 0)
            Destroy(gameObject);

        if (_shotTimer > 0)
        {
            _shotTimer -= Time.deltaTime;
        }

        else
        {
            amountOfShots -= 1;
            _shotTimer = timeBetweenShots;
            var shotPos = _basePos.transform.position;
            var newShot = Instantiate(greenSyringeShot, shotPos, Quaternion.identity);
            var shotDirection = (_player.transform.position - shotPos).normalized;
            newShot.GetComponent<GreenSyringe>().SetDirection(shotDirection);
        }
    }

    public void SetBasePos(GameObject newBasePos)
    {
        _basePos = newBasePos;
    }
    
}
