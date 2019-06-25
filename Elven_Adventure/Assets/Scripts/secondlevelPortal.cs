using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class secondlevelPortal : MonoBehaviour
{
    [SerializeField] private LayerMask Player = 0;
    private void OnCollisionEnter(Collision collision)
    {
        if(Player == (Player | (1 << collision.gameObject.layer)))
        {
            GameManager.Instance.NextScene();
        }
    }
}
