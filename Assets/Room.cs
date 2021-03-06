﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {
	public bool northDoor;
	public bool southDoor;
	public bool eastDoor;
	public bool westDoor;
	public string[] messages;
	public ArrayList enemies;
	public ArrayList entities;
	public ArrayList fieldObjects;
	public Matrix matrix;
	public int xCoordinate;
	public int yCoordinate;


	public void ShowEnemies(){
		foreach (GameObject enemy in enemies) {
			enemy.transform.Find ("Body").GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
			enemy.GetComponent<BoxCollider2D> ().enabled = true;
		}
	}

	public void HideEnemies(){
		foreach (GameObject enemy in enemies) {
			enemy.transform.Find ("Body").GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, .2f);
			enemy.GetComponent<BoxCollider2D> ().enabled = false;
		}
	}


	// Use this for initialization
	void Start () {
		gameObject.name = "Room " + Random.Range (0, 9999999).ToString ();
		entities = new ArrayList ();
		enemies = new ArrayList();
		fieldObjects = new ArrayList();
		float randomValue = Random.value;

		if (randomValue < .01f) {
			AddEnemy ();
			AddEnemy ();
			AddEnemy ();

			foreach (GameObject enemy in enemies) {
				enemy.transform.position = GameObject.Find("Bin").transform.position;
			}
		}

		foreach (GameObject spawnPoint in ItemSpawnPoint.spawnPoints) {
			if(Random.value < .1f) AddBomb (spawnPoint);
		}

		randomValue = Random.value;

		UpdateNeighbors ();
	}

	void AddBomb(GameObject spawnPoint){
		GameObject fieldObject = Instantiate (Resources.Load ("Bomb"), GameObject.Find("Bin").transform.position, Quaternion.identity) as GameObject;
		fieldObject.GetComponent<Bomb> ().room = this;
		fieldObject.GetComponent<Bomb> ().spawnPoint = spawnPoint;
//		fieldObject.transform.parent = transform.Find ("Entities");
		entities.Add (fieldObject);
		fieldObjects.Add (fieldObject);
	}

	void AddEnemy(){
		GameObject enemyObject = Instantiate (Resources.Load ("Corgi"), Vector3.zero, Quaternion.identity) as GameObject;
		enemyObject.GetComponent<Corgi> ().room = this;
//		enemyObject.transform.parent = transform.Find ("Entities");
		enemies.Add (enemyObject);
		entities.Add (enemyObject);
	}

	void UpdateNeighbors(){
		Room neighbor;
		if (northDoor) {
			neighbor = matrix.RoomAt (xCoordinate, yCoordinate + 1);
			if (neighbor != null) neighbor.southDoor = true;
		}

		if (southDoor) {
			neighbor = matrix.RoomAt (xCoordinate, yCoordinate - 1);
			if (neighbor != null) neighbor.northDoor = true;
		}

		if (eastDoor) {
			neighbor = matrix.RoomAt (xCoordinate + 1, yCoordinate);
			if (neighbor != null) neighbor.westDoor = true;
		}

		if (westDoor) {
			neighbor = matrix.RoomAt (xCoordinate - 1, yCoordinate);
			if (neighbor != null) neighbor.eastDoor = true;
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	public bool DoorAt(int x, int y){
		if (x == 1) {
			return(eastDoor);
		} else if (x == -1) {
			return(westDoor);
		} else if (y == 1) {
			return(northDoor);
		} else if (y == -1) {
			return(southDoor);
		}else{
			return(false);
		}
	}

	public void RemoveObject(GameObject fieldObject){
		fieldObjects.Remove (fieldObject);
		entities.Remove (fieldObject);
	}

	public void AddObject(GameObject fieldObject){
		fieldObjects.Add (fieldObject);
		entities.Add (fieldObject);
	}

	public void EnterRoom(){
		int i = 0;
		foreach (GameObject fieldObject in this.fieldObjects) {
//			float negFactor = 1;
//			if (i % 2 == 0) negFactor = -1;
//			float xPosition = Mathf.CeilToInt ((float)(i + 1) / 2) * negFactor * 1.5f;
//			fieldObject.transform.position = new Vector3 (xPosition, -1.75f, 0);
//			i++;
			fieldObject.transform.position = fieldObject.GetComponent<Bomb>().spawnPoint.transform.position;
		}

		i = 0;

		foreach (GameObject enemy in this.enemies) {
			float negFactor = 1;
			if (i % 2 == 0) negFactor = -1;
			float xPosition;
			if (this.enemies.Count % 2 == 0) {
				xPosition = Mathf.CeilToInt ((float)(i + 1) / 2) * negFactor * 1.5f;
			} else {
				xPosition = Mathf.CeilToInt ((float)i / 2) * negFactor * 1.5f;
				print ("using xPosition: " + xPosition);
			}
			enemy.transform.position = new Vector3(xPosition, -3, 0);
			i++;
		}
	}

	public void ExitRoom(){
		ShowEnemies ();

		foreach (GameObject fieldObject in this.entities) {
			fieldObject.transform.position = GameObject.Find("Bin").transform.position;
		}
	}

	public List<GameObject> AllEntities(){
		List<GameObject> list = new List<GameObject> ();

		list.Add (Player.instance.gameObject);
		foreach (GameObject enemy in enemies) {
			list.Add (enemy);
		}
		foreach (GameObject fieldObject in fieldObjects) {
			list.Add (fieldObject);
		}
		return(list);
	}
}
