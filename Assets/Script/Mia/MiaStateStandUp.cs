using System;
using UnityEngine;

namespace Assets.Script.Mia
{
    public class MiaStateStandUp : MiaStatesGroundBase
    {
        public MiaStateStandUp(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Player.CanInputMove = false; 
        }

        public override void OnExit()
        {
            base.OnExit();
            Player.CanInputMove = true;
            Player.StartCoroutine(Player.SuperArmor());
            Player.StartCoroutine(Player.HurtFlashFX());
        }

        public override void OnUpdate()
        {
            //base.OnUpdate();
            if (isAnimFinish && Player.IsOnGround)
            {
                Player.RB.velocity = Vector2.zero;
                FSM.ChangeState(Player.IdleState);
                return;
            }
        }
    }
}
