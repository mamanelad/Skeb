 using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]
public class EnemyAI : MonoBehaviour {

	// What to chase?
	private Transform target;
	
	// How many times each second we will update our path
	[SerializeField] private float maxUpdateRate = 20f;
	[SerializeField] private float minUpdateRate = 2f;
	private float _updateRate;
	
	// Caching
	private Seeker seeker;
	private Rigidbody2D rb;
	
	//The calculated path
	[SerializeField] private Path path;
	
	//The AI's speed per second
	[SerializeField] float maxSpeed = 800f;
	[SerializeField] float minSpeed = 100f;
	private float _speed;
	
	[SerializeField] ForceMode2D fMode;
	
	[HideInInspector]
	public bool pathIsEnded = false;
	
	// The max distance from the AI to a waypoint for it to continue to the next waypoint
	[SerializeField] private float maxNextWaypointDistance = 20f;
	[SerializeField] private float minNextWaypointDistance = 1f;
	private float NextWaypointDistance;
	
	// The waypoint we are currently moving towards
	private int currentWaypoint = 0;

	private bool _searchingPlayer;

	[SerializeField] private float minMass = 1;
	[SerializeField] private float maxMass = 5;

	[SerializeField] private float minLinearDrag = 1f;
	[SerializeField] private float maxLinearDrag = 6f;
	
	
	void Start () {
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();

		rb.mass = Random.Range(minMass, maxMass);
		rb.drag = Random.Range(minLinearDrag, maxLinearDrag);
		
		_updateRate = Random.Range(minUpdateRate, maxUpdateRate);
		_speed = Random.Range(minSpeed, maxSpeed);
		NextWaypointDistance = Random.Range(minNextWaypointDistance, maxNextWaypointDistance);
		
		if (target == null) {
			if (!_searchingPlayer)
			{
				_searchingPlayer = true;
				StartCoroutine(SearchPlayer());
			}
			return;

			
		}
		
		// Start a new path to the target position, return the result to the OnPathComplete method
		seeker.StartPath (transform.position, target.position, OnPathComplete);
		
		StartCoroutine (UpdatePath ());
	}

	IEnumerator SearchPlayer()
	{
		GameObject sResult = GameObject.FindGameObjectWithTag("Player");
		if (sResult == null)
		{
			yield return new WaitForSeconds(0.5f);
			StartCoroutine(SearchPlayer());
		}
		else
		{
			target = sResult.transform;
			_searchingPlayer = false;
			StartCoroutine(UpdatePath());
			yield return false;
		}
	}
	
	IEnumerator UpdatePath () {
		if (target == null) {
			if (!_searchingPlayer)
			{
				_searchingPlayer = true;
				StartCoroutine(SearchPlayer());
			}
			yield return false;
		}
		
		// Start a new path to the target position, return the result to the OnPathComplete method
		seeker.StartPath (transform.position, target.position, OnPathComplete);
		var currUpdateRate = _updateRate;
		yield return new WaitForSeconds ( 1f/currUpdateRate);
		StartCoroutine (UpdatePath());
	}
	
	public void OnPathComplete (Path p) {
		// Debug.Log ("We got a path. Did it have an error? " + p.error);
		if (!p.error) {
			path = p;
			currentWaypoint = 0;
		}
	}
	
	void FixedUpdate () {
		
		if (target == null) {
			if (!_searchingPlayer)
			{
				_searchingPlayer = true;
				StartCoroutine(SearchPlayer());
			}
			 return;
		}
		
		
		if (path == null)
			return;
		
		if (currentWaypoint >= path.vectorPath.Count) {
			if (pathIsEnded)
				return;
			
			// Debug.Log ("End of path reached.");
			pathIsEnded = true;
			return;
		}
		pathIsEnded = false;
	
		//Direction to the next waypoint
		Vector3 dir = ( path.vectorPath[currentWaypoint] - transform.position ).normalized;
		var curSpeed = _speed;
		dir *= curSpeed * Time.fixedDeltaTime;
		
		//Move the AI
		rb.AddForce (dir, fMode);
		
		float dist = Vector3.Distance (transform.position, path.vectorPath[currentWaypoint]);
		var nextWaypointDistance = NextWaypointDistance;
		if (dist < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
	}
	
}
