using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("State")]
    public int maxHP; //100
    public bool DefaultIsFacingRight = true;
    public bool IsSuperArmor = false;

    [Space(5)]
    [Header("Idle")]
    public float IdleTime = 0;

    [Space(5)]
    [Header("Run")]
    public float runSpeed;
    public float ChaseDistance = 1.5f;

    [Space(5)]
    [Header("Attack1")]
    public int Attack1Damage = 10;
    public float Attack1RefillTime = 1;
    public bool IsHeaveAttack1 = false;

    [Space(5)]
    [Header("Attack2")]
    public int Attack2Damage = 30;
    public float Attack2RefillTime = 5;
    public float Attack2Distance;
    public bool IsHeaveAttack2 = false;

    [Space(5)]
    [Header("Attack3")]
    public int Attack3Damage = 50;
    public float Attack3RefillTime = 8;
    public float Attack3BulletSpeed;
    public float Attack3Rate = 0.05f;
    public float RangedDistance = 10f;
    public bool IsHeaveAttack3 = false;

    [Space(5)]
    [Header("Hurt")]
    public float HurtResetTime = 0.5f;
    public float StunResetTime = 3f;

    [Space(5)]
    [Header("Drop")]
    public int RedSoulDropRate = 40;
    public int RedSoulMaxDrop = 5; 
    public int RedSoulMinDrop = 0;
    public int GreenSoulDropRate = 30;
    public int GreenSoulMaxDrop = 3;
    public int GreenSoulMinDrop = 0;
}
