using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 0f;
    [SerializeField] public float projectileFireRate = 0f;
    [SerializeField] private GameObject muzzleFlash = null;
    [SerializeField] private GameObject impactFX = null;

    private GameObject firefly = null;

    private void Start()
    {
        StartCoroutine(Timer());
        if(muzzleFlash != null)
        {
            GameObject _muzzleVFX = Instantiate(muzzleFlash, transform.position, Quaternion.identity);
            _muzzleVFX.transform.forward = gameObject.transform.forward;
            ParticleSystem _psMuzzle = _muzzleVFX.GetComponent<ParticleSystem>();
            if(_psMuzzle != null)
            {
                Destroy(_muzzleVFX, _psMuzzle.main.duration);
            }
            else
            {
                ParticleSystem _psChildMuzzle = _muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(_muzzleVFX, _psChildMuzzle.main.duration);
            }
        }
    }

    public void SetFireFly(GameObject _Firefly)
    {
        firefly = _Firefly;
    }

    private void Update()
    {
        if(projectileSpeed != 0)
        {
            transform.position += transform.forward * (projectileSpeed * Time.deltaTime);
        }
        if(firefly != null)
        {
            if (Vector3.Distance(gameObject.transform.position, firefly.transform.position) < 5)
            {
                firefly.GetComponent<Firefly>().hit = true;
                firefly.GetComponent<Firefly>().KillTheFly();
                GameManager.Instance.addscore(firefly);
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        projectileSpeed = 0;

        ContactPoint _conPoint = collision.contacts[0];
        Quaternion _rotation = Quaternion.FromToRotation(Vector3.up, _conPoint.normal);
        Vector3 _fxPos = _conPoint.point;

        if(impactFX != null)
        {
            GameObject _impactVFX = Instantiate(impactFX, _fxPos, _rotation);
            ParticleSystem _psImpact = _impactVFX.GetComponent<ParticleSystem>();
            if (_psImpact != null)
            {
                Destroy(_impactVFX, _psImpact.main.duration);
            }
            else
            {
                ParticleSystem _psChildImpact = _impactVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(_impactVFX, _psChildImpact.main.duration);
            }
        }
        Destroy(gameObject);
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(15f);
        Destroy(gameObject);
        yield return null;
    }
}