using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZagEnemi : AbstractObject {

    public List<Transform> tabPath = new List<Transform>();
    private Transform paths;

    private void Update()
    {
        
    }

    public void AddNewPointPath()
    {
        
        if(paths == null)
        {
            paths = new GameObject("paths").transform;
            paths.position = transform.position;
            paths.parent = this.transform;
        }
        var tempPath = new GameObject("point" + tabPath.Count).transform;
        if (tabPath.Count == 0)
        {
            tempPath.position = paths.position;
        }
        else
        {
            tempPath.position = tabPath[tabPath.Count - 1].position;
        }
        tabPath.Add(tempPath);
        tempPath.parent = paths;
    }

    public void RemoveLastPointPath()
    {
        if(tabPath.Count > 0)
        {
            var tempPath = tabPath[tabPath.Count - 1];
            tabPath.RemoveAt(tabPath.Count - 1);
            DestroyImmediate(tempPath.gameObject);
        }
    }
}
