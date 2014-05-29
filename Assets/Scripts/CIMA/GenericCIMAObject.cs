using UnityEngine;
using System.Collections;

public class GenericCIMAObject : MonoBehaviour {
	// Use this for initialization
	public bool visible = false;
	private bool inFocus = false;
	private Vector3 CameraPos;
	private Vector3 CameraForward;
	public int correctValue = 0;
	public Material OrangeMaterial;
	public Material RedMaterial;
	public Material YellowGreenMaterial;
	public Material GreenMaterial;
	public Material YellowMaterial;
	public GameObject player;
	private int currentValue = 3;
	private CellphoneHolder_v2 Phonescript;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		Phonescript = player.GetComponent<CellphoneHolder_v2>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Phonescript.mode==5){
		visible = renderer.isVisible;
		// Get position of player and orientation of camera
//		CameraForward = Camera.main.transform.forward;
//		CameraPos = Camera.main.transform.position;
		// Object is in focus if visible AND a raycast from the camera hits that object
//		RaycastHit raycastInfo = new RaycastHit();

		inFocus = visible /*&& (GetComponent<Collider>().Raycast(new Ray (CameraPos, CameraForward), out raycastInfo, Mathf.Infinity))*/;
		if(GameObject.Find("PhoneCamera")){
		if(inFocus){
				if(Input.GetKeyDown (KeyCode.KeypadPlus)) {
					if(currentValue<5)
						currentValue++;
				}
				if(Input.GetKeyDown (KeyCode.KeypadMinus)) {
					if(currentValue>1)
						currentValue--;
				}
			}
		}
	}
	switch(currentValue){
	case 1: renderer.material = RedMaterial;
		break;
	case 2: renderer.material = OrangeMaterial;
		break;
	case 3: renderer.material = YellowMaterial;
		break;
	case 4: renderer.material = YellowGreenMaterial;
		break;
	case 5: renderer.material = GreenMaterial;
		break;
	}
	}
}