using UnityEngine;
using System.Collections;

public class CellphoneHolder_v2 : MonoBehaviour {
	
	// The prefab to use for the phone object.
	public GameObject phonePrefab;
	// Whether the player has guaranteed access to the phone. Overrides the value given in the game instance if such a value is present.
	public bool hasPhone;
	// Materials to render on the screen for various modes.
	public Material currentTaskListImage;
	public Material screenMenuImage;
	public Material camARImage;
	public Material menuRTT;
	public GameObject phonecam;
	public GameObject maincam;
	public Material mapImage;
	public Shader shadetype;
	public Camera mapcam;
	public GameObject arObject;
	
	// TODO: Indicates which of the future scenarios to view in the AR mode, if applicable.
	int currentAROption;
	
	//mode
	public int mode = 0; //0=off, 1=menu, 2=map, 3=notes, 4=ARcamera, 5=CIMAcamera
	public int selected = 0; //0=map, 1=notes, 2=arcamera, 3=messages, 4=cimacamera
	
	Texture2D maptexture;
	Texture2D notetexture;
	Texture2D cameratexture;
	
	string Objectives = "";
	[HideInInspector] public Vector2 scrollPosition = Vector2.zero;
	
	// The phone object used in the game view.
	[HideInInspector] public GameObject phoneObj;
	// The position to instantiate the phone at and move it to each time it is lowered
	private Vector3 basePos;
	// The z-distance to keep the phone away from the camera. Must keep higher than the phone's thickness in order to avoid clipping.
	// (If necessary, change the near clipping distance on the main camera to be lower than this value.)
	private float distanceToCamera = 0.15f;
	
	// The phone's screen and various coordinates of its screen.
	public Transform screen;
	private Vector3 screenTopLeft = new Vector3(Screen.width/3,Screen.height/2,100), screenLowerRight = new Vector3(Screen.width*2/3,Screen.height,500), screenCenter = new Vector3(Screen.width/2,Screen.height*3/4,300);
	
	// Keeps track of various states of the phone: whether the phone is currently in use, whether the phone is in a transition mode, or is to print outstanding tasks during the Task View mode, or is in player name entry mode.
	public bool phoneInUse = false, transition = false, inputtingName = false;
	
	// The player's name is assigned by "writing" it on the back of the phone. This is passed to the GameData instance so it can be referenced in dialogue.
	//private string nameAssign = "Enter Name Here";

	
	/// Always-active functions
	
