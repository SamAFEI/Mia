using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Mia
{
    public class MiaStateDodge : MiaState
    {
        public MiaStateDodge(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Player.shadow.ShadowBone = Player.boneSide;
            Player.shadow.maxAfterTime = Player.Data.dodgeEndTime;
            Player.shadow.IsSpawn = true;
            Player.SetSuperArmor(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            Player.SetSuperArmor(false);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!Player.IsDodging)
            {
                FSM.ChangeState(Player.IdleState);
            }
        }
    }
}
