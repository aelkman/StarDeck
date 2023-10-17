using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ArtifactDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Artifact artifact;
    public string artifactName;
    public SpriteRenderer spriteRenderer;
    public TextMeshProUGUI flavorText;
    public bool isIcon = false;
    public ArtifactViewer artifactViewer;
    public GameObject hoverText;
    // Start is called before the first frame update
    void Start()
    {
        if(isIcon) {
            hoverText.SetActive(false);
        }
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

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if(isIcon) {
            hoverText.SetActive(true);
            hoverText.GetComponent<ArtifactHoverDescription>().flavorText.text = artifactName + " - " + artifact.description;
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if(isIcon) {
            hoverText.SetActive(false);
        }
    }

    public void SetValues() {
        artifactName = artifact.name;
        Image image = gameObject.GetComponent<Image>();
        image.sprite = artifact.artwork;
        // flavorText.text = artifactName + " - " + artifact.description;
    }

    private void OnMouseOver() {
        if(!isIcon) {
            if(Input.GetMouseButtonUp(0)) {
                AudioManager.Instance.PlayCardRustling();
                artifactViewer.AddArtifact(artifact);
                MainManager.Instance.AddArtifact(artifact.codeName);
                // spriteRenderer.enabled = false;
                gameObject.SetActive(false);
            }
        }
    }

}
