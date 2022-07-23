using System.Collections;
using System.Collections.Generic;
using FSM;
using System.Linq;
using UnityEngine;

public class BossGoap : MonoBehaviour
{

    public string stateToShow;

    //Nuevos, en carpeta Scripts > Boss > FSM > New
    public AttackState attackState; //Dispara al jugador con rafagas de balas.
    public LaserAttackState laserAttackState; //El boss activamente guarda energia para luego atacar con un laser.
    public PowerDownState powerDownState; //Luego de usar el ataque laser, el boss se apaga por unos segundos.
    public ChargeState chargeState; //El boss se lanza hacia el player, en caso de colicionar le causa daño.
    public InvokeWaveState invokeWaveState; //El boss invoca una orda de enemigos, activa un escudo y es invulnerable hasta que todos los enemigos de la wave son eliminados.
    public PushPlayerState pushPlayerState; //Si el player se aproxima mucho al boss, este activa un escudo momentanea mente que daña al player y lo empuja hacía atrás.

    private FiniteStateMachine _fsm;

    private float _lastReplanTime;
    private float _replanRate = .5f;
    
    public bool useCoroutine;


    void Start()
    {
        attackState.OnNeedsReplan += OnReplan;
        laserAttackState.OnNeedsReplan += OnReplan;
        powerDownState.OnNeedsReplan += OnReplan;
        chargeState.OnNeedsReplan += OnReplan;
        invokeWaveState.OnNeedsReplan += OnReplan;
        pushPlayerState.OnNeedsReplan += OnReplan;

        //OnlyPlan();
        PlanAndExecute();
    }

    private void Update()
    {
        if (_fsm != null)
            stateToShow = _fsm.currentStateDebug;
    }

    private void OnlyPlan()
    {
        /* var actions = new List<GOAPAction>{
                                               new GOAPAction("Attack")
                                                  .Effect("isPlayerInSight", true),

                                               new GOAPAction("Invoke")
                                                  .Pre("BossHPcritial", true),

                                               new GOAPAction("Charge")
                                                  .Pre("isPlayerPoweredUp",   true)
                                                  .Pre("isPlayerClose", false)
                                                  .Effect("isPlayerClose", true),

                                               new GOAPAction("Push")
                                                   .Pre("isPlayerInSight", true)
                                                   .Pre("isPlayerClose", true),

                                               new GOAPAction("Laser Attack")
                                                   .Pre("BossPoweredUp", true)
                                                   .Effect("BossPoweredUp", false)
                                                   .Effect("BossPoweredDown", true),

                                               new GOAPAction("Powered Down")
                                                   .Pre("BossPoweredDown", true)

                                           };
         var from = new GOAPState();
         from.values["isPlayerInSight"] = false;
         from.values["isPlayerClose"] = false;
         from.values["BossHPcritial"] = false;
         from.values["BossPoweredUp"] = false;
         from.values["BossPoweredDown"] = false;
         from.values["isPlayerPoweredUp"] = false;
         from.values["isPlayerAlive"] = true;

         var to = new GOAPState();
         to.values["isPlayerAlive"] = false;

         var planner = new GoapPlanner();

         planner.Run(from, to, actions);*/
    }

