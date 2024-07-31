using Assets.Script.Manager;
using Unity.VisualScripting;
using UnityEngine;
namespace Assets.Script.Enemy
{
    public class EnemyStateStun : EnemyState
    {
        public EnemyStateStun(EnemyController _enemy, EnemyFSM _FSM, string _animName) : base(_enemy, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            stateTime = Enemy.Data.StunResetTime;
            Enemy.StartCoroutine(Enemy.StunFlashFX());
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (stateTime < 0f)
            {
                FSM.ChangeState(Enemy.IdleState);
            }
        }
    }
}
