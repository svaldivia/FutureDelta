using UnityEngine;
using System.Collections;

/* Description=========================================================
 * This manages the GUI on the screen
 * 
 * Features:
 * 1. Draws the minimap on the screen
 * 
 *=====================================================================
 */


public class GUIManager : MonoBehaviour {

	public RenderTexture MiniMapTexture;
	public Material MiniMapMaterial;

	private float offset;
	private bool infoToggle;
	private GameObject player;
	private bool infoOpen;

	void Start(){

		player = GameObject.Find ("Player");

	}

	void Awake () {
		offset = 5;
	}
	

	void OnGUI	 () {

		infoToggle = player.GetComponent<PlayerController> ().infoToggle;
//		Debug.Log ("Info toggle:" + infoToggle);

		float size = Screen.width / 5;	
		
		//Draw Mini Map On Screen
		if (Event.current.type == EventType.repaint) {
			Graphics.DrawTexture (new Rect (offset,Screen.height - size, size, size), MiniMapTexture, MiniMapMaterial);
		}

		//Check if player has spotted a carbon building
		if (infoToggle) {
//			Debug.Log("Drawing info");
//			GUI.Box(new Rect(Screen.width/4, Screen.height/4,40,40),"!!!");

			if (Input.GetMouseButtonDown(0) && !infoOpen) {
					infoOpen = true;
					Debug.Log ("SHOW ME DA INFO");
					GUI.Box(new Rect(Screen.width/4, Screen.height/4,40,40),"!!!");
			}
		} else {
			infoOpen = false;
		}

	}
}
