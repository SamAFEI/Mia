using UnityEngine;

namespace Assets.Script.Mia
{
    public class MiaStateAirAttack : MiaStatesAttackBase
    {
        protected int comboCounter;
        protected float lastTimeAttacked;
        protected float comboWindow = 0.3f;
        public MiaStateAirAttack(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            {
                comboCounter = 0;
            }
            comboCounter++;
            base.OnEnter(); 
            if (comboCounter < 3)
            {
                Player.SetGravityScale(0);
                Player.RB.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            lastTimeAttacked = Time.time;
        }

        public override void OnUpdate()
        {
            base.OnUpdate(); 
        }

        public override void AnimatorPlay()
        {
            //base.AnimatorPlay();
            Player.animator.Play("AirAttack" + comboCounter);
        }
    }
}
