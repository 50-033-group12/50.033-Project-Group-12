using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Events;

public class UIManager : MonoBehaviour
{
    public GameObject primaryWeap;
    public GameObject secondaryWeap;
    public GameObject ultimateWeap;
    public GameObject healthBar;
    public GameObject fuelBar;

    [SerializeField]
    public Sprite[] primaryWeapons;
    [SerializeField]
    public Sprite[] secondaryWeapons;
    [SerializeField]
    public Sprite[] ultimateWeapons;

    // Enums for weapons
    private PlayerEvents events;


    void Start(){
        fuelBar = this.transform.GetChild(0).gameObject;
        healthBar = this.transform.GetChild(1).gameObject;
        primaryWeap = this.transform.GetChild(2).gameObject;
        ultimateWeap = this.transform.GetChild(3).gameObject;
        secondaryWeap = this.transform.GetChild(4).gameObject;
    }

    public void ChangePrimaryWeapon(int weapon){
        primaryWeap.transform.GetChild(0).GetComponent<Image>().sprite = primaryWeapons[weapon];
    }

    public void ChangeSecondaryWeapon(int weapon){
        secondaryWeap.transform.GetChild(0).GetComponent<Image>().sprite = secondaryWeapons[weapon];
    }

    public void ChangeUltimateWeapon(int weapon){
        ultimateWeap.transform.GetChild(0).GetComponent<Image>().sprite = ultimateWeapons[weapon];
    }

    public void ChangeFuel(float current, float total){
        float currentFill = current/total;
        fuelBar.transform.GetChild(2).GetComponent<Image>().fillAmount = currentFill;
    }

    public void ChangeHealth(float current, float total){
        float currentFill = current/total;
        healthBar.transform.GetChild(2).GetComponent<Image>().fillAmount = currentFill;
    }

    public void ChangeAmmo(int clip, int total){
        primaryWeap.transform.GetChild(1).GetComponent<Text>().text = clip.ToString();
        primaryWeap.transform.GetChild(3).GetComponent<Text>().text = total.ToString();
    }

    // ticked primary/secondary/cooldown <X, Y> is expressed as percentage of X over Y
    public void TickedPrimaryReload(int x, int y){
        primaryWeap.transform.GetChild(4).GetComponent<Image>().fillAmount = (1f - ((float)x/(float)y));
    }

    public void TickedUltimateCooldown(int x, int y){
        ultimateWeap.transform.GetChild(1).GetComponent<Image>().fillAmount = (1f - ((float)x/(float)y));
    }

    public void TickedSecondaryCooldown(int x, int y){
        secondaryWeap.transform.GetChild(1).GetComponent<Image>().fillAmount = (1f - ((float)x/(float)y));
    }
}
