using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        }
    }
}
