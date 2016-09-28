using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    public Slider slider;
    
    public int startingPlayerHealth = 100;
    public int currentPlayerHealth;

	void Start () {
        currentPlayerHealth = startingPlayerHealth;
        //reference to a player movement script
	}

    public void TakeDamage(int amount){
        //to be called in another script
        startingPlayerHealth -= amount;
        slider.value = currentPlayerHealth;

        if (currentPlayerHealth <= 0){
            //Dead();
        }

    }

    void Dead(){
        //Player.enabled = false;
        //this will link to the player movement script and deactivate controls
    }





}
