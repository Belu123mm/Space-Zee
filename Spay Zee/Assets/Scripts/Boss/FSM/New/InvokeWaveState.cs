using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using FSM;
using Object = UnityEngine.Object;


public class InvokeWaveState : MonoBaseState
{
    public GameObject enemySpawner;
    public GameObject BossBase;
    public GameObject BossHide;
    private GameObject Warning;

    public Boss boss;

    public override event Action OnNeedsReplan;

    float timer;
    bool InPosition;
    private bool Hiden;
    private bool spawned;
    bool start;
    int invokeReEnterCounter;

    private void Awake()
    {
        InPosition = false;
        spawned = false;
        Hiden = false;
        timer = 0;
    }

    public override void UpdateLoop()
    {
        if (start)
        {
            if (!InPosition)
            {
                if (transform.position != BossHide.transform.position)
                {
                    transform.position = Vector3.MoveTowards(transform.position, BossHide.transform.position, Time.deltaTime * 10);

                    Vector3 lookAtPos = BossHide.transform.position;
                    lookAtPos.z = transform.position.z;
                    transform.up = lookAtPos - transform.position;

                    return;
                }
            }
            if ((transform.position == BossHide.transform.position) && Hiden == false)
            {
                InPosition = true;
                Hiden = true;
                Warning = Instantiate(Resources.Load("Art/MISC/warningEffect/warningmessage", typeof(Object))) as GameObject;
                Warning.transform.position = new Vector3(-10.08f, 0, 0);
                Destroy(Warning, 2.3f);
                return;
            }

            if (InPosition && Hiden)
            {
                if (!spawned)
                {
                    StartCoroutine(Spawner());
                    spawned = true;
                }
                if (timer >= 6f)
                {
                    Vector3 lookAtPos = BossBase.transform.position;
                    lookAtPos.z = transform.position.z;
                    transform.up = lookAtPos - transform.position;

                    transform.position = Vector3.MoveTowards(transform.position, BossBase.transform.position, Time.deltaTime * 2);
                    if ((transform.position == BossBase.transform.position)) Hiden = false;
                }
            }
        }
    }

    private void Update()
    {
        if (spawned)
        {
            timer += Time.deltaTime;
            boss.life += Time.deltaTime / 2f;
        }           
    }

    public override IState ProcessInput()
    {
        if (timer >= 7 && Transitions.ContainsKey("OnChargeState") && invokeReEnterCounter > 1)
        {
            InPosition = false;
            Hiden = false;
            spawned = false;
            start = false;
            timer = 0;
            invokeReEnterCounter = 0;
            BossWorldState.instance.invokeStateStarter -= 15;
            boss.powerCounter++;
            return Transitions["OnChargeState"];
        }
        else if(timer >= 8)
        {
            boss.powerCounter++;
            invokeReEnterCounter++;
            InPosition = false;
            Hiden = false;
            spawned = false;
            start = false;
            timer = 0;
            BossWorldState.instance.invokeStateStarter -= 15;
            OnNeedsReplan?.Invoke();
        }
        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        start = true;
        timer = 0;
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        spawned = false;
        return base.Exit(to);        
    }

    IEnumerator Spawner()
    {
        yield return new WaitForSeconds(3.1f);
        enemySpawner.SetActive(true);
        StopCoroutine(Spawner());
    }
}
