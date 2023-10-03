using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArtifactDisplay : MonoBehaviour
{
    public Artifact artifact;
    public string artifactName;
    public SpriteRenderer spriteRenderer;
    public TextMeshProUGUI flavorText;
    public bool isIcon = false;
    public ArtifactViewer artifactViewer;
    public Image iconImage;
    // Start is called before the first frame update
    void Start()
    {
        artifactViewer = GameObject.Find("ArtifactViewer").GetComponent<ArtifactViewer>();
        if(!isIcon) {
            if(MainManager.Instance != null) {
                artifactName = MainManager.Instance.possibleArtifacts[Random.Range(0, MainManager.Instance.possibleArtifacts.Count)];
                MainManager.Instance.possibleArtifacts.Remove(artifactName);
            }
            else {
                artifactName = "MARKS_MED";
            }
            artifact = Resources.Load<Artifact>("Artifacts/" + artifactName);
            artifactName = artifact.name;
            spriteRenderer.sprite = artifact.artwork;
            flavorText.text = artifactName + " - " + artifact.description;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValues() {
        artifactName = artifact.name;
        // iconImage.sprite = artifact.artwork;
        flavorText.text = artifactName + " - " + artifact.description;
    }

    private void OnMouseOver() {
        if(!isIcon) {
            if(Input.GetMouseButtonUp(0)) {
                artifactViewer.AddArtifact(artifact);
                spriteRenderer.enabled = false;
            }
        }
    }
}
