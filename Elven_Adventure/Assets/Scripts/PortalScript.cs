using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{

    [SerializeField] private LayerMask Interactible = 0;
    [SerializeField] private LayerMask Player = 0;
    [SerializeField] public GameObject Fire = null;
    [SerializeField] public List<GameObject> PortalSpawns = new List<GameObject>();

    private SphereCollider sphereCol = null;
    private BoxCollider boxCol = null;

    private void Start()
    {
        sphereCol = GetComponent<SphereCollider>();
        boxCol = GetComponent<BoxCollider>();
        sphereCol.enabled = true;
        boxCol.enabled = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (boxCol.enabled == false)
        {
            if(Interactible == (Interactible | (1 << collision.gameObject.layer)))
            {
                if(GameManager.Instance.FireFlyCount >= 10)
                {
                    foreach (GameObject spawns in PortalSpawns)
                    {
                        spawns.SetActive(true);
                    }
                    Fire.SetActive(false);
                    boxCol.enabled = true;
                    sphereCol.enabled = false;
                }
                else
                {
                    GameManager.Instance.NotEnoughFlies();
                }
            }
        }
        else
        {
            if(Player == (Player | (1 << collision.gameObject.layer)))
            {
                GameManager.Instance.NextScene();
            }
        }
    }
    public void SetManager()
    {
        if (gameObject.name == "Portal2")
        {
            GameManager.Instance.Portal2 = this.gameObject;
        }
    }
}
