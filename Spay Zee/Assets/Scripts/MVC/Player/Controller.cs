using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : Icontroller
{
    Model _model;

    int pauseCounter;

    public Controller (View V, Model M)
    {
        _model = M;
        _model.ActionDMG += V.TakeDMG;
        _model.ActionDIE += V.DeadPlayer;
        _model.ActionMove += V.MovePlayer;
        _model.ActionFire += V.Shoot;
        _model.ActionSwitchShotType += V.ChangeShotType;
        _model.ActionHeal += V.Heal;
        _model.ActionSpeedBoost += V.Boost;
        _model.ActionInvulnerable += V.Invulnerable;
        _model.ActionMultiShot += V.MultiShot;
        _model.ActionStop += V.ShowMenu;
        _model.ActionContinue += V.HideMenu;
    }
    public void OnUpdate()
    {      
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseCounter++;
        }

        if(pauseCounter == 0)
        {
            _model.LookPlayer();

            _model.MovePlayer(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (Input.GetKeyDown(KeyCode.F))
                _model.Takedmg(1);

            if (Input.GetMouseButton(0))
                _model.Fire();

            if (Input.GetKeyDown(KeyCode.Q))
                _model.Previousbehaviour();

            if (Input.GetKeyDown(KeyCode.E))
                _model.NextBehaviour();
        }

        if (pauseCounter == 1)
        {
            _model.PauseMenu();
        }
        else if(pauseCounter > 1)
        {
            _model.UnpauseMenu();
            pauseCounter = 0;
        }
                                     
    }
}