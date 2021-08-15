using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource sfxPlayer;
    private AudioSource bgmPlayer;

    private int prim;
    private int sec;
    private int ult;
    [SerializeField] public AudioClip bgm;
    [SerializeField] public AudioClip[] primaryWeapon;
    [SerializeField] public AudioClip[] secondaryWeapon;
    [SerializeField] public AudioClip[] ultimateWeapon;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake(){
        sfxPlayer = this.gameObject.AddComponent<AudioSource>();
        bgmPlayer = this.gameObject.AddComponent<AudioSource>();

        bgmPlayer.clip = bgm;
        bgmPlayer.volume = 0.2f;
        bgmPlayer.loop = true;
        bgmPlayer.Play();
    }

    public void OnPrimaryEquipped(Events.PrimaryWeapon weapon){
        prim = (int) weapon;
    }

    public void OnPrimaryFired(){
        sfxPlayer.PlayOneShot(primaryWeapon[prim], 1f);
    }

    public void OnSecondaryEquipped(Events.SecondaryWeapon weapon){
        sec = (int) weapon;
    }
    public void OnSecondaryFired(){
        sfxPlayer.PlayOneShot(secondaryWeapon[sec], 1f);
    }

    public void OnUltimateEquipped(Events.UltimateWeapon weapon){
        ult = (int) weapon;
    }
    public void OnUltimateFired(){
        sfxPlayer.PlayOneShot(ultimateWeapon[ult], 1f);
    }
}
