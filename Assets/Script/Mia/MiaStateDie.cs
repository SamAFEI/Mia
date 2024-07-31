using UnityEngine;

namespace Assets.Script.Mia
{
    public class MiaStateDie : MiaState
    {
        public MiaStateDie(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Player.RB.velocity = Vector2.zero;
            GameObject.Find("GUICanvas").GetComponent<UICanvas>().SwitchOnDiedScreen();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            //base.OnUpdate();
            if (!GameManager.Instance.IsDie)
            {
                FSM.ChangeState(Player.IdleState);
                return;
            }
        }
    }
}
