using Assets.Script.Manager;
using UnityEngine;

namespace Assets.Script.Mia
{
    public class MiaStateParry : MiaState
    {
        protected bool canCounter;
        public MiaStateParry(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Player.CanInputMove = false;
            Player.RB.velocity = Vector2.zero;
            canCounter = false;
            Player.ParryPoint.SetActive(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            Player.CanInputMove = true;
            Player.SetParrying(false);
            Player.ParryPoint.SetActive(false);
        }

        public override void OnUpdate()
        {
            base.OnUpdate(); 
            if (Player.CanCounter)
            {
                canCounter = canCounter || Player.ParryHits();
            }
            else
            {
                canCounter = false;
            }

            if (canCounter)
            {
                Player.StartCoroutine(AudioManager.Instance.PlayShield());
                FSM.ChangeState(Player.CounterState);
                return;
            }
            if (!Player.IsParrying)
            {
                FSM.ChangeState(Player.IdleState);
                return;
            }
        }
    }
}
