using System.Collections;
using System.Collections.Generic;
using FSM;
using System.Linq;
using UnityEngine;

public class BossGoap : MonoBehaviour
{
    public Model player;
    public Boss boss;

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

    [Header("AttackState")]

    public GameObject attackPosition;
    public List<Transform> canons;

    [Header("LaserAttackState")]
    public GameObject laserGoTo;
    public GameObject laserGuide;
    public GameObject redLazer;

    [Header("PowerDownState")]
    public GameObject objective;
    public GameObject batery;

    [Header("ChargeState")]
    public GameObject warning;
    public GameObject triggerCollider;

    [Header("InvokeWaveState")]
    public GameObject enemySpawner;
    public GameObject BossBase;
    public GameObject BossHide;

    //[Header("PushPlayerState")]

    void Start()
    {
        player = FindObjectOfType<Model>();
        attackState = new AttackState(boss, attackPosition, canons);
        laserAttackState = new LaserAttackState(boss, laserGoTo, laserGuide, redLazer);
        powerDownState = new PowerDownState(objective, batery, boss);
        chargeState = new ChargeState(boss, player, warning, triggerCollider);
        invokeWaveState = new InvokeWaveState(boss, enemySpawner, BossBase, BossHide);
        pushPlayerState = new PushPlayerState(boss, player);

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

    private void PlanAndExecute()
    {
        var actions = new List<GOAPAction>{
                                            new GOAPAction("Attack")
                                                 .Pre(x => (int)x[BossState.PlayerLife] >= 0)
                                                 .Pre(x => (BossMood)x[BossState.Mood] == BossMood.Calm)
                                                 .Effect(x => x[BossState.Overheating] = Mathf.Clamp((int)x[BossState.Overheating] + 1,0,20))
                                                 .Effect(x => x[BossState.PlayerLife] = Mathf.Clamp((int)x[BossState.PlayerLife] - 2,0,20))
                                                 .LinkedState(attackState)
                                                 .Cost(2),
                                                  //Check to add change playerlife here

                                             new GOAPAction("Charge")
                                                 .Pre(x => (int)x[BossState.PlayerLife] >= 0)
                                                 .Pre(x => (bool)x[BossState.PlayerClose] == false)
                                                 .Effect(x => x[BossState.PlayerClose] = true)
                                                 .Effect(x => x[BossState.PlayerLife] = Mathf.Clamp((int)x[BossState.PlayerLife] - 1,0,20))
                                                 .LinkedState(chargeState)
                                                 .Cost(2),

                                             new GOAPAction("Push")
                                                 .Pre(x => (int)x[BossState.PlayerLife] >= 0)
                                                 .Pre(x => (bool)x[BossState.PlayerClose] == true)
                                                 .Pre(x => (BossMood)x[BossState.Mood] != BossMood.PoweredUp)
                                                 .Effect(x => x[BossState.PlayerClose] = false)
                                                 .LinkedState(pushPlayerState)
                                                 .Cost(1),

                                             new GOAPAction("Lazer")
                                                 .Pre(x => (int)x[BossState.Overheating] <= 3)
                                                 .Pre(x => (int)x[BossState.PlayerLife] >= 0)
                                                 .Pre(x => (bool)x[BossState.PlayerClose] == false)
                                                 .Pre(x => (BossMood)x[BossState.Mood] == BossMood.PoweredUp)
                                                 .Pre(x => (float)x[BossState.Health] > 50)
                                                 .Effect(x => x[BossState.Mood] = BossMood.Angry)
                                                 .Effect(x => x[BossState.Overheating] = Mathf.Clamp((int)x[BossState.Overheating] + 1,0,20))
                                                 .Effect(x => x[BossState.PlayerLife] = Mathf.Clamp((int)x[BossState.PlayerLife] - 10,0,20))
                                                 .Cost(5)
                                                 .LinkedState(laserAttackState),

                                             new GOAPAction("Invoke")
                                                 .Pre(x => (int)x[BossState.Overheating] <= 3)
                                                 .Pre(x => (int)x[BossState.PlayerLife] >= 0)
                                                 .Pre(x => (float)x[BossState.Health] < 50)
                                                 .Pre(x => (BossMood)x[BossState.Mood] == BossMood.Angry)
                                                 .Effect(x => x[BossState.Health] = 100)
                                                 .Effect(x => x[BossState.Overheating] = Mathf.Clamp((int)x[BossState.Overheating] + 1,0,20))
                                                 .Effect(x => x[BossState.PlayerClose] = false)
                                                 .Effect(x => x[BossState.Mood] = BossMood.Calm)
                                                 .Cost(2)
                                                 .LinkedState(invokeWaveState),

                                             new GOAPAction("PowerDown")
                                                 .Pre(x => (int)x[BossState.PlayerLife] >= 0)
                                                 .Pre(x => (int)x[BossState.Overheating] >= 3)
                                                 .Pre(x => (BossMood)x[BossState.Mood] != BossMood.PoweredUp)
                                                 .Effect(x => x[BossState.Overheating] = 0)
                                                 .Effect(x => x[BossState.Mood] = BossMood.PoweredUp)
                                                 .LinkedState(powerDownState),
                                          };

        GOAPState from = new GOAPState();

        from.values[BossState.PlayerClose] = boss.IsPlayerClose();  //bool
        from.values[BossState.PlayerLife] = player.CurrentHP;       //int
        from.values[BossState.Health] = boss.CheckBossLife();       //float
        from.values[BossState.Overheating] = boss.CheckOverheating();//int
        from.values[BossState.Mood] = boss.Mood;                    //string-enum

        GOAPState to = new GOAPState();

        to.values[BossState.PlayerLife] = 0;
        to.values[BossState.Health] = 100f;

        var planner = new GoapPlanner();

        var plan = planner.Run(from, to, actions, useCoroutine, this);

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

        _fsm.CurrentState.Exit(null);
        _fsm.Active = false;

        PlanAndExecute();
    }

    private void ConfigureFsm(IEnumerable<GOAPAction> plan)
    {
        Debug.Log("Completed Plan");
        _fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }

}
public enum BossState
{
    PlayerLife,
    Angry,
    PlayerClose,
    PoweredUp,
    Overheating,
    Health,
    Mood
}
public enum BossMood
{
    Calm,
    PoweredUp,
    Angry
}
