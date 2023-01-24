using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBoss : Boss
{
    public GameObject bossHitPoint;
    public LineRenderer[] webs;
    Transform[] hits;
    int webindex = 0;
    private void Start()
    {
        webindex = webs.Length;
        hits = new Transform[webs.Length];

        float offset = 0;
        for (int i = 0; i < webs.Length; i++)
        {
            Vector3 addVect = offset < 0 ? Vector3.zero : new Vector3(
                transform.position.x + offset * (i % 2 == 0 ? 1 : -1), 0, 0);
            
            hits[i] = Instantiate(
                bossHitPoint, 
                transform.position + addVect, 
                Quaternion.identity).transform;

            webs[i].SetPosition(0, transform.position + addVect);
            webs[i].SetPosition(1, transform.position + addVect);
            offset += (i % 2 == 0 ? 7 : 0);
        }
    }

    private void Update()
    {
        for (int i = 0; i < webindex; i++)
            SetWeb(i);
    }

    public void SetWeb(int i)
    {
        webs[i].SetPosition(1, transform.position);

        hits[i].position = new Vector2((webs[i].GetPosition(0).x + transform.position.x)*2/3, 
        (webs[i].GetPosition(0).y + transform.position.y)*2/3);
    }
}
