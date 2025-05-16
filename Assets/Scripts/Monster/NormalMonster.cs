using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPattern;
using UnityEngine.AI;

public class NormalMonster : Monster, IDamagable
{
    private bool _isActivateControll = true;
    private bool _canTracking; // ObservableProperty로 설정해줘서 이벤트로 관리 가능함.

    [SerializeField] private float detectionRadius;

    [SerializeField] private int MaxHp;
    private ObservableProperty<int> CurrentHp = new();
    private ObservableProperty<bool> IsMoving = new();
    private ObservableProperty<bool> IsAttacking = new();

    private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _targetTransform;

    private void Awake() => Init();

    private void Update() => HandleControll();

    private void Init()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void HandleControll()
    {
        if (!_isActivateControll) return;
        HandleMove();
        DetectPlayer();
    }

    private void HandleMove()
    {
        if (_targetTransform == null) return;

        if (_canTracking)
        {
            // 정지 = true인지? 만약 true라면 false로 바꿈.
            _navMeshAgent.SetDestination(_targetTransform.position);
            IsMoving.Value = true;
        }
        else
        {
            // 정지 = true인지? 만약false라면 true로 바꿈.
            IsMoving.Value = false;
        }

        _navMeshAgent.isStopped = !_canTracking;
        IsMoving.Value = _canTracking;
    }

    private void DetectPlayer()
    {
        int targetLayerMask = LayerMask.GetMask("Player");
        if (Physics.OverlapSphere(transform.position, detectionRadius, targetLayerMask).Length > 0)
        {
            _canTracking = true;
        }
        else
        {
            _canTracking = false;
        }
    }

    public void TakeDamage()
    {
        // 데미지 판정 구현
        // 체력 깎고
        // 체력이 0 이하가 되면 사망 처리함.
    }
}
