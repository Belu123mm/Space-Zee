using System.Collections.Generic;
using FSM;
using UnityEngine;

public class ChaseState : MonoBaseState {

    public float speed = 2f;
    
    public float rangeDistance = 5;
    public float meleeDistance = 1.5f;
    
    private Model _player;
    Boss boss;

    public ChaseState(Model player, Boss _boss)
    {
        _player = player;
    }
    
    public override void UpdateLoop() {
        var dir = (_player.transform.position - boss.transform.position).normalized;

        boss.transform.position += dir * (speed * Time.deltaTime);
    }

    public override IState ProcessInput() {
        var sqrDistance = (_player.transform.position - boss.transform.position).sqrMagnitude;
        
        if (sqrDistance < meleeDistance * meleeDistance && Transitions.ContainsKey("OnMeleeAttackState")) {
            return Transitions["OnMeleeAttackState"];
        }

        return this;
    }
}