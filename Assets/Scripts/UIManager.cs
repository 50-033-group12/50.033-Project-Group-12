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

    [SerializeField] public Sprite[] primaryWeapons;
    [SerializeField] public Sprite[] secondaryWeapons;
    [SerializeField] public Sprite[] ultimateWeapons;

    private float lastKnownRatio = 1;

    void Start()
    {
        fuelBar = this.transform.GetChild(0).gameObject;
        healthBar = this.transform.GetChild(1).gameObject;
        primaryWeap = this.transform.GetChild(2).gameObject;
        ultimateWeap = this.transform.GetChild(3).gameObject;
        secondaryWeap = this.transform.GetChild(4).gameObject;
    }

    public void ChangePrimaryWeapon(Events.PrimaryWeapon weapon)
    {
        primaryWeap.transform.GetChild(0).GetComponent<Image>().sprite = primaryWeapons[(int)weapon];
    }

    public void ChangeSecondaryWeapon(Events.SecondaryWeapon weapon)
    {
        secondaryWeap.transform.GetChild(0).GetComponent<Image>().sprite = secondaryWeapons[(int)weapon];
    }

    public void ChangeUltimateWeapon(Events.UltimateWeapon weapon)
    {
        ultimateWeap.transform.GetChild(0).GetComponent<Image>().sprite = ultimateWeapons[(int)weapon];
    }

    public void ChangeFuel(float current, float total)
    {
        float currentFill = current / total;
        fuelBar.transform.GetChild(2).GetComponent<Image>().fillAmount = currentFill;
    }

    public void ChangeHealth(float current, float total)
    {
        float currentFill = current / total;
        bool isDecrease = currentFill < lastKnownRatio;
        lastKnownRatio = currentFill;
        if (isDecrease) shake();
        healthBar.transform.GetChild(2).GetComponent<Image>().fillAmount = currentFill;
    }

    void shake()
    {
        var rect = this.transform.GetComponent<RectTransform>();
        float shakeAmt = 1.2f; // the degrees to shake the camera
        float shakePeriodTime = 0.12f; // The period of each shake
        float dropOffTime = 1f; // How long it takes the shaking to settle down to nothing
        LTDescr shakeTween = LeanTween.rotateAroundLocal(rect, Vector3.forward, shakeAmt, shakePeriodTime)
            .setEase(LeanTweenType.easeShake) // this is a special ease that is good for shaking
            .setLoopClamp()
            .setRepeat(-1);
        
        // Slow the camera shake down to zero
        LeanTween.value(gameObject, shakeAmt, 0f, dropOffTime).setOnUpdate(
            (float val) => { shakeTween.setTo(Vector3.one * val); }
        ).setEase(LeanTweenType.easeOutQuad);
    }

    public void ChangeAmmo(int clip, int total)
    {
        primaryWeap.transform.GetChild(1).GetComponent<Text>().text = clip.ToString();
        primaryWeap.transform.GetChild(3).GetComponent<Text>().text = total.ToString();
    }

    // ticked primary/secondary/cooldown <X, Y> is expressed as percentage of X over Y
    public void TickedPrimaryReload(int x, int y)
    {
        primaryWeap.transform.GetChild(4).GetComponent<Image>().fillAmount = (1f - ((float)x / (float)y));
    }

    public void TickedUltimateCooldown(int x, int y)
    {
        ultimateWeap.transform.GetChild(1).GetComponent<Image>().fillAmount = (1f - ((float)x / (float)y));
    }

    public void TickedSecondaryCooldown(int x, int y)
    {
        secondaryWeap.transform.GetChild(1).GetComponent<Image>().fillAmount = (1f - ((float)x / (float)y));
    }
}