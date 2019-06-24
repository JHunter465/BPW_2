using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Is dit de plek waar je vaak je naam bovenaan zet? In dat geval schrijf ik hier graag mijn naam boven
// als trotse maker van al deze classes :)
//Jael van Rossum
//HKU Building Playful Worlds 2
//nr. 3032611
public class Firefly : MonoBehaviour
{
    [SerializeField] private LayerMask canInteract = 0;
    [SerializeField] private LayerMask Environment = 0;

    private Light child_light = null;
    private bool Moving = false;
    public bool stopped = false;
    private bool Reseting = false;

    public bool hit = false;

    private float Flickervalue = 5f;
    private float x_New = 0f;
    private float y_New = 0f;
    private float z_New = 0f;

    private float cur_intensity = 0f;

    private Vector3 oldPos = Vector3.zero;
    private Vector3 movePos = Vector3.zero;
    private Vector3 startpos = Vector3.zero;

    private Rigidbody rb = null;
    void Start()
    {
        //get stuff like rigidbody, startposition and Lightcomponent
        rb = GetComponent<Rigidbody>();
        startpos = transform.position;
        child_light = GetComponentInChildren<Light>();
    }

    void Update()
    {
        //Check if Firefly is already performing movement else, start moving
        if (!Moving)
        {
            oldPos = transform.position;
            StartCoroutine(moveFirefly());
            Moving = true;
        }
    }

    //Reset the Firefly if it collides with anything other than the player or its projectiles
    private void OnTriggerEnter(Collider other)
    {
        if(Environment != (Environment | (1 << other.gameObject.layer)) && other.gameObject.name != "Firefly" && !Reseting)
        {
            Reseting = true;
            StartCoroutine(Flyfadeout());
        }
        else if (canInteract == (canInteract | (1 << other.gameObject.layer)))
        {
            Reseting = true;
            StartCoroutine(Flyfadeout());
        }
    }

    //Same as before but now if it is inside a collider because we're using transform.position so 
    //the engine might think it never entered because it didn't hit the border of a collider
    private void OnTriggerStay(Collider other)
    {
        if (Environment != (Environment | (1 << other.gameObject.layer)) && other.gameObject.name != "Firefly" && !Reseting)
        {
            Reseting = true;
            stopped = true;
            Moving = true;
            StartCoroutine(FlyReset());
        }
    }
    IEnumerator moveFirefly()
    {
        //extra check to see if the fly should move or stand still
        if (stopped)
        {
            yield return null;
        }
        else
        {
            //Choose a random position in space to move towards within the range of 5 units (5 units is hardcoded as the fireflies otherwise seem to act like bumblebees)
            x_New = Random.Range((transform.position.x - 5f), (transform.position.x + 5f));
            y_New = Random.Range((transform.position.y - 5f), (transform.position.y + 5f));
            z_New = Random.Range((transform.position.z - 5f), (transform.position.z + 5f));
            movePos = new Vector3(x_New, y_New, z_New);
            for (float t = 0f; t < 1; t += Random.Range(0.01f, 0.05f))
            {
                if (!stopped)
                {
                    rb.MovePosition(Vector3.Lerp(oldPos, movePos, t));
                    yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
                }
                //Keep checking if the fly should stop as the loop takes a while to exit
                else if (stopped)
                {
                    yield return null;
                    break;
                }
            }
            Moving = false;
            yield return null;
        }
    }

    public void FlickertheThing()
    {
        StartCoroutine(Flickering());
    }

    public void KillTheFly()
    {
        StartCoroutine(Flyfadeout());
    }
    public IEnumerator Flickering()
    {
        while (!hit)
        {
            cur_intensity = GetComponentInChildren<Light>().intensity;
            float _randomValue = Random.Range(cur_intensity - 5, cur_intensity + 5);
            GetComponentInChildren<Light>().intensity = 0f;
            yield return new WaitForSeconds(.2f);
            GetComponentInChildren<Light>().intensity = cur_intensity;
            yield return new WaitForSeconds(.2f);
        }
        yield return null;
    }

    //Stop moving the firefly and fadeout its light
    IEnumerator Flyfadeout()
    {
        stopped = true;
        Moving = true;
        cur_intensity = GetComponentInChildren<Light>().intensity;
        for (float t = 0; t < 1; t += 0.1f)
        {
            GetComponentInChildren<Light>().intensity = Mathf.Lerp(cur_intensity, 0, t);
            yield return new WaitForSeconds(0.3f);
        }
        yield return StartCoroutine(FlyReset());
    }

    //Reset the Firefly to its original position after fading out if it has collided with an object
    IEnumerator FlyReset()
    {
        rb.MovePosition(startpos);
        for (float i = 0f; i < 1; i += 0.1f)
        {
            GetComponentInChildren<Light>().intensity = Mathf.Lerp(0, cur_intensity, i);
            yield return new WaitForSeconds(.5f);
        }
        // remove all the blocking variables so the firefly may be free again!
        stopped = false;
        Moving = false;
        Reseting = false;
        hit = false;
        yield return null;
    }
}
