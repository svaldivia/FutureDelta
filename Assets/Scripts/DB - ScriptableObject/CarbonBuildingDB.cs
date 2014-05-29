using System.Collections.Generic;
using UnityEngine;

/* Description=================================================
 * ScriptableObjects that contains an array of CarbonU 
 * (carbon units). Each carbon unit represents the information
 * of a single building.
 * 
 * This acts like a database that can be stored as an asset
 * 
 *============================================================
 */

public class CarbonBuildingDB : ScriptableObject {

	public List<CarbonU> Buildings;
	public CarbonBuildingDB (List<CarbonU> content){
		this.Buildings = content;
	}
}
