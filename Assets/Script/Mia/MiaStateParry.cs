using Assets.Script.Manager;
using UnityEngine;

namespace Assets.Script.Mia
{
    public class MiaStateParry : MiaStatesGroundBase
    {
        protected bool canCounter;
        public MiaStateParry(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Player.CanInputMove = false;
            Player.RB.velocity = Vector3.zero;
            canCounter = false;
        }

        public override void OnExit()
        {
            base.OnExit();
            Player.CanInputMove = true;
            Player.SetParrying(false);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            canCounter = canCounter || Player.ParryHits();
            if (isAnimFinish)
            {
                if (canCounter)
                {
                    Player.StartCoroutine(AudioManager.Instance.PlayShield());
                    FSM.ChangeState(Player.CounterState);
                    return;
                }
                else
                {
                    FSM.ChangeState(Player.IdleState);
                    return;
                }
            }
        }
    }
}
