using UnityEngine;

namespace Assets.Script.Mia
{
    public class MiaStateAttack : MiaStatesAttackBase
    {
        protected int comboCounter;
        protected float lastTimeAttacked;
        protected float comboWindow = 0.2f;
        protected float comboTime = 0.4f;
        protected float comboLoopTime;
        protected string comboAnimator;
        protected enum enCombo
        {
            ComboA, ComboB, ComboC, ComboD, ComboE
        }
        public MiaStateAttack(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            comboAnimator = "";
            //if (comboCounter == 2 && Time.time >= lastTimeAttacked + comboWindow
            //    && Time.time <= lastTimeAttacked + comboTime && Player.IsHoldDownAttack)
            //{
            //    comboAnimator = enCombo.ComboB.ToString();
            //    comboLoopTime = 0.6f;
            //}
            //else 
            if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            {
                comboCounter = 0;
            }
            comboCounter++;
            base.OnEnter();
            isStrong = false;
            if (Player.IsHeavyAttacking)
            {
                damage = Player.HeavyAttackDamage;
                isStrong = true;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            lastTimeAttacked = Time.time;
        }
        public override void OnUpdate()
        {
            /*if (comboAnimator == enCombo.ComboB.ToString()
                    && Player.IsHoldDownAttack && comboLoopTime >= 0f)
            {
                comboLoopTime -= Time.deltaTime;
            }
            else if (comboAnimator == enCombo.ComboB.ToString())
            {
                isAnimFinish = true;
            }*/
            base.OnUpdate();
        }
        public override void AnimatorPlay()
        {
            //base.AnimatorPlay();
            if (Player.IsHeavyAttacking)
            {
                if (comboCounter == 3)
                {
                    Player.animator.Play(enCombo.ComboC.ToString());
                    Player.SetHeavyAttacking(false);
                }
                else
                {
                    Player.animator.Play("HeavyAttack");
                }
                comboCounter = 0;
            }
            else if(comboAnimator != "")
            {
                Player.animator.Play(comboAnimator);
            }
            else
            {
                Player.animator.Play("Attack" + comboCounter);
            }
        }
    }
}
