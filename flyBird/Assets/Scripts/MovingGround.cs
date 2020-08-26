using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGround : MonoBehaviour
{
    public float speed = 2;

    private Rigidbody2D rb2D;

    // Start is called before the first frame update
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.velocity = new Vector2(-speed, 0);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (GameController.instance.isGameOver)
    //    {
    //        rb2D.velocity = Vector2.zero;
    //    }
    //}
}
