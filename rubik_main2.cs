using UnityEngine;
using System.Collections;

public class main2 : MonoBehaviour {

	//Rubik
	public GameObject R; 									
	//Cube
	public GameObject cube; 								
	//Rubik cube muntat
	private GameObject[,,] Rubik = new GameObject[3,3,3];

	private GameObject[,,] Rubik_original = new GameObject[3, 3, 3];

	//variables deteccio de target
	private Ray ray;
	private RaycastHit hit;
	private GameObject target;
	private Vector3 vpos;

	//rotacio on/of
	private bool rotating = false;
	//direccio que es moura {X,x, Y,y)
	private char dir;
	//nucli, el qual girara el pla
	private GameObject core;
	//index del pla
	int p_index;
	//contador de frames
	private int f = 0;



	//Creacio del rubik's cube
	void RubikCube(){
		for (int i = 0; i < 3; i++) {
			for(int j = 0; j < 3; j++){
				for(int k = 0; k < 3; k++){
					Rubik[i, j, k] = Instantiate(cube, new Vector3(i-1, j-1, k-1), cube.transform.rotation) as GameObject;
					Rubik[i, j, k].transform.parent = cube.transform.parent;
				}
			}
		}
	}
	GameObject get_core(int p_index, char p){
		
		if (p == 'x' || p == 'X') {
			//Debug.Log ("HOLA");
			return Rubik [1, 1, p_index];

		} else if (p == 'y' || p == 'Y') {
			//Debug.Log ("HOLA");
			return Rubik [1, p_index, 1];

		} else {

			return null;
		}
	}
	void get_plane (GameObject core, char p, int p_index){
		Debug.Log (Rubik [0, 0, p_index].transform.position.ToString ());
		if (p == 'x' || p == 'X') {
			for (int i = 0; i < 3; i++) {
				for(int j = 0; j < 3; j++){

					Rubik[i, j, p_index].transform.parent = core.transform;
					//Rubik[p_index, i, j].transform.parent = core.transform;	
				}
			}

		} else if (p == 'y' || p == 'Y') {
			for (int i = 0; i < 3; i++) {
				for(int j = 0; j < 3; j++){
					Rubik[i, p_index, j].transform.parent = core.transform;
				}
			}
		}		
		
	}

	//variables de rotacio

	private Quaternion fromRotation;
	private Quaternion toRotation;

