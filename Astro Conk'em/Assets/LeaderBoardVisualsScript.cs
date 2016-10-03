using UnityEngine;
using System.Collections;

public class LeaderBoardVisualsScript : MonoBehaviour
{

    public static LeaderBoardVisualsScript singleton;
    bool active = false;

    public GameObject entryPrefab;

    // Use this for initialization
    void Start()
    {
        singleton = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Activate()
    {
        active = true;
        Vector3 entryPos = entryPrefab.transform.localPosition;

        for(int i=0; i<5; i++)
        {
            Instantiate(entryPrefab, entryPos, Quaternion.LookRotation(transform.forward));
            entryPos -= Vector3.up * 85;
        }
    }
}
