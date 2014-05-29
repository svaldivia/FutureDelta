using UnityEngine;
using System.Collections;

//To implement this minimap script, make sure that your minimap is a picture of the entire terrain (even if you can't walk around the entire terrain)
//Once the script is on the player, drag in the minimap picture, a background picture (just a black square or something), the terrain, and a player icon
//The minimap should scroll with your player


public class MovingMiniMapScript_v2 : MonoBehaviour {
	//Textures:
	public GUIContent minimap;
	public GUIContent playerIcon;
	public GUIContent bg;
	public float mapOnScreenScale = 0.2f; // the scale of the size of the minimap on screen (divided by this number)
	
	//GUI placement numbers:
	float mapHeight;
	private Vector3 screenTopLeft = new Vector3 (Screen.width / 3, Screen.height / 2, 100); 
//	screenLowerRight = new Vector3(Screen.width*2/3,Screen.height,500), screenCenter = new Vector3(Screen.width/2,Screen.height*3/4,300);
	float mapWidth;
	public Texture2D texture;
	public float borderPadding = 10;
	Vector2 mapPosition;
	public Terrain terrain;
	Vector3 terrainSize;
	public Material imageMap;
	float ratioX; // ratio of the terrain to the map
	float ratioY;
	float playerMapX;//player's position on the miniMap
	float playerMapY;
	
	//Player's transform variable:
	public Transform player;
	
	//Player's phone-holder component
	private CellphoneHolder_v2 phoneScript;
	
	// Use this for initialization
	void Start () {
		//mapHeight & width
		mapHeight = minimap.image.height;
		mapWidth = minimap.image.width;
		//Position of the map:
		//terrain size:
		terrainSize = terrain.terrainData.size;
		//ratio of terrain to miniMap:
		ratioX = mapWidth/terrainSize.x;
		ratioY = mapHeight/terrainSize.z;
		texture = (Texture2D)Resources.Load("minimap");
		print("Terrain Y:" + terrainSize.z);
		print("Terrain x:" + terrainSize.x);
		imageMap = new Material(Shader.Find("Diffuse"));
		
		if (player == null) player = GameObject.FindGameObjectWithTag ("Player").transform;
		phoneScript = player.GetComponent<CellphoneHolder_v2>();
		
	}
	
	// Update is called once per frame
	void Update () {
		//position of player on miniMap:
		playerMapX = player.position.x*ratioX;
		playerMapY = mapHeight - player.position.z*ratioY;
	}
	
	void OnGUI(){
		GUI.depth = 0;
		phoneScript = player.GetComponent<CellphoneHolder_v2>();
		if (phoneScript.mode==2) {
			imageMap.SetTexture ("_MainTex", (Texture)texture);
			GUILayout.BeginArea(new Rect(screenTopLeft.x + 112,screenTopLeft.y + 15, 375, 217));
			//how to move the map...
			GUI.DrawTexture(new Rect(mapWidth/(2) - playerMapX - 300, mapHeight/(2) - playerMapY - 413, mapWidth, mapHeight), minimap.image);
			//Draw the player
			GUI.DrawTexture(new Rect(mapWidth/(2) - 315, mapHeight/(2) - 413, playerIcon.image.width, playerIcon.image.height), playerIcon.image);
			GUILayout.EndArea();
			phoneScript.screen.renderer.material = imageMap;
			
		}
	}
}
 