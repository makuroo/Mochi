using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ChangeLayer());
        }
    }

   private IEnumerator ChangeLayer()
    {
        gameObject.layer = 0;
        yield return new WaitForSeconds(1f);
        gameObject.layer = 6;
    }
}
