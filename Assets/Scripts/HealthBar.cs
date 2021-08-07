using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image HealthBarImage;
    public float CurrentHealth;
    private float MaxHealth = 100f;
    public PlayerController playerController;
    

    // Start is called before the first frame update
    void Start()
    {
        HealthBarImage = GetComponent<Image>();
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentHealth = playerController.health;
        HealthBarImage.fillAmount = CurrentHealth / MaxHealth;
    }
}
