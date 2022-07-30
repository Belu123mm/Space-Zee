using System;
using FSM;
using UnityEngine;

public class MeleeAttackState : MonoBaseState {
    
    public override event Action OnNeedsReplan;

    public float attackRate = .5f;
    
    public float maxDistance = 1.5f;
    
    private Model _player;

    private float _lastAttackTime;

    Boss boss;
    public MeleeAttackState(Boss _boss, Model player)
    {
        boss = _boss;
        _player = player;
    }


    public override void UpdateLoop() {
        if (Time.time >= _lastAttackTime + attackRate) {
            _lastAttackTime = Time.time;
            Debug.Log("Ataco");
        }
        
        var sqrDistance = (_player.transform.position - boss.transform.position).sqrMagnitude;
        
        if (sqrDistance > maxDistance * maxDistance) {
            OnNeedsReplan?.Invoke();
        }
    }

    public override IState ProcessInput() {
        return this;
    }
}