using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopGround : MonoBehaviour
{
    private BoxCollider2D groundCollider;
    private float boxColliderWidth;
    // Start is called before the first frame update
    void Start()
    {
        groundCollider = GetComponent<BoxCollider2D>();
        boxColliderWidth = groundCollider.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -2 * boxColliderWidth)
        {
            transform.position = new Vector3(
                transform.position.x + 4 * boxColliderWidth,
                transform.position.y,
                transform.position.z);
        }
    }
}
