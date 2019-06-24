using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(SpinmeRound());
    }

    IEnumerator SpinmeRound()
    {
        while (enabled)
        {
            transform.Rotate(Vector3.up);
            yield return new WaitForSeconds(.05f);
        }
        yield return null;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 9)
        {
            if (GameManager.Instance.FireFlyCount > 10)
            {
                GameManager.Instance.EndLevel();
            }
            else
            {
                GameManager.Instance.NotEnoughFliesEnd();
            }
        }
    }
}
