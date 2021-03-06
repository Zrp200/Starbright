﻿using UnityEngine;
using System.Collections;

public class TouchControl : MonoBehaviour {

	public float swypeSpeed = 10f;

	GameObject pc;
	GameObject camera;
	Body target;
	// Use this for initialization
	void Start () {
		pc = GameObject.Find ("PC");
		camera = GameObject.Find ("Main Camera");
	}
	
	// Update is called once per frame

	void Update () {
		TapSelect(); 
	}
	
	void TapSelect() {
		float count = 0;

		foreach (Touch touch in Input.touches) {


			//if swyped
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
				
				// Get movement of the finger since last frame
				Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
				
				// Move camera by swipe
				camera.GetComponent<CameraBehavior>().Scroll(new Vector3(-touchDeltaPosition.x * swypeSpeed,
				                                             -touchDeltaPosition.y * swypeSpeed, 0));
			}

			if (touch.phase == TouchPhase.Began) {
				Ray ray = Camera.main.ScreenPointToRay(touch.position);
				RaycastHit hit ;
				
				//if touch planet object
				if (Physics.Raycast (ray, out hit)) {
					
					target = hit.transform.GetComponent<Body>();
					pc.BroadcastMessage("Orbit", target);
					
				}
			}

			//when stop touching
			if(touch.phase == TouchPhase.Ended){
				if(target != null && pc.GetComponent<PlayerCharacter>().IsOrbiting())
				{
					pc.BroadcastMessage("StopOrbit", target);
				}
				camera.GetComponent<CameraBehavior>().StopScroll();
			}

			count++;

		}
	

		//four fingers quits
		if (count >= 4)
			Application.Quit ();
		//three fingers restarts
		if (count >= 3)
			Application.LoadLevel (Application.loadedLevel);


	}
}
