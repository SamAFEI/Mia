using UnityEngine;

namespace Assets.Script.Mia
{
    public class MiaStateCounter : MiaStatesAttackBase
    {
        public MiaStateCounter(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter(); 
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (isAnimFinish)
            {
                FSM.ChangeState(Player.IdleState);
                return;
            }
        }
    }
}
