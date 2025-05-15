using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPattern;

public class AudioManager : MonoBehaviour
{
    private AudioSource _bgmSource;


    private void Awake() => Init();

    private void Init()
    {
        _bgmSource.GetComponent<AudioSource>();

    }

    public void BgmPlay()
    {
        _bgmSource.Play();
    }
}
