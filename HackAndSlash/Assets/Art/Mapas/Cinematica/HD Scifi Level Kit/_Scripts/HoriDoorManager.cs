using UnityEngine;
using System.Collections;

public class HoriDoorManager : MonoBehaviour {

	public DoorHori door1;
	public DoorHori door2;
	bool canInteract = true;
	void OnTriggerEnter(){
		if (!canInteract) return;
		canInteract = false;

		if (door1!=null){
			door1.Close();	
		}

		if (door2!=null){
			door2.Close();	
		}

	}
}
