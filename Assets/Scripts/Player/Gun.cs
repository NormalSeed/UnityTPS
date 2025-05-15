using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Cinemachine;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField][Range(0, 100)] private float _attackRange;
    [SerializeField] private int _shootDamage;
    [SerializeField] private float _shootDelay;
    [SerializeField] private AudioClip _shootSFX;

    private CinemachineImpulseSource _impulse;
    private Camera _camera;

    private bool _canShoot { get => _currentCount <= 0; }
    private float _currentCount;

    private void Awake() => Init();

    private void Update() => HandleCanShoot();

    private void Init()
    {
        _camera = Camera.main;
        _impulse = GetComponent<CinemachineImpulseSource>();
    }

    public bool Shoot()
    {
        if (!_canShoot) return false;

        PlayShootSound();
        PlayCameraEffect();
        PlayShootEffect();
        _currentCount = _shootDelay;

        // TODO: Ray �߻� -> ��ȯ���� ��󿡰� ������ �ο�. ���� ���� �� ���� ����

        GameObject target = RayShoot();
        if (target == null) return true;

        Debug.Log($"�ѿ� ���� : {target.name}");
        // ------

        return true;
    }

    private void HandleCanShoot()
    {
        // _canShoot������ �������ִ� �Լ�
        if (_canShoot) return;

        _currentCount -= Time.deltaTime;
    }

    private GameObject RayShoot()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _attackRange, _targetLayer))
        {
            // TODO: ���͸� ��� �����ϴ��Ŀ� ���� �ٸ�
            return hit.transform.gameObject;
        }
        return null;
    }

    private void PlayShootSound()
    {
        SFXController sfx = GameManager.Instance.Audio.GetSFX();
        sfx.Play(_shootSFX);
    }

    private void PlayCameraEffect()
    {
        _impulse.GenerateImpulse();
    }

    private void PlayShootEffect()
    {
        // TODO: �ѱ� ȭ�� ȿ�� ��ƼŬ�� �����ϱ�
    }
}