	void Start() {
		 /* if (hasPhone == false) hasPhone = (GameData.Instance.CollectedItems.NumberOf("phone") >= 0)*/
		maincam = GameObject.FindGameObjectWithTag("MainCamera");
		//mapcam = (Camera) Instantiate(maincam.camera, new Vector3(0, 0, 0), Quaternion.FromToRotation(new Vector3(0, 0, 0), new Vector3(0, 0, 1)));
		//mapImage.mainTexture = (Texture)mapcam.targetTexture;
		camARImage = new Material(shadetype);
		menuRTT = new Material(shadetype);
		basePos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y-0.3f, Camera.main.transform.position.z+0.5f);
		phoneObj = (GameObject)Instantiate(phonePrefab, basePos, Camera.main.transform.rotation);
		phoneObj.transform.parent = Camera.main.transform;
		//phoneObj.layer = 14;
		phoneObj.SetActive (false);
		screen = phoneObj.transform.Find ("Screen");
		if (screenMenuImage == null) screenMenuImage = screen.renderer.material;
		transition = false;
	}
	
	void Update() {
			if(Input.GetKeyDown (KeyCode.P)) {
				if(!phoneInUse && !transition){
					StartCoroutine (RaisePhone (true));
			}
				if(phoneInUse && !transition){
				mode=0;
				StartCoroutine (LowerPhone ());
			}
			}
			if(Input.GetKeyDown (KeyCode.Q)) {
			if(phoneInUse && mode==1){
			if(selected<4){
				selected++;
				}else{selected=0;
			}}
			if(phoneInUse && mode!=1 && mode!=0){
				if(mode==4 || mode==5){
					StartCoroutine (RaisePhone (true));
					screen.renderer.material = screenMenuImage;
				}else{
				mode=1;
				}
			}
		}
			if(Input.GetKeyDown (KeyCode.Return)) {
			if(phoneInUse && !transition){
					switch(selected){
				case 1: mode=2;
				break;
				case 3: mode=3;
				break;
				case 2:
				StartCoroutine (ARViewStart());
					mode=4;
				break;
				case 4:
				StartCoroutine (ARViewStart());
				mode=5;
					break;
				}
			}
			}
	}
	
	void OnGUI() {
		if(mode==5 && !transition){
			phonecam.camera.transform.position = maincam.transform.position;
			phonecam.transform.forward = maincam.transform.forward;
			phonecam.camera.cullingMask = ((1 << LayerMask.NameToLayer("TransparentFX")) | (1 << LayerMask.NameToLayer("carbonobject")) | (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Water")));
			phonecam.transform.Rotate(Vector3.forward * -90);
			camARImage.SetTexture("_MainTex",(Texture)phonecam.camera.targetTexture);
			screen.renderer.material = camARImage;
		}
		if(mode==4 && !transition){
			phonecam.camera.transform.position = maincam.transform.position;
			phonecam.transform.forward = maincam.transform.forward;
			phonecam.camera.cullingMask = ((1 << LayerMask.NameToLayer("TransparentFX")) | (1 << LayerMask.NameToLayer("BARRIERISLANDS")) | (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Water")));
			phonecam.transform.Rotate(Vector3.forward * -90);
			camARImage.SetTexture("_MainTex",(Texture)phonecam.camera.targetTexture);
			screen.renderer.material = camARImage;
		}
		if(mode==1){
		switch(selected){
			case 1:
		menuRTT.SetTexture ("_MainTex",(Texture)Resources.Load("MapSelect"));
		screen.renderer.material = menuRTT;
			break;
			case 3:
		menuRTT.SetTexture ("_MainTex",(Texture)Resources.Load("NoteSelect"));
		screen.renderer.material = menuRTT;
				break;
			case 2:
		menuRTT.SetTexture ("_MainTex",(Texture)Resources.Load("CameraSelect"));
		screen.renderer.material = menuRTT;
				break;
				case 0:
		menuRTT.SetTexture ("_MainTex",(Texture)Resources.Load("MessageSelect"));
		screen.renderer.material = menuRTT;
				break;
			}
		}
		if (mode == 2) {
				}
		if(mode==3){
			GUI.BeginGroup(new Rect(screenTopLeft.x + 112,screenTopLeft.y + 15, 375, 217));
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(380), GUILayout.Height(220));
			GUILayout.Label("THIS IS A TEST OBJECTIVE - " + Objectives);
        	GUILayout.EndScrollView();
			GUI.EndGroup();
		}
	}
	
	public IEnumerator RaisePhone(bool test) {
		if (hasPhone) {
			transition = true;
			phoneInUse = true;
			phoneObj.gameObject.SetActive(true);
			yield return StartCoroutine (RaiseMotion (phoneObj.transform, new Vector3(0.05f, -0.04f, distanceToCamera), Quaternion.Euler(0.0f, -90.0f, 90.0f), 0.5f));
			transition = false;
			if(test){
				mode=1;
			}
		}
		else Debug.Log ("Phone not available yet.");
		yield return 0.0f;
		transition=false;
	}
	
	public IEnumerator LowerPhone() {
		if (hasPhone) {
			transition = true;
			phoneInUse = false;
			if (phoneObj != null) {
				screen.renderer.material = screenMenuImage;
				yield return StartCoroutine(RaiseMotion (phoneObj.transform, new Vector3(0.0f, -0.3f, 0.5f), Quaternion.Euler(0.0f, 0.0f, 0.0f), 0.5f));
				transition = false;
				mode=0;
				transition=false;
				phoneObj.gameObject.SetActive(false);
				arObject.SetActive (false);
			}
		}
		yield return 0.0f;
	}
	
	public IEnumerator ARViewStart() {
		// Moves phone to center of screen and rotates it to landscape view
		transition=true;
		yield return StartCoroutine (RaiseMotion (phoneObj.transform, new Vector3(0.0f, 0.0f, distanceToCamera*0.9f), Quaternion.Euler (0.0f, -90.0f, 90.0f), 0.5f));
		transition = false;
		phonecam = GameObject.Find("PhoneCamera");
		// Change screen texture to the view of a new camera at the phone's position that only has the AR layer visible
		
		//TODO: Change visible layers according to chosen function
		arObject.SetActive (true);
	}

	
	// Sets an object's local position and rotation to new values over time. In this case, used for the phone with TransitionMode.
	private IEnumerator RaiseMotion(Transform obj, Vector3 endPos, Quaternion newRotation, float duration) {
		TransitionMode ();
		float i = 0.0f;
		Vector3 startPos = obj.transform.localPosition;
		float speed = 1.0f/duration;
		Quaternion startRotation = obj.transform.localRotation;
		while (i < 1.0f) {
			i += Time.deltaTime * speed;
			if (obj != null) {
				obj.localPosition = Vector3.Lerp(startPos, endPos, i);
				obj.localRotation = Quaternion.Slerp (startRotation, newRotation, i);
			}
			
			yield return 0.0f;
		}
		screenCenter = Camera.main.WorldToScreenPoint (screen.transform.position);
		//RecalculateBounds (screen.transform);
	}
	
	private void TransitionMode() {
		// Turns on the variable indicating a transition state, which affects some GUI functions.
		transition = true;

		//TODO: find a more elegant way to deal with future scenarios and colliders!
		arObject.SetActive (false);
	}

	// Finds opposite corner points and a centrepoint for an object (in this script, the phone's screen).
	private void RecalculateBounds(Transform obj) {
		screenCenter = transform.InverseTransformPoint(obj.renderer.bounds.center);
		screenTopLeft = Camera.main.WorldToScreenPoint(transform.TransformPoint(screenCenter + new Vector3(-obj.renderer.bounds.extents.x, obj.renderer.bounds.extents.y, 0)));
		screenLowerRight = Camera.main.WorldToScreenPoint(transform.TransformPoint(screenCenter + new Vector3(obj.renderer.bounds.extents.x, -obj.renderer.bounds.extents.y, 0)));
		Debug.Log ("Changed to new screen coordinates: " + screenTopLeft.x+","+screenTopLeft.y + " through " + screenLowerRight.x+","+screenLowerRight.y);
	}
}
