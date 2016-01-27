﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IAttackable {

	public int health;
	public int maxHealth;
	public int magic;
	public int maxMagic;
	public static Player instance;
	public static bool turnAvailable;

	// Use this for initialization
	void Start () {
		health = maxHealth;
		magic = maxMagic;
		instance = this;
		turnAvailable = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string Name(){
		return("playur");
	}

	void OnMouseDown() {
		GameObject.Find ("Combat").GetComponent<Canvas>().enabled = true;
		GameObject.Find ("Combat").transform.Find ("CombatMenu").GetComponent<CombatMenu> ().target = gameObject;
		Transform menuTransform = CombatMenu.instance.transform;
		menuTransform.position = Camera.main.WorldToScreenPoint (transform.position);
	}

	public void ReceiveHit(int damage, DamageTypes damageType){
		health -= damage;
	}

	public void DestroyMe(){

	}


	public void Attack(){
		if (turnAvailable && !GameController.frozen) {
			SpeechBubble.mainBubble.Activate ();
			int damage = Random.Range (1, 10);
			IAttackable attackable = Target().GetComponent(typeof(IAttackable)) as IAttackable;

			SpeechBubble.AddMessage ("you attack!");
			SpeechBubble.AddEvent (attackable, damage, DamageTypes.Physical);
			SpeechBubble.AddMessage (attackable.Name() + " sustains " + damage + " damage", true);
			if(BattleController.inCombat) turnAvailable = false;
		}
	}

	public void Heal(){
		if (turnAvailable && !GameController.frozen && Player.instance.magic > 0) {
			Player.instance.magic -= 1;
			SpeechBubble.mainBubble.Activate ();
			int damage = Random.Range (10, 20);
			IAttackable attackable = Target().GetComponent(typeof(IAttackable)) as IAttackable;

			SpeechBubble.AddEvent (attackable, -damage, DamageTypes.Physical);
			SpeechBubble.AddMessage ("you heal " + attackable.Name() +  " for " + damage + "!", true);
			if(BattleController.inCombat) turnAvailable = false;
		}
	}

	public void Fire(){
		if (turnAvailable && !GameController.frozen && Player.instance.magic > 0) {
			Player.instance.magic -= 1;
			SpeechBubble.mainBubble.Activate ();
			int damage = Random.Range (10, 20);
			SpeechBubble.AddMessage ("you cast fire!");
			IAttackable attackable = Target().GetComponent(typeof(IAttackable)) as IAttackable;

			SpeechBubble.AddEvent (attackable, damage, DamageTypes.Fire);
			SpeechBubble.AddMessage (attackable.Name() + " sustains " + damage + " damage", true);
			if(BattleController.inCombat) turnAvailable = false;
		}
	}

	public int Health(){
		return(health);
	}

	public GameObject Target(){
		return(GameObject.Find ("Combat").transform.Find ("CombatMenu").GetComponent<CombatMenu> ().target);
	}
}