    private void PlanAndExecute()
    {
        var actions = new List<GOAPAction>{
                                            new GOAPAction("Attack")                                            
                                                 .Pre("isPlayerAlive", true)
                                                 .Pre("isBossAngry", true)
                                                 .Effect("isPlayerInSight", true)       
                                                 .Effect("isBossAngry", false)
                                                 .LinkedState(attackState)                                                 
                                                 .Cost(2),

                                             new GOAPAction("Charge")
                                                 .Pre("isPlayerAlive", true)
                                                 .Pre("isPlayerClose", false)
                                                 .Effect("isPlayerClose", true)
                                                 .LinkedState(chargeState)
                                                 .Cost(2),

                                             new GOAPAction("Push")
                                                 .Pre("isPlayerAlive", true)
                                                 .Pre("isPlayerClose", true)
                                                 .Effect("isPlayerClose", false)
                                                 .Effect("BossPoweredUp", true)
                                                 .LinkedState(pushPlayerState)
                                                 .Cost(1),

                                             new GOAPAction("Lazer")
                                                 .Pre("LowEnergyBoss", false)
                                                 .Pre("isPlayerAlive", true)
                                                 .Pre("isPlayerClose", false)
                                                 .Pre("BossPoweredUp", true)
                                                 .Pre("LowHPBoss", false)
                                                 .Effect("BossPoweredUp", false)
                                                 .Effect("isPlayerAlive", false)
                                                 .Cost(5)
                                                 .LinkedState(laserAttackState),

                                             new GOAPAction("Invoke")
                                                 .Pre("LowEnergyBoss", false)
                                                 .Pre("isPlayerAlive", true)
                                                 .Pre("LowHPBoss", true)
                                                 .Pre("isBossAngry", false)
                                                 .Effect("LowHPBoss", false)
                                                 .Effect("isPlayerClose", false)
                                                 .Effect("BossPoweredUp", false)
                                                 .Cost(2)
                                                 .LinkedState(invokeWaveState),

                                             new GOAPAction("PowerDown")
                                                 .Pre("isPlayerAlive", true)
                                                 .Pre("LowEnergyBoss", true)
                                                 .Effect("LowEnergyBoss", false)
                                                 .LinkedState(powerDownState),
        };

        var planner = new GoapPlanner();

        var plan = planner.Run(BossWorldState.instance.GetWorldState(), BossWorldState.instance.GetObjectiveState(), actions, useCoroutine, this);

        ConfigureFsm(plan);
    }

    private void OnReplan()
    {
        if (Time.time >= _lastReplanTime + _replanRate)
        {
            _lastReplanTime = Time.time;
        }
        else
        {
            return;
        }

        var actions = new List<GOAPAction>{
                                            new GOAPAction("Attack")
                                                 .Pre("isPlayerAlive", true)
                                                 .Pre("isBossAngry", true)
                                                 .Effect("isPlayerInSight", true)
                                                 .Effect("isBossAngry", false)
                                                 .LinkedState(attackState)
                                                 .Cost(2),

                                             new GOAPAction("Charge")
                                                 .Pre("isPlayerAlive", true)
                                                 .Pre("isPlayerClose", false)
                                                 .Effect("isPlayerClose", true)
                                                 .LinkedState(chargeState)
                                                 .Cost(2),

                                             new GOAPAction("Push")
                                                 .Pre("isPlayerAlive", true)
                                                 .Pre("isPlayerClose", true)
                                                 .Effect("isPlayerClose", false)
                                                 .Effect("BossPoweredUp", true)
                                                 .LinkedState(pushPlayerState)
                                                 .Cost(1),

                                             new GOAPAction("Lazer")
                                                 .Pre("LowEnergyBoss", false)
                                                 .Pre("isPlayerAlive", true)
                                                 .Pre("isPlayerClose", false)
                                                 .Pre("BossPoweredUp", true)
                                                 .Pre("LowHPBoss", false)
                                                 .Effect("BossPoweredUp", false)
                                                 .Effect("isPlayerAlive", false)
                                                 .Cost(5)
                                                 .LinkedState(laserAttackState),

                                             new GOAPAction("Invoke")
                                                 .Pre("LowEnergyBoss", false)
                                                 .Pre("isPlayerAlive", true)
                                                 .Pre("LowHPBoss", true)
                                                 .Pre("isBossAngry", false)
                                                 .Effect("LowHPBoss", false)
                                                 .Effect("isPlayerClose", false)
                                                 .Effect("BossPoweredUp", false)
                                                 .Cost(2)
                                                 .LinkedState(invokeWaveState),

                                             new GOAPAction("PowerDown")
                                                 .Pre("isPlayerAlive", true)
                                                 .Pre("LowEnergyBoss", true)
                                                 .Effect("LowEnergyBoss", false)
                                                 .LinkedState(powerDownState),
                                          };

        var planner = new GoapPlanner();

        var plan = planner.Run(BossWorldState.instance.GetWorldState(), BossWorldState.instance.GetObjectiveState(), actions, useCoroutine, this);

        ConfigureFsm(plan);
    }

    private void ConfigureFsm(IEnumerable<GOAPAction> plan)
    {
        Debug.Log("Completed Plan");
        _fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }

}
