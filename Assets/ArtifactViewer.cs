using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactViewer : MonoBehaviour
{
    public GameObject artifactIconPrefab;
    public float currentPos = 0;
    private float offSetValueX = 62f;
    public List<GameObject> artifactsInstances;
    // Start is called before the first frame update
    void Start()
    {
        artifactsInstances = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddArtifact(Artifact artifact) {
        Vector3 location = new Vector3(currentPos, 0, 0);
        GameObject ai = Instantiate(artifactIconPrefab, location, Quaternion.identity, transform);
        ai.GetComponent<ArtifactDisplay>().artifact = artifact;
        ai.GetComponent<ArtifactDisplay>().SetValues();
        artifactsInstances.Add(ai);
        currentPos += offSetValueX;
    }
}
