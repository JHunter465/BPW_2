using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Is dit de plek waar je vaak je naam bovenaan zet? In dat geval schrijf ik hier graag mijn naam boven
// als trotse maker van al deze classes :)
//Jael van Rossum
//HKU Building Playful Worlds 2
//nr. 3032611
public class Firefly_Spawner : MonoBehaviour
{
    [Tooltip("Which Prefabs would you like to be spawned randomly as fireflies...")]
    [SerializeField] private List<GameObject> Fireflies = null;
    [Tooltip("This Box determines the size in which fireflies will spawn, NOTE: they will move outside the box once spawned")]
    [SerializeField] private GameObject SpawningBox = null;
    [Tooltip("Sets the minimum value of brightness for the firefly when spawned")]
    [SerializeField] private float min_Intensity = 0f;
    [Tooltip("Sets the maximum value of random ass bananas being spawned right after harambe tries to break down your tree when your mom got into a fight with him... What do you think dummy?! Max brightness for the firefly when spawned")]
    [SerializeField] private float max_Intensity = 0f;
    [Tooltip("Maximum amount of Fireflies allowed at once in the scene")]
    [SerializeField] private int maxFireflies = 12;

    private float x_Size = 0f;
    private float y_Size = 0f;
    private float z_Size = 0f;
    private float x_Pos = 0f;
    private float y_Pos = 0f;
    private float z_Pos = 0f;
    private float light_Intensity = 0f;

    private int fireFly_Index = 0;
    private int hasSpawned = 0;
    private int flyList_Num = 0;

    private List<GameObject> currentFlies = new List<GameObject>();

    private Light flylight = null;

    void Start()
    {
        //set Bounding box size so random spawn position can be calculated
        x_Size = SpawningBox.transform.localScale.x*transform.localScale.x;
        y_Size = SpawningBox.transform.localScale.y*transform.localScale.y;
        z_Size = SpawningBox.transform.localScale.z*transform.localScale.z;
        x_Pos = SpawningBox.transform.position.x;
        y_Pos = SpawningBox.transform.position.y;
        z_Pos = SpawningBox.transform.position.z;

        StartCoroutine(fireFlySpawn());
    }
    
    void Spawn()
    {
        //Choose random Firefly Prefab from editorIndex
        fireFly_Index = Random.Range(0, Fireflies.Count - 1);

        //Instantiate Firefly, set scale and add to Firefly list
        GameObject spawned_Firefly = Instantiate(Fireflies[fireFly_Index], new Vector3(Random.Range(x_Pos-x_Size,x_Pos+x_Size),Random.Range(y_Pos-y_Size,y_Pos+y_Size),Random.Range(z_Pos-z_Size,z_Pos+z_Size)), Quaternion.identity);
        hasSpawned += 1;
        spawned_Firefly.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        currentFlies.Add(spawned_Firefly);
        StartCoroutine(Flyfadein(spawned_Firefly));
    }

    void Destroy()
    {
        //Get Oldest Firefly and set variables for coroutine to destroy Firefly
        flylight = currentFlies[flyList_Num].GetComponentInChildren<Light>();
        light_Intensity = flylight.intensity;
        StartCoroutine(Flyfadeout(light_Intensity, flylight));
    }

    IEnumerator fireFlySpawn()
    {
        //While loop for constant spawning of Fireflies and checking to destroy if too many
        while (true)
        {
            yield return new WaitForSeconds(5f);
            Spawn();
            if (hasSpawned > maxFireflies)
            {
                Destroy();
            }
            yield return null;
        }
    }

    //When fly is spawned, slowly fade in the light
    IEnumerator Flyfadein (GameObject fly)
    {
        for (float i = 0f; i < 1; i+= 0.1f)
        {
            fly.GetComponentInChildren<Light>().intensity = Mathf.Lerp(0, Random.Range(min_Intensity,max_Intensity), i);
            yield return new WaitForSeconds(.5f);
        }
        yield return null;
    }

    //When fly must be destroyed. First fadeout...
    IEnumerator Flyfadeout (float cur_intensity, Light flylight)
    {
        for (float t = 0; t < 1; t += 0.1f)
        {
            flylight.intensity = Mathf.Lerp(cur_intensity, 0, t);
            yield return new WaitForSeconds(0.3f);
        }
        yield return StartCoroutine(FlyDestroy());
    }
    
    //Then destroy it and take it off the list
    IEnumerator FlyDestroy()
    {
        hasSpawned -= 1;
        Destroy(currentFlies[flyList_Num]);
        flyList_Num += 1;
        yield return null;
    }
}