	void rotate(GameObject core, float x, float y){
		fromRotation = core.transform.localRotation;
		float a = fromRotation.x, b = fromRotation.y + y, c = fromRotation.z + x;
		toRotation = Quaternion.Euler(a, b, c);
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
	//fa que tots els cubs tornin a ser independents dins el Rubik, desfa el pla.
	void desfer_plane (GameObject core, char p){
		
		if (p == 'x' || p == 'X') {
			for (int i = 0; i < 3; i++) {
				for(int j = 0; j < 3; j++){
					Rubik[i,j,p_index].transform.parent = core.transform.parent;
					
				}
			}
			Debug.Log("success");
			//Debug.Log("success");
		} else if (p == 'y' || p == 'Y') {
			for (int i = 0; i < 3; i++) {
				for(int j = 0; j < 3; j++){
					Rubik[i, p_index, j].transform.parent = core.transform.parent;
				}
			}
		}
	}
	/* 
		ATENCIO S'HA DE FER UNA RECTIFICACIO DE POSICIONS EN LA MATRIU!!!!

		 */
	void Rubik_rect(){
		GameObject[,,] Rubik_copia = Rubik_original;
		//Tots els Rubik[i, j, k] han de ser igual al seu transform
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				for (int k = 0; k < 3; k++) {

					int a = (int)Rubik [i, j, k].transform.localPosition.x;
					int b = (int)Rubik [i, j, k].transform.localPosition.y;
					int c = (int)Rubik [i, j, k].transform.localPosition.z;
					if (i != a+1 || j != b+1 || k != c+1){

						Rubik_copia[a+1, b+1, c+1] = Rubik[i, j, k];
					}

				}
			}
		}


	}
	// Rectificador del Transform, degut a que quan es fan rotacions hi ha petites variacions de posicio, rotacio
	// i escala que ens poden portar problemes. D'aquesta manera es passa qualsevol valor a un nombre enter.
	void rectificador (){




		for (int i = 0; i < 3; i++) {
			for(int j = 0; j < 3; j++){
				for(int k = 0; k < 3; k++){
					//
					Vector3 pos = Rubik[i, j, k].transform.localPosition;
					Rubik[i, j, k].transform.localPosition = new Vector3(rect (pos.x),
					                                                     rect (pos.y),
					                                                     rect (pos.z)
					                                                     );
					
					Vector3 rot = Rubik[i, j, k].transform.rotation.eulerAngles;
					rot = new Vector3(rect_rot(rot.x), rect_rot (rot.y), rect_rot (rot.z));
					Rubik [i, j, k].transform.rotation = Quaternion.Euler (rot);
				}
			}
		}


		
	}

	void Start(){
		RubikCube ();
		Rubik_original = Rubik;
		cube.SetActive (false);
	}

	void Update () {
		
		//Obtencio de l'objecte que esta clicant el Ratoli
		if (Input.GetMouseButtonDown (1)) {
			vpos = Input.mousePosition;
			ray = Camera.main.ScreenPointToRay (vpos);
			if (Physics.Raycast (ray, out hit)) {
				target = hit.collider.gameObject;
				
				Debug.Log (target.name);
				Debug.Log ("Rotacio: " + target.transform.rotation.ToString () +
					" Posicio: " + target.transform.localPosition.ToString ());
			}
		}

		if (Input.GetKeyDown (KeyCode.W) && !rotating) {
			//determinar direccio W
			dir = 'X';
			//index del pla en funcio del target
			p_index = (int)target.transform.position.z;
			//core en funcio de l'index i de la direccio
			core = get_core (p_index, dir);
			get_plane (core, dir, p_index);
			rotating = true;
		}else if (Input.GetKeyDown (KeyCode.S) && !rotating) {
			//determinar direccio S
			dir = 'x';
			//index del pla en funcio del target
			p_index = (int)target.transform.position.z;
			//core en funcio de l'index i de la direccio
			core = get_core (p_index, dir);
			get_plane (core, dir, p_index);
			rotating = true;
		}else if (Input.GetKeyDown (KeyCode.D) && !rotating) {
			//determinar direccio D
			dir = 'Y';
			//index del pla en funcio del target
			p_index = (int)target.transform.position.y;
			//core en funcio de l'index i de la direccio
			core = get_core (p_index, dir);
			get_plane (core, dir, p_index);
			rotating = true;
		}else if (Input.GetKeyDown (KeyCode.A) && !rotating) {
			//determinar direccio A
			dir = 'y';
			//index del pla en funcio del target
			p_index = (int)target.transform.position.y;
			//core en funcio de l'index i de la direccio
			core = get_core (p_index, dir);
			get_plane (core, dir, p_index);
			rotating = true;
		}else if(rotating){
			if(dir == 'X'){
				float x = 90, y = 0;
				rotate(core, x, y);
			}else if (dir == 'x'){
				float x = -90, y = 0;
				rotate(core, x, y);
			}else if(dir == 'Y'){
				float x = 0, y = -90;
				rotate(core, x, y);
			}else if(dir == 'y'){
				float x = 0, y = 90;
				rotate(core, x, y);
			}
			f++;

			//aquesta condicio ens permet saber en quin moment ha acabat la rotacio.
			if (f == 40) {

				rotating = false;
				f = 0;

				//amb aquesta accio tots els Cubs passen altre cop al parent del Rubik.
				desfer_plane(core, dir);
				//rectifica valors de posicio i rotacio
				rectificador();
				//Reorganitza el Rubik, perq coincideixi cada index am la seva posicio
				Rubik_rect ();
				target = null;
				core = null;
				dir = 'o';
			



			}
		}










	}


}
