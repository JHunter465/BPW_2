using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachtoManager : MonoBehaviour
{
    public void attachToManager()
    {
        GameManager.Instance.NewSpawnLocation = this.gameObject;
    }
}
