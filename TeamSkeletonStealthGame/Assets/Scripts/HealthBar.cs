using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Player p1;
    public Image bar; // Health bar png
    public Image fill;
    public Text hpText; // Text displays "hp / 100"

    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            p1 = player.GetComponent<Player>();
        }
        else
        {
            Debug.LogError("Player object with tag 'Player' not found in the scene.");
        }
        
    }

    void Start()
    {
        UpdateHealthBar(); 

    }

    void Update()
    {
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        float fillAmount = Mathf.Clamp01(p1.getHP() / 100f); 
        fill.fillAmount = fillAmount; // Update fill

        if (hpText != null)
        {
            hpText.text = $"{p1.getHP()}"; // Update hp text
        }
    }

}
