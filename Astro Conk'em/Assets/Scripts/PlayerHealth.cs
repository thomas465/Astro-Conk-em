using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    public Slider m_slider;
	public Image m_hurtFlash;
    
	public int m_startingPlayerHealth = 100;
	public int m_currentPlayerHealth;
	private bool m_playerDamaged = false;

	void Start () 
	{
		m_currentPlayerHealth = m_startingPlayerHealth;
        //reference to a player movement script
	}

	void update()
	{
		if (m_playerDamaged == true) 
		{
			m_hurtFlash.color = new Color (1f, 0f, 0f, 1f);
		} 
		else 
		{
			m_hurtFlash.color = Color.Lerp (m_hurtFlash.color, Color.clear, 5 * Time.deltaTime);
		}
			
	}

    public void TakeDamage(int amount)
	{
        //to be called in another script
		m_startingPlayerHealth -= amount;
		m_playerDamaged = true;
		m_slider.value = m_currentPlayerHealth;

		if (m_currentPlayerHealth <= 0)
		{
            //Dead();
        }

    }

    void Dead()
	{
        //PlayerControls.enabled = false;
        //this will link to the player movement script and deactivate controls
    }





}
