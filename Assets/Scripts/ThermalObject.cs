using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Description=========================================================
 * Accesses the database to get information about the current mesh
 * and changes the current material to the thermal material. Color
 * is determined by the value of the entry in the database.
 * 
 * colorScale function determines which color to assign to the current
 * object.
 * 
 *=====================================================================
 */


class ThermalObject : MonoBehaviour {

	public Material thermalMat = null;
	public bool activeMode = false;
	
	private double tonsOfCarbon = 0;
	private Material[] materials = null;
	private Material[] oldMats = null;

	void Start() {
		//Load material array
		//NOTE: If only 1 material, materials.length = 1
		materials = renderer.materials;

		//Save original materials
		oldMats = renderer.materials;

		//Load database information
		Object o = Resources.Load("CarbonBuildingDB",typeof(CarbonBuildingDB));
		CarbonBuildingDB cdDB = (CarbonBuildingDB)o;

		Debug.Log (this.name);
		int index = getIndex(this.name,cdDB);

		//Was the object found?
		if (index == -1) {
			Debug.LogError ("Object was not found in database, check its name! Name: "+this.name);		
		} else {
			Debug.Log (cdDB.Buildings [index].tonsOfCarbon);
			tonsOfCarbon = cdDB.Buildings [index].tonsOfCarbon;
		}

		//Load Original materials
		renderer.materials = oldMats;
	}
	
	void ThermalMode(bool on) {
		if (on) {
			for(int i =0; i<materials.Length ; i++){
				materials[i] = thermalMat;
			}
			renderer.materials = materials;
		} else {
			renderer.materials = oldMats;
		}
	}
	void Update(){
		if (Input.GetKeyDown (KeyCode.Tab)) {
			activeMode = !activeMode;
			ThermalMode(activeMode);
		}

		if (activeMode) {
			//Color change
			Color start = Color.black;
			Color end = colorScale((float)tonsOfCarbon);
			float duration = 1.0F;
			float lerp = Mathf.PingPong(Time.time,duration)/duration; 

			//Interpolate colors
			for(int i=0;i < materials.Length; i++){
				renderer.materials[i].color = Color.Lerp(start,end,lerp);
			}
		}
	}

	//Get index of a key in the database
	int getIndex(string key, CarbonBuildingDB db){

		//Look for key
		for (int i =0; i < db.Buildings.Count; i++) {
			if(db.Buildings[i].name.CompareTo(key) == 0){
				return i; 
			}	
		}
		return -1;
	}

	//Figure out scale for Tons of Carbon
	//0-2 Cyan
	//3-4 blue
	//5-6 green
	//7-8 yellow
	//9-10 red

	Color colorScale(float carbon){
		Color result = new Color(0f,0f,0f);
		if (0 <= carbon && carbon <= 2) {
			result = new Color (0f, carbon / 10, carbon / 10);
		} else if (3 <= carbon && carbon <= 4) {
			result = new Color (0f, 0f, carbon / 10);
		} else if (5 <= carbon && carbon <= 6) {
			result = new Color (0f, carbon / 10, 0f);
		} else if (7 <= carbon && carbon <= 8) {
			result = new Color (carbon / 10, carbon / 10, 0f);
		} else if (9 <= carbon && carbon <= 10) {
			result = new Color (carbon / 10, 0f, 0f);
		} else {
			Debug.LogError("Carbon measure out of range");		
		}
		return result;
	}










}