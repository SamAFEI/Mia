using UnityEngine;

namespace Assets.Script.Mia
{
    public class MiaStateIdle : MiaStatesGroundBase
    {
        public MiaStateIdle(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Player.RB.velocity = Vector2.zero;
            AudioManager.Instance.StopAttack();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Player.InputX != 0)
            {
                FSM.ChangeState(Player.RunState);
                return;
            }
        }
    }
}
