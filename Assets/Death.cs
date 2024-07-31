
using UnityEngine;

public class Death : MonoBehaviour
{
    public MiaController Controller;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            Debug.Log(collision.gameObject);
            Controller.SetCurrentHP(1000);
        }
    }
}
