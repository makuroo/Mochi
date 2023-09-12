using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] float speed = 7f;
    private Rigidbody2D rb;
    private PlayerMovement target;
    private Vector2 moveDirection;
    private PlayerStatus iFrame;
    private Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindObjectOfType<PlayerMovement>();
        iFrame = GameObject.FindObjectOfType<PlayerStatus>();
        moveDirection = (target.transform.position - transform.position).normalized * speed;
        rb.velocity = new Vector3(moveDirection.x, moveDirection.y, 0);
        direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        
        //Quaternion newQuaternion = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 100f);
        //transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, newQuaternion.z));
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStatus>().health -= GameObject.FindObjectOfType<Boss>().projectileDamage;
            Destroy(gameObject);
        }
    }
}
