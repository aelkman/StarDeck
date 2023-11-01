using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ArtifactItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Artifact artifact;
    public string artifactName;
    public Image image;
    public TextMeshProUGUI flavorText;
    public ArtifactViewer artifactViewer;
    public GameObject hoverText;
    private bool mouseOver = false;
    // Start is called before the first frame update
    void Start()
    {
        artifactViewer = GameObject.Find("ArtifactViewer").GetComponent<ArtifactViewer>();
        if(MainManager.Instance != null) {
            artifactName = MainManager.Instance.possibleArtifacts[Random.Range(0, MainManager.Instance.possibleArtifacts.Count)];
            MainManager.Instance.possibleArtifacts.Remove(artifactName);
        }
        else {
            artifactName = "MARKS_MED";
        }
        artifact = Resources.Load<Artifact>("Artifacts/" + artifactName);
        artifactName = artifact.name;
        image.sprite = artifact.artwork;
        flavorText.text = artifactName + " - " + artifact.description;
    }

    // Update is called once per frame
    void Update()
    {
        if(mouseOver && Input.GetMouseButtonUp(0)) {
            AudioManager.Instance.PlayCardRustling();
            artifactViewer.AddArtifact(artifact);
            MainManager.Instance.AddArtifact(artifact.codeName);
            // spriteRenderer.enabled = false;
            gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        mouseOver = true;
        hoverText.SetActive(true);
        hoverText.GetComponent<ArtifactHoverDescription>().flavorText.text = artifactName + " - " + artifact.description;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        mouseOver = false;
        hoverText.SetActive(false);
    }

    public void SetValues() {
        artifactName = artifact.name;
        Image image = gameObject.GetComponent<Image>();
        image.sprite = artifact.artwork;
        // flavorText.text = artifactName + " - " + artifact.description;
    }
}
