using System;
using UnityEngine;

namespace Assets.Script.Mia
{
    public class MiaStateStun : MiaStatesGroundBase
    {
        public MiaStateStun(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Player.CanInputMove = false;
            stateTime = 0.3f; 
            AudioManager.Instance.StopAttack();
            AudioManager.Instance.StartCoroutine(AudioManager.Instance.PlaySigh());
        }

        public override void OnExit()
        {
            base.OnExit();
            Player.CanInputMove = true;
        }

        public override void OnUpdate()
        {
            stateTime -= Time.deltaTime;
            //base.OnUpdate();
            if (stateTime < 0f && Player.IsOnGround)
            {
                Player.RB.velocity = Vector2.zero; 
                if (!Player.IsStunning)
                {
                    FSM.ChangeState(Player.StandUpState);
                    return;
                }
            }
        }
    }
}
