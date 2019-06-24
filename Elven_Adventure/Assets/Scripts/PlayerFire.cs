using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam = null;
    [SerializeField] private LayerMask HittableObjects = 0;
    [SerializeField] private GameObject ShootPosition = null;
    [SerializeField] private List<GameObject> vfx = new List<GameObject>();

    private GameObject vfx_Spawn = null;

    public PlayerStaff staff = null;

    private float timeToFire = 0f;

    private GameObject oldFly = null;

    private void Start()
    {
        if(cam == null)
        {
            Debug.LogError("No camera was able to be referenced!");
            this.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= timeToFire)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit _hit;
        if(Physics.Raycast (cam.transform.position, cam.transform.forward, out _hit, staff.range, HittableObjects))
        {
            vfx_Spawn = vfx[0];
            GameObject vfx_Clone = Instantiate(vfx_Spawn, ShootPosition.transform.position, Quaternion.identity);
            vfx_Clone.transform.rotation = cam.transform.rotation;
            timeToFire = Time.time + 1 / vfx_Spawn.GetComponent<ProjectileMovement>().projectileFireRate;
            if(_hit.collider.gameObject.layer == 13)
            {
                if(oldFly != _hit.collider.gameObject)
                {
                    oldFly = _hit.collider.gameObject;
                    vfx_Clone.GetComponent<ProjectileMovement>().SetFireFly(_hit.collider.gameObject);
                    _hit.collider.GetComponent<Firefly>().FlickertheThing();
                    _hit.collider.GetComponent<Firefly>().stopped = true;
                }
            }
        }
    }
}
