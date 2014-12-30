﻿using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour {

	public static PlayerCharacter instance;

	private bool isOrbiting;
	private bool isColliding;
	private Body body; //body that it's orbiting
	private bool gameOver;

	public bool inBounds = true;
	bool flashed = false;

	private Vector3 lastPosition;
	private Vector3 deltaPosition;
	GameObject camera;
	GameObject backgroundCamera;

	FlashBehavior flash;

	public float Mass 
	{
		get { return BodyComponent.Mass; }
		set { BodyComponent.Mass = value; }
	}

	public Body BodyComponent
	{
		get { return GetComponent<Body>(); }
	}

	public bool IsOrbiting() 
	{
		return isOrbiting;
	}

	public bool IsColliding() 
	{
		return isColliding;
	}

	public Vector3 getDeltaPosition()
	{
		return deltaPosition;
	}

	public void setOrbiting(bool o)
	{
		isOrbiting = o;
	}
	
	void Start () 
	{
		instance = this;
		isOrbiting = false;
		isColliding = true;

		lastPosition = transform.position;
		camera = GameObject.Find ("Main Camera");
		backgroundCamera = GameObject.Find ("Background Planets Camera");

		flash = GameObject.Find ("Flash").GetComponent<FlashBehavior>();

		DontDestroyOnLoad (this);
	}

	void Update () 
	{
		BodyComponent.UpdateBody ();

		if(!gameOver)
		{
			if (isOrbiting) {
				BodyComponent.Gravitiate (body);
			}

			if(isColliding) {
				isColliding = !isColliding;
			}

			deltaPosition = transform.position - lastPosition;

			if(Input.GetKeyDown(KeyCode.Q))
			{
				GameOver ();
			}
		}
		else
		{
			if(flash.blackFinished)
			{
				Application.LoadLevel("FinalScore");
			}
		}
	}

	public void Orbit (Body b) {
		if (b != BodyComponent) {
			isOrbiting = true;
			body = b;
		}
	}

	public void StopOrbit() {
		isOrbiting = false;
		camera.GetComponent<CameraBehavior> ().CameraReturn (this);
	}

	public Body getOrbiting() {
		return body;
	}

	void OnCollisionEnter2D(Collision2D c) {
		if(!isColliding)
		{
			BodyComponent.Hit(c.gameObject.GetComponent<Body> ());
			isColliding = true;
		}
	}

	public void GameOver()
	{
		flash.blackScreen();
		gameOver = true;

		GetComponent<CircleCollider2D>().enabled = false;
	}
}
