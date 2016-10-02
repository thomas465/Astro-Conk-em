using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    public static PlayerHealth healthSingleton;

	public Image m_hurtFlash;
    
	public int m_startingPlayerHealth = 100;
	public int m_currentPlayerHealth;
	private bool m_playerDamaged = false;

    public Image fill, hurtFill;
    public Transform healthBarParent;

	void Start () 
	{
        healthSingleton = this;
        m_currentPlayerHealth = m_startingPlayerHealth;
        fill.fillAmount = 1;
        //reference to a player movement script
	}

	void Update()
	{
		//if (m_playerDamaged == true) 
		//{
			
            //possibly not quite opaque
		//} 
		//else 
		//{
			m_hurtFlash.color = Color.Lerp (m_hurtFlash.color, Color.clear, 5 * Time.deltaTime);
        //}

        hurtFill.fillAmount = Mathf.Lerp(hurtFill.fillAmount, fill.fillAmount, 2 * Time.deltaTime);

        healthBarParent.transform.localScale = Vector3.Lerp(healthBarParent.transform.localScale, Vector3.one * 0.7617157f, 10 * Time.deltaTime);
	}

    public void TakeDamage(int amount)
	{
        healthBarParent.transform.localScale = Vector3.one * 1.25f;
        m_hurtFlash.color = new Color(1f, 0f, 0f, 0.75f);
        //to be called in another script
        m_startingPlayerHealth -= amount;
		m_playerDamaged = true;


        fill.fillAmount = (float)m_startingPlayerHealth / 100;

		if (m_startingPlayerHealth <= 0)
		{
            Dead();
        }

    }

    void Dead()
	{
        //PlayerControls.enabled = false;
        //this will link to the player movement script and deactivate controls
        PlayerScript.playerSingleton.Die();
    }





}
