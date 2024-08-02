using UnityEngine;

namespace Assets.Script.Mia
{
    public class MiaStateDie : MiaState
    {
        bool IsSwitch;
        public MiaStateDie(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Player.CanInputMove = false;
            Player.RB.velocity = Vector2.zero;
            TimerManager.Instance.SlowFrozenTime(0.5f);
            stateTime = 1f;
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            //base.OnUpdate();
            stateTime -= Time.deltaTime;
            if (!IsSwitch &&stateTime < 0)
            {
                IsSwitch = true;
                AudioManager.Instance.StopBGM();
                GameObject.Find("GUICanvas").GetComponent<UICanvas>().SwitchOnDiedScreen();
            }
            if (!GameManager.Instance.IsDie)
            {
                FSM.ChangeState(Player.IdleState);
                return;
            }
        }
    }
}
