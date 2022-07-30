using System;
using FSM;

public class PatrolState : MonoBaseState {

    private Model _player;
    Boss boss;
    
    public PatrolState(Boss _boss, Model player)
    {
        boss = _boss;
        _player = player;
    }
   
    
    public override void UpdateLoop() {
        //TODO: patrullo
    }

    public override IState ProcessInput() {
        var sqrDistance = (_player.transform.position - boss.transform.position).sqrMagnitude;

        if (sqrDistance < 100f) {
            return Transitions["OnChaseState"];
        }

        return this;
    }
}
