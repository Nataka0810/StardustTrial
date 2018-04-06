using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataAttract : MonoBehaviour {

	void OnTriggerStay (Collider collider) {
		if (collider.gameObject.tag == "Data") {
			collider.GetComponent<DataMove> ().DataAttract(transform.position);
		}
	}
}
