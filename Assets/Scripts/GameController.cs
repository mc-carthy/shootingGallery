using UnityEngine;
using System.Collections; // IEnumerator
using System.Collections.Generic; // List

public class GameController : MonoBehaviour {

	public static GameController _instance;
	[HideInInspector]
	public List<TargetBehaviour> targets = new List<TargetBehaviour> ();

	void Awake() {
		_instance = this;
	}

	void Start () {
		StartCoroutine ("SpawnTargets");
	}
	
	void SpawnTarget() {
		// Get a random target
		int index = Random.Range(0, targets.Count);
		TargetBehaviour target = targets [index];

		// Show selected target
		target.ShowTarget();
	}

	IEnumerator SpawnTargets() {
		yield return new WaitForSeconds (1.0f);

		// Continue forever
		while (true) {
			int numOfTargets = Random.Range (1, 4);

			for (int i = 0; i < numOfTargets; i++) {
				SpawnTarget ();
			}

			yield return new WaitForSeconds(Random.Range(0.5f * numOfTargets, 2.5f));
		}
	}
}
