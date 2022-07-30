using System;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class GOAPAction {

    public List<Func<Dictionary<BossState, object>, bool>> preconditionsLambdas { get; private set; }
    public List<Action<Dictionary<BossState, object>>> effectsLambdas       { get; private set; }
    public string                   name          { get; private set; }
    public float                    cost          { get; private set; }
    public IState                   linkedState   { get; private set; }


    public GOAPAction(string name) {
        this.name     = name;
        cost          = 1f;
        preconditionsLambdas = new List<Func<Dictionary<BossState, object>, bool>>();
        effectsLambdas       = new List<Action<Dictionary<BossState, object>>>();
    }

    public GOAPAction Cost(float cost) {
        if (cost < 1f) {
            //Costs < 1f make the heuristic non-admissible. h() could overestimate and create sub-optimal results.
            //https://en.wikipedia.org/wiki/A*_search_algorithm#Properties
            Debug.Log(string.Format("Warning: Using cost < 1f for '{0}' could yield sub-optimal results", name));
        }

        this.cost = cost;
        return this;
    }

    public GOAPAction Pre(Func<Dictionary<BossState,object>,bool> lambda) {
        preconditionsLambdas.Add(lambda);
        return this;
    }

    public GOAPAction Effect(Action<Dictionary<BossState, object>> lambda) {
        effectsLambdas.Add(lambda);
        return this;
    }

    public GOAPAction LinkedState(IState state) {
        linkedState = state;
        return this;
    }
}
