using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public bool IsControllActivate { get; set; } = true;

    private PlayerStatus _status;
    private PlayerMovement _movement;
    private Animator _animator;

    [SerializeField] private CinemachineVirtualCamera _aimCamera;
    [SerializeField] Gun _gun;

    [SerializeField] private KeyCode _aimKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode _shootKey = KeyCode.Mouse0;

    private void Awake() => Init();
    private void OnEnable() => SubscribeEvents();
    private void Update() => HandlePlayerControll();
    private void OnDisable() => UnsubscribeEvents();
    private void Init()
    {
        _status = GetComponent<PlayerStatus>();
        _movement = GetComponent<PlayerMovement>();
        _animator = GetComponent<Animator>();
    }

    private void HandlePlayerControll()
    {
        if (!IsControllActivate) return;

        HandleMovement();
        HandleAiming();
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (_status.IsAiming.Value && Input.GetKey(_shootKey))
        {
            _status.IsAttacking.Value = _gun.Shoot();
        }
        else
        {
            _status.IsAttacking.Value = false;
        }
    }

    private void HandleMovement()
    {
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

        // SetAnimation Parameter
        if (_status.IsAiming.Value)
        {
            Vector3 input = _movement.GetInputDirection();
            _animator.SetFloat("X", input.x);
            _animator.SetFloat("Z", input.z);
        }
    }

    private void HandleAiming()
    {
        _status.IsAiming.Value = Input.GetKey(_aimKey);
    }   
    
    public void SubscribeEvents()
    {
        _status.IsMoving.Subscribe(SetMoveAnimation);

        _status.IsAiming.Subscribe(_aimCamera.gameObject.SetActive);
        _status.IsAiming.Subscribe(SetAimAnimation);

        _status.IsAttacking.Subscribe(SetAttackAnimation);
    }

    public void UnsubscribeEvents()
    {
        _status.IsMoving.Unsubscribe(SetMoveAnimation);

        _status.IsAiming.Unsubscribe(_aimCamera.gameObject.SetActive);
        _status.IsAiming.Unsubscribe(SetAimAnimation);

        _status.IsAiming.Unsubscribe(SetAttackAnimation);
    }

    private void SetAimAnimation(bool value) => _animator.SetBool("IsAim", value);
    private void SetMoveAnimation(bool value) => _animator.SetBool("IsMove", value);

    private void SetAttackAnimation(bool value) => _animator.SetBool("IsAttack", value);
}
