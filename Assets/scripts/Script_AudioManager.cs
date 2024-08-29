using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource music;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void onMusic()
    {
        music.Play();
    }

    public void offMusic()
    {
        music.Stop(); 
    }
}
