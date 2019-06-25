using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayerUIPrefab = null;
    [SerializeField] public GameObject Player = null;
    [SerializeField] public GameObject NewSpawnLocation = null;
    [SerializeField] public GameObject Portal2 = null;

    private GameObject PlayerUIInstance = null;
    private GameObject FlyInstance = null;
    private bool StartCanvas = true;
    private bool secretleveldone = false;

    public int FireFlyCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static GameManager Instance { get; private set; }
    void Start()
    {
        PlayerUIInstance = Instantiate(PlayerUIPrefab);

        //Configure UI
        PlayerUI ui = PlayerUIInstance.GetComponent<PlayerUI>();
        if(ui == null)
        {
            Debug.LogError("No playerui component on Playerui Prefab");
        }
        ui.setController(Player.GetComponent<PlayerController>());
    }

    public void addscore(GameObject _FlyInstance)
    {
        if(_FlyInstance != FlyInstance)
        {
            FlyInstance = _FlyInstance;
            FireFlyCount += 1;
            PlayerUIInstance.GetComponent<PlayerUI>().setFireFly(FireFlyCount);
        }
    }
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PreviousLevel()
    {
        secretleveldone = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        StartCoroutine(SetPlayerPos());
    }

    public void EndLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
    }
    public void NotEnoughFlies()
    {
        PlayerUIInstance.GetComponent<PlayerUI>().TellNotEnough(FireFlyCount);
    }
    public void NotEnoughFliesEnd()
    {
        PlayerUIInstance.GetComponent<PlayerUI>().TellNoEnd(FireFlyCount);
    }

    IEnumerator SetPlayerPos()
    {
        if (secretleveldone)
        {
            FindObjectOfType<PlayerController>().SetManager();
            FindObjectOfType<AttachtoManager>().attachToManager();
            //FindObjectOfType<canvasScript>().SetManager();
            FindObjectOfType<PortalScript>().SetManager();
            Player.transform.position = NewSpawnLocation.transform.position;
            List<GameObject> portalspawns = Portal2.GetComponent<PortalScript>().PortalSpawns;
            foreach (GameObject spawns in portalspawns)
            {
                spawns.SetActive(true);
            }
            Portal2.GetComponent<PortalScript>().Fire.gameObject.SetActive(false);
        }
        yield return null;
    }
}
