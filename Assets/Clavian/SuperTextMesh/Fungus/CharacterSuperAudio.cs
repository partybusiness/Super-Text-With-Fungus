using UnityEngine;
using System.Collections;

//Stores audio parameters so you can change them when a character speaks
public class CharacterSuperAudio : MonoBehaviour {
    
    [Tooltip("Default sound to be read by the above audio source. Can be left null to make no sound by default.")]
    public AudioClip[] audioClips;
    [Tooltip("Should a new letter's sound stop a previous one and play, or let the old one keep playing?")]
    public bool stopPreviousSound = true;
    [Tooltip("Minimum pitch for perlin noise. If same or greater than max pitch, this will be the pitch.")]
    [Range(0f, 3f)]
    public float minPitch = 0.9f;
    [Tooltip("Maximum pitch for perlin noise.")]
    [Range(0f, 3f)]
    public float maxPitch = 1.2f;

}
