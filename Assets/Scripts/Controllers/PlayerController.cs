using UnityEngine;
using System.Collections;

/* Description=============================================
 * Custom player controller (under continuous development)
 * 
 * This player controller includes the basic movement and 
 * allows some modification of certain movements. In addition,
 * it can also run and fly!
 * 
 * Features:
 * 1. Modifiable attributes
 * 2. Fly mode
 * More in development
 *========================================================
 */

[RequireComponent (typeof(CharacterController))]

public class PlayerController : MonoBehaviour {
	
	//Public Variables--------------------

	public float movementSpeed = 5.0f;
	public float flyingSpeed = 10.0f;
	public float mouseSensitivity = 3.0f;
	public float upDownRange = 60.0f;
	public float jumpSpeed = 10.0f;
	public float flyVelocity = 10.0f;
	public float maxHeight = 20.0f;
	public float upSpeed = 0.2f;

	//Flight Momentum
	public float acc = 1.0f;
	public float maxSpeed = 20.0f;

	public float visionRange = 5.0f;
	public bool infoToggle = false;

	public AudioClip wind;

	float curSpeedV = 0;
	float curSpeedH = 0;
	float rotUpDown = 0;
	float verticalVelocity = 0;
	
	//Private Variables-------------------
	
	private bool fly = false;
	private float maxH;

	
	CharacterController characterController;
	
	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
		characterController = GetComponent<CharacterController>();
		maxH = maxHeight;
	}
	
	// Update is called once per frame
	void Update () {
		
		
		//Rotation
		float rotLeftRight = Input.GetAxis("Mouse X")*mouseSensitivity;
		transform.Rotate(0,rotLeftRight,0);
		
		rotUpDown -= Input.GetAxis("Mouse Y")*mouseSensitivity;
		rotUpDown = Mathf.Clamp(rotUpDown,-upDownRange,upDownRange);
		Camera.main.transform.localRotation = Quaternion.Euler(rotUpDown,0,0);

		
		//Movement
		float forwardSpeed = Input.GetAxis("Vertical")*movementSpeed;
		float sideSpeed = Input.GetAxis("Horizontal")*movementSpeed;
		
		verticalVelocity += Physics.gravity.y*Time.deltaTime;
		
		if(characterController.isGrounded && Input.GetButtonDown("Jump")){
			verticalVelocity = jumpSpeed;
		}
		
		//Sprint
		if(Input.GetKey(KeyCode.LeftShift)){
			forwardSpeed += 20;
		}
		
		
		//=======================FLYING PART=================================
		
		if(Input.GetKeyDown("f")){
			fly = !fly;
		}
		
		if(fly){
			
			audio.PlayOneShot(wind);
			
			verticalVelocity = flyVelocity;
			
			curSpeedH += Input.GetAxis("Horizontal")*flyingSpeed*acc;
			curSpeedH = Mathf.Clamp(curSpeedH,-maxSpeed, maxSpeed); //clamps curSpeed
			
			if(Input.GetAxis("Horizontal")==0){
				if(curSpeedH > 0) 
				{
					curSpeedH -= acc;
				}
				if(curSpeedH < 0){
					curSpeedH += acc;
				}
			}
			sideSpeed = curSpeedH;
			
			
			curSpeedV += Input.GetAxis("Vertical")*flyingSpeed*acc;
			curSpeedV = Mathf.Clamp(curSpeedV,-maxSpeed, maxSpeed); //clamps curSpeed
			
			if(Input.GetAxis("Vertical")==0){
				if(curSpeedV > 0) 
				{
					curSpeedV -= acc;
				}
				if(curSpeedV < 0){
					curSpeedV += acc;
				}
			}
			
			forwardSpeed = curSpeedV;	
			
			if(Input.GetKey(KeyCode.Z)){
				maxHeight = characterController.transform.position.y;
				verticalVelocity = -verticalVelocity;
			}
			
			if(Input.GetKey(KeyCode.Space)){
				maxHeight += upSpeed;
			}
			
			if(characterController.transform.position.y >maxHeight){
				verticalVelocity -= flyVelocity; 
			}
		}else{
			if(characterController.isGrounded){
				maxHeight = maxH;
			}else{
				maxHeight = characterController.transform.position.y;
			}
		}
		
		//=====================================================================
		
		//Set speed and move character
		Vector3 speed = new Vector3(sideSpeed,verticalVelocity,forwardSpeed);
		speed = transform.rotation * speed;
		
		//The time since the last frame: Time.deltaTime
		characterController.Move (speed*Time.deltaTime);


		//==================Ray Cast for Carbon Vision=========================

		RaycastHit hit;

		Ray visionRay = new Ray (Camera.main.transform.position, Camera.main.transform.forward);

		//Draw ray for debug purposes
		Debug.DrawRay (Camera.main.transform.position, Camera.main.transform.forward * visionRange);

		//Check for ray hit
		if (Physics.Raycast (visionRay, out hit, visionRange)) {
//			Debug.LogWarning ("Hitting something");
			infoToggle = true;
		} else {
//			Debug.LogWarning("NOTHING");	
			infoToggle = false;
		}
	}


}
