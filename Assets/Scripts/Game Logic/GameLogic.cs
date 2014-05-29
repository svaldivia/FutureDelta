using UnityEngine;
using System.Collections;

/* Description=============================================
 * The default script that manages the logic of how the game works.
 * 
 * Functionality:
 * 1. Change the skybox material with Carbon Vision
 * 2. Change the main camera's image effects
 * 
 *========================================================
 */

public class GameLogic : MonoBehaviour {
	
	public Material skyboxCarbonV;
	public Material normalSky;


	private GameObject mainCamera;
	private bool CarbonVOn = false;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Tab)) {

			//Change the material of the skybox
			CarbonVOn = !CarbonVOn;
			if(CarbonVOn){
				RenderSettings.skybox = skyboxCarbonV;
			}else {
				RenderSettings.skybox = normalSky;
			}

			//Set main camera image effects
			mainCamera.GetComponent<Bloom>().enabled = !mainCamera.GetComponent<Bloom>().enabled;
			mainCamera.GetComponent<NoiseAndGrain>().enabled = !mainCamera.GetComponent<NoiseAndGrain>().enabled;
			mainCamera.GetComponent<ContrastEnhance>().enabled = !mainCamera.GetComponent<ContrastEnhance>().enabled;
			mainCamera.GetComponent<Crease>().enabled = !mainCamera.GetComponent<Crease>().enabled;
			mainCamera.GetComponent<GlowEffect>().enabled = !mainCamera.GetComponent<GlowEffect>().enabled;
		}
	}


}
