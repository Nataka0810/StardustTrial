using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarAttract : MonoBehaviour {

	void OnTriggerStay (Collider collider) {
		if (collider.gameObject.tag == "Star") {
			collider.GetComponent<StarMove> ().StarAttract(transform.position);
		}
	}
}
