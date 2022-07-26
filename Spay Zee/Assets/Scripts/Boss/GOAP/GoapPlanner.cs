﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FSM;
using UnityEngine;

public class GoapPlanner
{

    private const int _WATCHDOG_MAX = 10000;
    private int _watchdog;

    public IEnumerable<GOAPAction> Run(GOAPState from, GOAPState to, IEnumerable<GOAPAction> actions, bool RunCoroutine, MonoBehaviour mono)
    {
        _watchdog = _WATCHDOG_MAX;

        var astar = new AStar<GOAPState>();

        if (RunCoroutine == true)
        {
            IEnumerable<GOAPState> call = null;
            mono.StartCoroutine(astar.Run(from,
                state => Satisfies(state, to),
                node => Explode(node, actions, ref _watchdog),
                state => GetHeuristic(state, to),
                x => call = x));
            //Debug.Log(_watchdog);
            //Debug.Log(call);
            return CalculateGoap(call);
        }
        else
        {
            var path = astar.RunOriginal(from,
                state => Satisfies(state, to),
                node => Explode(node, actions, ref _watchdog),
                state => GetHeuristic(state, to));

            return CalculateGoap(path);
        }
    }

    private IEnumerable<GOAPAction> CalculateGoap(IEnumerable<GOAPState> sequence)
    {

        //foreach (var act in sequence.Skip(1))
        //{
        //    Debug.Log(act);
        //}
        //Debug.Log(sequence);
        //Debug.Log("WATCHDOG " + _watchdog);
        return sequence.Skip(1).Select(x => x.generatingAction);
    }


    public static FiniteStateMachine ConfigureFSM(IEnumerable<GOAPAction> plan, Func<IEnumerator, Coroutine> startCoroutine)
    {
        var prevState = plan.First().linkedState;

        var fsm = new FiniteStateMachine(prevState, startCoroutine);

        foreach (var action in plan.Skip(1))
        {
            if (prevState == action.linkedState) continue;
            fsm.AddTransition("On" + action.linkedState.Name, prevState, action.linkedState);

            prevState = action.linkedState;
        }

        return fsm;
    }

    private static float GetHeuristic(GOAPState from, GOAPState goal)
    {
        int a = goal.valuesDictionary.Count(kv => !kv.In(from.valuesDictionary));

        return a;
    }
    private static bool Satisfies(GOAPState from, GOAPState to)
    {
        //Debug.Log(from + "--- " + to);
        return to.valuesDictionary.All(kv => kv.In(from.valuesDictionary));
    }

    private static IEnumerable<WeightedNode<GOAPState>> Explode(GOAPState node, IEnumerable<GOAPAction> actions, ref int watchdog)
    {
        if (watchdog == 0) return Enumerable.Empty<WeightedNode<GOAPState>>();
        watchdog--;

        var a = actions.Where(action => action.preconditionsLambdas.All(kv => kv.Invoke(node.valuesDictionary)))
                      .Aggregate(new List<WeightedNode<GOAPState>>(), (possibleList, action) =>
                      {
                          var newState = new GOAPState(node);
                          for (int i = 0; i < action.effectsLambdas.Count; i++) action.effectsLambdas[i](newState.valuesDictionary);
                          newState.generatingAction = action;
                          newState.step = node.step + 1;

                          possibleList.Add(new WeightedNode<GOAPState>(newState, action.cost));
                          return possibleList;
                      });
        return a;
    }
}
