using UnityEngine;
using System.Collections;

public class TargetBehaviour : MonoBehaviour {

	public bool beenHit;
	public float moveSpeed = 1f;    // How fast to move in x axis
	public float frequency = 5f;    // Speed of sine movement
	public float magnitude = 0.1f;  // Size of sine movement

	private bool activated;
	private Vector3 originalPos;
	private Animator animator;
	private GameObject parent;

	void Start() {
		parent = transform.parent.gameObject;
		originalPos = parent.transform.position;
		animator = parent.GetComponent<Animator> ();
		ShowTarget ();
	}

	/// <summary>
	/// Called whenever the player clicks on the object.
	/// only works if you have a collider on the object
	/// </summary>
	void OnMouseDown() {
		if (!beenHit && activated) {
			beenHit = true;
			animator.Play ("Flip");

			StopAllCoroutines ();

			StartCoroutine (HideTarget());
		}
	}

	public void ShowTarget() {
		if (!activated) {
			activated = true;
			beenHit = false;
			animator.Play ("Idle");
			iTween.MoveBy (parent, iTween.Hash ("y", 1.4f, "easeType", "easeInOutExpo", "time", 0.5f, "oncomplete", "OnShown", "oncompletetarget", gameObject));
		}
	}

	public IEnumerator HideTarget() {
		yield return new WaitForSeconds (0.5f);
		// Move down to original position
		iTween.MoveBy(parent.gameObject, iTween.Hash("y", (originalPos.y - parent.transform.position.y), "easeType", "easeOutQuad", "loopType", "none", "time", 0.5f, "oncomplete", "OnHidden", "oncompletetarget", gameObject));
	}

	IEnumerator MoveTarget() {
		Vector3 relativeEndPos = parent.transform.position;

		// Check if facing left or right
		if (transform.eulerAngles == Vector3.zero) {
			// If we're going right, positive
			relativeEndPos.x = 4;
		} else {
			// Otherwise, negative
			relativeEndPos.x = -4;
		}

		float movementTime = Vector3.Distance (parent.transform.position, relativeEndPos) * moveSpeed;

		Vector3 pos = parent.transform.position;
		float time = 0f;

		while (time < movementTime) {
			time += Time.deltaTime;

			pos += parent.transform.right * Time.deltaTime * moveSpeed;
			parent.transform.position = pos + (parent.transform.up * Mathf.Sin (Time.time * frequency) * magnitude);

			yield return new WaitForSeconds (0);
		}

		StartCoroutine (HideTarget ());
	}

	void OnHidden() {
		parent.transform.position = originalPos;
		activated = false;
	}

	void OnShown() {
		StartCoroutine ("MoveTarget");
	}
}
