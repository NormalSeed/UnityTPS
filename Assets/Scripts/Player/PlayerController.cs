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
