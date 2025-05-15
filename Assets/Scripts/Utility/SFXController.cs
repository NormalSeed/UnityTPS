using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPattern;

public class SFXController : PooledObject
{
    private AudioSource _audioSource;
    private float _currentCount;


    private void Awake() => Init();
    private void Init()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // �� �������� ���ŵ� �� ������ �ð� = DeltaTime
        _currentCount -= Time.deltaTime;

        if (_currentCount <= 0 )
        {
            _audioSource.Stop();
            _audioSource.clip = null;
            ReturnPool();
        }
    }

    public void Play(AudioClip clip, bool loop = false, bool onAwake = false)
    {
        _audioSource.loop = loop;
        _audioSource.playOnAwake = onAwake;
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();

        _currentCount = clip.length;
    }
}
