using Assets.SoccerGameEngine_Basic_.Scripts.Managers;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Managers;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities;
using RobustFSM.Base;
using UnityEngine;
using static Assets.SoccerGameEngine_Basic_.Scripts.Managers.MatchManager;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.MatchOn.SubStates.MatchStopped.SubStates
{
    public class BroadcastHalfTimeStatus : BState
    {
        /// <summary>
        /// A reference to the wait time
        /// </summary>
        float waitTime;

        public override void Enter()
        {
            base.Enter();

            //set the wait time
            waitTime = 1f;

            //raise the half-start event
            RaiseTheHalfTimeStartEvent();
        }

        public override void Execute()
        {
            base.Execute();

            //decrement the time
            waitTime -= Time.deltaTime;

            //go to wait-for-kick-to-complete if time is less than 0
            if (waitTime <= 0f) Machine.ChangeState<HalfTime>();
        }

        public override void Exit()
        {
            base.Exit();

            //raise the event that I finished broadcasting the start
            //of the first half
            ActionUtility.Invoke_Action(Owner.OnFinishBroadcastHalfTimeStart);
        }

        /// <summary>
        /// Raises the half start event
        /// </summary>
        public void RaiseTheHalfTimeStartEvent()
        {
            //prepare an empty string
            string message = "Half Time";

            //raise the event
            BroadcastHalfTimeStart temp = Owner.OnBroadcastHalfTimeStart;
            if (temp != null) temp.Invoke(message);
        }

        /// <summary>
        /// Returns the owner of this instance
        /// </summary>
        public MatchManager Owner
        {
            get
            {
                return ((MatchManagerFSM)SuperMachine).Owner;
            }
        }
    }
}
