using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStats : MonoBehaviour
{
    [Header("UI")]
    public Image PlayerAvatar;
    public Slider HealthBar;
    public Text Kills;
    [Header("Player")]
    private int health;
    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            if (value > maxHealth) health = maxHealth;
        }
    }

    // Maybe we have a health powerup that increases max health? 
    private int maxHealth = 100;
    public int MaxHealth
    {
        get { return maxHealth; }
        set
        {
            maxHealth = value;
            Health = value;
        }
    }

    private void Awake()
    {
        health = maxHealth;
    }
    // This is purely for making everyone lose health to test ui elements, will be removed in the future 
    void OnEnable()
    {
        GameController.Instance.Ticked += HealthNegationTest;
    }
    // This is purely for making everyone lose health to test ui elements, will be removed in the future }

    void OnDisable()
    {
        GameController.Instance.Ticked -= HealthNegationTest;
    }
    // This is purely for making everyone lose health to test ui elements, will be removed in the future }

    private void HealthNegationTest()
    {
      //  Debug.Log(health);
        health -= 5;
    }
    
    void Update()
    {// Doesn't update from property?
        if (HealthBar.value != health) HealthBar.value = health;
    }
}
