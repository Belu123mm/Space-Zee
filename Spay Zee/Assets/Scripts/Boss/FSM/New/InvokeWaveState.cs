using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using FSM;
using Object = UnityEngine.Object;


public class InvokeWaveState : MonoBaseState
{
    GameObject enemySpawner;
    GameObject BossBase;
    GameObject BossHide;
    private GameObject Warning;

    Boss boss;

    public override event Action OnNeedsReplan;

    float timer;
    bool InPosition;
    private bool Hiden;
    private bool spawned;
    int invokeReEnterCounter;

    float auxTim = 0;
    public InvokeWaveState(Boss _boss, GameObject _enemySpawner, GameObject _bossBase, GameObject _bossHide, Action onNeedsReplan)
    {
        boss = _boss;
        enemySpawner = _enemySpawner;
        BossBase = _bossBase;
        BossHide = _bossHide;
        OnNeedsReplan = onNeedsReplan;
        InPosition = false;
        spawned = false;
        Hiden = false;
        timer = 0;
        auxTim = 0;
    }

    public override void UpdateLoop()
    {
        
            if (!InPosition)
            {
                if (boss.transform.position != BossHide.transform.position)
                {
                    boss.transform.position = Vector3.MoveTowards(boss.transform.position, BossHide.transform.position, Time.deltaTime * 10);

                    Vector3 lookAtPos = BossHide.transform.position;
                    lookAtPos.z = boss.transform.position.z;
                    boss.transform.up = lookAtPos - boss.transform.position;

                    return;
                }
            }
            if ((boss.transform.position == BossHide.transform.position) && Hiden == false)
            {
                InPosition = true;
                Hiden = true;
                Warning = GameObject.Instantiate(Resources.Load("Art/MISC/warningEffect/warningmessage", typeof(Object))) as GameObject;
                Warning.transform.position = new Vector3(-10.08f, 0, 0);
                GameObject.Destroy(Warning, 2.3f);
                return;
            }

            if (InPosition && Hiden)
            {
                if (!spawned)
                {
                    auxTim += Time.deltaTime;
                    if (auxTim >= 3.1f)
                    {
                        enemySpawner.SetActive(true);
                        spawned = true;
                    }
                }
                if (timer >= 6f)
                {
                    Vector3 lookAtPos = BossBase.transform.position;
                    lookAtPos.z = boss.transform.position.z;
                    boss.transform.up = lookAtPos - boss.transform.position;

                    boss.transform.position = Vector3.MoveTowards(boss.transform.position, BossBase.transform.position, Time.deltaTime * 2);
                    if ((boss.transform.position == BossBase.transform.position)) Hiden = false;
                }
            
        }
  
        if (spawned)
        {
            timer += Time.deltaTime;
            if (boss.life < 100f)
            {
                boss.life += Time.deltaTime / 2f;
            }
            else { boss.life = 100f; }
        }
    }

    public override IState ProcessInput()
    {
        if (timer >= 7 && Transitions.ContainsKey("OnChargeState") && invokeReEnterCounter > 1)
        {
            InPosition = false;
            Hiden = false;
            spawned = false;
            
            timer = 0;
            invokeReEnterCounter = 0;
            boss.invokeStateStarter -= 15;
            boss.overheatingCounter++;
            return Transitions["OnChargeState"];
        }
        else if (timer >= 8)
        {
            boss.overheatingCounter++;
            invokeReEnterCounter++;
            InPosition = false;
            Hiden = false;
            spawned = false;
            timer = 0;
            boss.invokeStateStarter -= 15;
            OnNeedsReplan?.Invoke();
        }
        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        timer = 0;
        base.Enter(from);
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        spawned = false;
        return base.Exit(to);
    }
}
