using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rotacio : MonoBehaviour {

	public float speed = 10;
	public float lerpSpeed = 10;
	
	private float xDeg;
	private float yDeg;
	
	private Quaternion fromRotation;
	private Quaternion toRotation;

	public float i = 0;
	public Quaternion rot;

	void Start(){
		transform.rotation = Quaternion.Euler (i, 0, 0);
	}
	
	void Update () {
		
		if (Input.GetMouseButton (2)) {
			xDeg -= Input.GetAxis ("Mouse X") * speed;
			yDeg += Input.GetAxis ("Mouse Y") * speed;
			Debug.Log(xDeg.ToString());
			
			fromRotation = transform.rotation;
			toRotation = Quaternion.Euler (yDeg, xDeg, 0);
			transform.rotation = Quaternion.Lerp (fromRotation, toRotation, Time.deltaTime * lerpSpeed);
		}
		/*
		if(Input.GetKeyDown(KeyCode.UpArrow){
			rot = 
		}else if(Input.GetKeyDown(KeyCode.DownArrow){
			
		}else if(Input.GetKeyDown(KeyCode.LeftArrow){
			
		}else if(Input.GetKeyDown(KeyCode.RightArrow){
			
		}*/
	}
}
