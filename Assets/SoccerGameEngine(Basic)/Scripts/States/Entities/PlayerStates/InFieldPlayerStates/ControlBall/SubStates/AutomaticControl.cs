using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.KickBall.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities.Enums;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities.Objects;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ControlBall.SubStates
{
    public class AutomaticControl : BState
    {
        int maxNumOfTries;
        float maxPassTime;
        Range rangePassTime = new Range(0.5f, 1f);

        public override void Enter()
        {
            base.Enter();

            //set the range
            maxNumOfTries = Random.Range(1, 5);
            maxPassTime = Random.Range(rangePassTime.Min, rangePassTime.Max);

            //set the steering
            Owner.RPGMovement.SetMoveTarget(Owner.OppGoal.transform.position);
            Owner.RPGMovement.SetRotateFacePosition(Owner.OppGoal.transform.position);
            Owner.RPGMovement.SetSteeringOn();
            Owner.RPGMovement.SetTrackingOn();
        }

        public override void Execute()
        {
            base.Execute();

            //decrement time
            if(maxPassTime > 0)
                maxPassTime -= Time.deltaTime;
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            //set the steering
            Owner.RPGMovement.SetMoveTarget(Owner.OppGoal.transform.position);
            Owner.RPGMovement.SetRotateFacePosition(Owner.OppGoal.transform.position);

            if (Owner.CanScore())
            {
                //go to kick-ball state
                Owner.KickType = KickType.Shot;
                SuperMachine.ChangeState<KickBallMainState>();
            }
            else if (maxPassTime <= 0 || Owner.IsThreatened())  //try passing if threatened or depleted wait time
            {
                // check if I still should consider pass safety
                bool considerPassSafety = true;// maxNumOfTries > 0;

                //start considering passing if wait -time is less than zero
                //find player to pass ball to if threatened or
                //has spend alot of time controlling the ball
                if (Owner.CanPass(considerPassSafety))
                {
                    //go to kick-ball state
                    Owner.KickType = KickType.Pass;
                    SuperMachine.ChangeState<KickBallMainState>();
                }

                // decrement max num of tries
                if (maxNumOfTries > 0)
                    --maxNumOfTries;
            }
        }

        public override void Exit()
        {
            base.Exit();

            //stop steering
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOff();
        }

        public Player Owner
        {
            get
            {
                return ((InFieldPlayerFSM)SuperMachine).Owner;
            }
        }
    }
}
