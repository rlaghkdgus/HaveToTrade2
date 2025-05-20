using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wolf"))
        {
            Destroy(collision.gameObject); // ´Á´ë Á¦°Å
            Destroy(this.gameObject);      // ÃÑ¾Ë Á¦°Å
        }
    }
}
