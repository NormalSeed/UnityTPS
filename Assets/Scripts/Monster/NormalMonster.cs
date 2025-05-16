using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPattern;
using UnityEngine.AI;

public class NormalMonster : Monster, IDamagable
{
    private bool _isActivateControll = true;
    private bool _canTracking; // ObservableProperty�� �������༭ �̺�Ʈ�� ���� ������.

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
            // ���� = true����? ���� true��� false�� �ٲ�.
            _navMeshAgent.SetDestination(_targetTransform.position);
            IsMoving.Value = true;
        }
        else
        {
            // ���� = true����? ����false��� true�� �ٲ�.
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
        // ������ ���� ����
        // ü�� ���
        // ü���� 0 ���ϰ� �Ǹ� ��� ó����.
    }
}
