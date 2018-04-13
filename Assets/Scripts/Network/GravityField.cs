using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour {

	void OnTriggerStay (Collider other) {
		if (other.tag == "Data") {
			other.GetComponent<DataMove> ().DataAttract(transform.position);
		}
	}
}
