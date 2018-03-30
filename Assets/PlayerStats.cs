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
    // Use this for initialization
    void OnEnable()
    {
        GameController.Instance.Ticked += HealthNegationTest;
    }
    void OnDisable()
    {
        GameController.Instance.Ticked -= HealthNegationTest;
    }

    private void HealthNegationTest()
    {
        Debug.Log(health);
        health -= 5;
    }

    private void Awake()
    {
        health = maxHealth;
    }
    // Update is called once per frame
    void Update()
    {// Doesn't update from property?
        if (HealthBar.value != health) HealthBar.value = health;
    }
}
