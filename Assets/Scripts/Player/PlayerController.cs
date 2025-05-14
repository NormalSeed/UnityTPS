using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public bool IsControllActive { get; set; } = true;

    private PlayerStatus _status;
    private PlayerMovement _movement;
    [SerializeField] private GameObject _aimCamera;
    private GameObject _mainCamera;

    [SerializeField] private KeyCode _aimKey = KeyCode.Mouse1;

    private void Awake() => Init();

    private void OnEnable() => SubscribeEvents();

    private void Update() => HandlePlayerControll();

    private void OnDisable() => UnsubscribeEvents();
    private void Init()
    {
        _status = GetComponent<PlayerStatus>();
        _movement = GetComponent<PlayerMovement>();
        _mainCamera = Camera.main.gameObject;

    }

    private void HandlePlayerControll()
    {
        if (!IsControllActive) return;

        HandleMovement();
        HandleAiming();
    }

    private void HandleMovement()
    {
        // (회전 수행 후)좌우 회전에 대한 벡터 반환
        Vector3 camRotateDir = _movement.SetAimRotation();

        float moveSpeed;
        if (_status.IsAiming.Value) moveSpeed = _status.WalkSpeed;
        else moveSpeed = _status.RunSpeed;

        Vector3 moveDir = _movement.SetMove(moveSpeed);
        _status.IsMoving.Value = (moveDir != Vector3.zero);

        Vector3 avatarDir;
        if (_status.IsAiming.Value) avatarDir = camRotateDir;
        else avatarDir = moveDir;

        _movement.SetAvatarRotation(avatarDir);
    }

    private void HandleAiming()
    {
        _status.IsAiming.Value = Input.GetKey(_aimKey);
    }   
    
    public void SubscribeEvents()
    {
        _status.IsAiming.Subscribe(value => SetActivateAimCamera(value));
    }

    public void UnsubscribeEvents()
    {
        _status.IsAiming.Unsubscribe(value => SetActivateAimCamera(value));
    }

    private void SetActivateAimCamera(bool value)
    {
        _aimCamera.SetActive(value);
        _mainCamera.SetActive(!value);
    }
}
