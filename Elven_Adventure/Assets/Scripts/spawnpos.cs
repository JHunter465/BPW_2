using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnpos : MonoBehaviour
{
    [SerializeField] private GameObject spawn = null;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForEndOfFrame();
        transform.position = spawn.transform.position;
        yield return null;
    }
}
