using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class main : MonoBehaviour {

	public GameObject R;
	public GameObject cube;
	public GameObject[,,] Rubik = new GameObject[3,3,3];

	private Ray ray;
	private RaycastHit hit;
	private GameObject target;
	private Vector3 vpos;

	private bool rotating = false;
	private char dir;
	private GameObject core;
	private int c = 0;


	void RubikCube(){
		for (int i = 0; i < 3; i++) {
			for(int j = 0; j < 3; j++){
				for(int k = 0; k < 3; k++){
					Rubik[i, j, k] = Instantiate(cube, new Vector3(i, j, k), cube.transform.localRotation) as GameObject;
					Rubik[i, j, k].transform.parent = cube.transform.parent;
				}
			}
		}
	}

	void Start(){
		RubikCube ();
		cube.SetActive (false);
	}

	int p_index;
	//private bool select = false;
	private float xDeg;
	private float yDeg;

	private Quaternion fromRotation;
	private Quaternion toRotation;

	void Update () {

		//Obtencio de l'objecte que esta clicant el Ratoli
		if (Input.GetMouseButtonDown (1)) {
			vpos = Input.mousePosition;
			ray = Camera.main.ScreenPointToRay(vpos);
			if(Physics.Raycast (ray, out hit) ){
				target = hit.collider.gameObject;

				Debug.Log (target.name);
				Debug.Log ("Rotacio: " + target.transform.localRotation.ToString() +
				           " Posicio: " + target.transform.localPosition.ToString());
			}

		}
	
		if (Input.GetKeyDown (KeyCode.W) && !rotating) {
			dir = 'X';
			p_index = (int)target.transform.localPosition.x;
			core = get_core (p_index, dir);
			get_plane (core, dir);
			rotating = true;

		} else if (Input.GetKeyDown (KeyCode.S) && !rotating) {
			dir = 'x';
			p_index = (int)target.transform.localPosition.x;
			core = get_core (p_index, dir);
			get_plane (core, dir);
			rotating = true;
		}

		if(rotating){
			if(dir == 'X'){
				float x = 90, y = 0, z = 0;
				rotate(core, x, y, z);
			}else if (dir == 'x'){
				toRotation = Quaternion.Euler(fromRotation.x - 90, fromRotation.y, fromRotation.x );
				fromRotation = core.transform.rotation;
				core.transform.rotation = Quaternion.Lerp (fromRotation, toRotation, Time.deltaTime * 10);
			}
			c++;
			//aquesta condicio ens permet saber en quin moment ha acabat la rotacio.
			if (c == 40) {
				rotating = false;
				c = 0;
				//amb aquesta accio tots els Cubs passen altre cop al parent del Rubik.
				desfer_plane(core, dir);
				//rectifica les petites variacions del transform degut a la rotacio.
				rectificador();
			}
		}

	}
	void rotate(GameObject core, float x, float y, float z){
		fromRotation = core.transform.localRotation;
		toRotation = Quaternion.Euler(fromRotation.z + z, fromRotation.y + y, fromRotation.x + x );
		core.transform.localRotation = Quaternion.Lerp (fromRotation, toRotation, Time.deltaTime * 10);
	}
	//rectificador de posicions
	float rect (float v){
		if (v > 0.8 && v < 1.2) {
			return 1;
		} else if (v > -0.2 && v < 0.2) {
			return 0;
		} else if (v > -1.2 && v < -0.8) {
			return -1;
		} else {
			return 0;
		}
	}
	//rectificador de rotacions
	float rect_rot (float r){
		if (r > 357 && r < 360) {
			return 0;
		} else if (r > 87 && r < 93) {
			return 90;
		} else if (r > 177 && r < 183) {
			return 180;
		} else if (r > 267 && r < 273) {
			return 270;
		} else {
			return 0;
		}
		
	}
	// Rectificador del Transform, degut a que quan es fan rotacions hi ha petites variacions de posicio, rotacio
	// i escala que ens poden portar problemes. D'aquesta manera es passa qualsevol valor a un nombre enter.
	void rectificador (){
		/*int x = (int)core.transform.localPosition.x;
		int y = (int)core.transform.localPosition.y;
		int z = (int)core.transform.localPosition.z;
		Debug.Log (x + " " + y + " " + z);
		core.transform.localPosition = new Vector3 (x, y, x);
		return core;
		*/
		for (int i = 0; i < 3; i++) {
			for(int j = 0; j < 3; j++){
				for(int k = 0; k < 3; k++){
					Vector3 pos = Rubik[i, j, k].transform.localPosition;
					Rubik[i, j, k].transform.localPosition = new Vector3(rect (pos.x),
					                                                	rect (pos.y),
					                                               		rect (pos.z)
					                                                	);

					Vector3 rot = Rubik[i, j, k].transform.localRotation.eulerAngles;
					rot = new Vector3(rect_rot(rot.x), rect_rot (rot.y), rect_rot (rot.z));
					Rubik[i, j, k].transform.localRotation = Quaternion.Euler (rot);
				}
			}
		}	

	}
	//
	void estat_inicial(){
		for (int i = 0; i < 3; i++) {
			for(int j = 0; j < 3; j++){
				for(int k = 0; k < 3; k++){
					Rubik[i, j, k].transform.parent = cube.transform.parent;
				}
			}
		}
	}
	//fa que tots els cubs tornin a ser independents dins el Rubik, desfa el pla.
	void desfer_plane (GameObject core, char p){

		if (p == 'x' || p == 'X') {
			for (int i = 0; i < 3; i++) {
				for(int j = 0; j < 3; j++){
					Rubik[p_index, i, j].transform.parent = core.transform.parent;
					
				}
			}
			//Debug.Log("success");
		} else if (p == 'y') {
			for (int i = 0; i < 3; i++) {
				for(int j = 0; j < 3; j++){
					Rubik[i, p_index, j].transform.parent = core.transform;
				}
			}
		} else if (p == 'z') {
			for (int i = 0; i < 3; i++) {
				for(int j = 0; j < 3; j++){
					Rubik[i, j, p_index].transform.parent = core.transform;
				}
			}
		}
	}
	//crea un pla
	void get_plane (GameObject core, char p){

		if (p == 'x' || p == 'X') {
			for (int i = 0; i < 3; i++) {
				for(int j = 0; j < 3; j++){
					Rubik[p_index, i, j].transform.parent = core.transform;

				}
			}
			//Debug.Log("success");
		} else if (p == 'y') {
			for (int i = 0; i < 3; i++) {
				for(int j = 0; j < 3; j++){
					Rubik[i, p_index, j].transform.parent = core.transform;
				}
			}
		} else if (p == 'z') {
			for (int i = 0; i < 3; i++) {
				for(int j = 0; j < 3; j++){
					Rubik[i, j, p_index].transform.parent = core.transform;
				}
			}
		}


	}
	//busca el cub central, referencia de les rotacions
	GameObject get_core(int p_index, char p){

		if (p == 'x' || p == 'X') {
			return Rubik [p_index, 1, 1];
		} else if (p == 'y') {
			return Rubik [1, p_index, 1];
		} else if (p == 'z') {
			return Rubik [1, 1, p_index];
		} else {
			return null;
		}
	


	}


}
