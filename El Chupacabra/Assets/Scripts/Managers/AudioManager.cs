using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private NewPlayerController controller;
    [SerializeField] AudioSource Music;
    [SerializeField] AudioSource Collectible;
    [SerializeField] AudioSource PlayerAudioSource;
    [SerializeField] AudioSource Enemey;
    //
    [SerializeField] AudioClip Music_Welcome;
    [SerializeField] AudioClip Music_GameOver;
    [SerializeField] AudioClip Music_Deloop;
    [SerializeField] AudioClip Music_Victory;
    [SerializeField] AudioClip Music_Marichis_1;
    [SerializeField] AudioClip Music_Marichis_2;
    //
    [SerializeField] AudioClip CollectedGem;
    [SerializeField] AudioClip PlayerWalkingSound;
    [SerializeField] AudioClip JumpSound;
    [SerializeField] AudioClip fullYell;
    [SerializeField] AudioClip Yell_P1;
    [SerializeField] AudioClip Yell_P2;
    //
    [SerializeField] AudioClip Basic_GotHitSound;
    [SerializeField] AudioClip Melee_GotHitSound;
    [SerializeField] AudioClip SpinAttack_Hit;
    [SerializeField] AudioClip SpinAttack_Miss;
    [SerializeField] AudioClip Dash_Hit;
    [SerializeField] AudioClip Dash_Miss;
    //
    [SerializeField] AudioClip SmackHitSound;
    [SerializeField] AudioClip Enemy_RangedMiss;

 

    // Update is called once per frame
    void Update()
    {
  
        if (controller.IsJumping)
        { PlayerAudioSource.clip = JumpSound; PlayerAudioSource.Play(); }

      
        
    }
}