using UnityEngine;

public class PlatformEffector : MonoBehaviour
{
    public int PlayerLayer = 9;
    public PlatformEffector2D effector { get; private set; }
    private void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }
    private void Update()
    {
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            effector.colliderMask &= ~(1 << PlayerLayer);
        }
        else
        {
            effector.colliderMask |= (1 << PlayerLayer);
        }
    }
}
