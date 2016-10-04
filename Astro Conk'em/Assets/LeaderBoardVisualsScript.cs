using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LeaderBoardVisualsScript : MonoBehaviour
{
    public Image fade;
    public static LeaderBoardVisualsScript singleton;
    bool active = false;

    public GameObject entryPrefab, nameBoxParent;
    Vector3 listStartPos;

    public List<Text> entries;
    public ScreenCoverScript sC;

    public Transform cam, camTargetPos;

    // Use this for initialization
    void Start()
    {
        singleton = this;
        listStartPos = entryPrefab.transform.localPosition;
        entryPrefab.GetComponent<Text>().text = "";

        sC.myImg.color = Color.white;
        entries = new List<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        fade.color = Color.Lerp(fade.color, Color.clear, 4 * Time.deltaTime);

        if(active)
        {
            nameBoxParent.transform.localPosition = Vector3.Lerp(nameBoxParent.transform.localPosition, -Vector3.up * 1000, 1 * Time.deltaTime);

            for(int i=0; i<entries.Count; i++)
            {
                entries[i].transform.localPosition = Vector3.Lerp(entries[i].transform.localPosition, new Vector3(0, entries[i].transform.localPosition.y, 0), 2 * Time.deltaTime);
            }

            if(Input.GetButtonDown("Fire1"))
            {
                sC.coverScreen = true;
            }

            cam.transform.position = Vector3.Lerp(cam.transform.position, camTargetPos.position, 4 * Time.deltaTime);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, camTargetPos.rotation, 3 * Time.deltaTime);
        }
    }

    public void Activate()
    {
        active = true;
        Vector3 entryPos = entryPrefab.transform.localPosition;

        for(int i=0; i<5; i++)
        {
            GameObject newName = Instantiate(entryPrefab, entryPos, Quaternion.LookRotation(transform.forward)) as GameObject;
            newName.transform.SetParent(transform, false);
            newName.transform.localPosition = entryPos;

            newName.GetComponent<Text>().text = "      " + (i+1) + ":  " + LocalHighScoreManager.g_instance.getScore(i).name + "               <i>" + LocalHighScoreManager.g_instance.getScore(i).score + "</i>";
          
            entries.Add(newName.GetComponent<Text>());
            entryPos -= Vector3.up * 9;
            entryPos -= Vector3.right * 50;
        }

        //GameManager.globalSoundSource.PlayOneShot(SoundBank.sndBnk.menuClick);
    }
}
