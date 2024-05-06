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
    [SerializeField] AudioClip Music_Welcome; // Start Game Music
    [SerializeField] AudioClip Music_GameOver; // Game over Music
    [SerializeField] AudioClip Music_Deloop; // Main Basic Loop (if it's not too annoying, just play it in endless loop)
    [SerializeField] AudioClip Music_Victory; // Victory
    [SerializeField] AudioClip Music_Marichis_1; // Bonus Music, might be fitting for after game over music, or just in the main menu)
    [SerializeField] AudioClip Music_Marichis_2; // Bonus Music, might be fitting for after game Victory music, or just in the main)
    //
    [SerializeField] AudioClip CollectedGem; // when gem was collected
    [SerializeField] AudioClip PlayerWalkingSound; // sound of when player is walking
    [SerializeField] AudioClip JumpSound; // jump sound
    [SerializeField] AudioClip fullYell; // Bonus SFX, combanition of two yells
    [SerializeField] AudioClip Yell_P1; // Bonus SFX, First part of the two yells
    [SerializeField] AudioClip Yell_P2; // Bonus SFX, Second Part of the two yells
    //
    [SerializeField] AudioClip Basic_GotHitSound; // When The player got hit, can be environment damage, or ranged damaged 
    [SerializeField] AudioClip Melee_GotHitSound; // when the player Get hits by a melee.
    //
    [SerializeField] AudioClip SpinAttack_Hit; // When the player hits something with the Spin Attack
    [SerializeField] AudioClip SpinAttack_Miss; // When the player misses with the Spin Attack
    [SerializeField] AudioClip Dash_Hit; // When the player hits something with the dash Attack
    [SerializeField] AudioClip Dash_Miss; //When the player hits something with the Spin Attack
    //
    [SerializeField] AudioClip SmackHitSound; // Bonus SFX, Basic Hit Sound of Smack.
    [SerializeField] AudioClip Enemy_RangedMiss; // When the enemy misses the player with the with Ranged Attack



    // Update is called once per frame
    void Update()
    {
  
        if (controller.IsJumping)
        { PlayerAudioSource.clip = JumpSound; PlayerAudioSource.Play(); }

      
        
    }
}