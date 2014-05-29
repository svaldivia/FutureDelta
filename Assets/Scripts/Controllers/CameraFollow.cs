using UnityEngine;
using System.Collections;

/* Description=============================================
 * This script follows the target's position and z rotation.
 * 
 * It is mainly used to control the movement of the mini
 * map camera.
 * 
 *========================================================
 */


public class CameraFollow : MonoBehaviour {

	public Transform Target; //For the mini map this is the player
	public float mouseSensitivity = 3.0f;

	void LateUpdate(){

		//Obtain position from target
		transform.position = new Vector3 (Target.position.x, transform.position.y, Target.position.z);
		float rotLeftRight = Input.GetAxis("Mouse X")*mouseSensitivity;
		transform.Rotate (0, 0, -rotLeftRight);	
	}
}
