using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class WeaponWindow : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public TypewriterText typewriterText;
    public VideoPlayer video;
    public RawImage rawImage;
    public bool selectionStarted = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetBody(string weapon) {
        if(weapon == "Blaster") {
            title.text = "Blaster";
            description.text = "Who doesn't like a good old fashioned <b>BLASTER PISTOL</b>?<br><br>Pew pew pew, and all that! Blast 'em down, taking care not to run out of <b>AMMO</b>, since you need ammo to shoot, of course!<br><br>When you run out, you'll need to <b>RELOAD</b> using one of your <i>Energy Cell</i> cards, or you can just give up and die if you want... that works too.";
            typewriterText.gameObject.SetActive(true);
            video.clip = (VideoClip)Resources.Load("WeaponAssets/Blaster Demo");
            video.targetTexture = (RenderTexture)Resources.Load("WeaponAssets/BlasterTex");
            rawImage.texture = video.targetTexture;
        }
        else if(weapon == "Hammer") {
            title.text = "Ice hammer";
            description.text = "Feeling a little <i>cold</i>? Then pick the Ice Hammer, a frozen delight!<br><br>Each <b>HAMMER</b> attack applies a <i>Frost Stack</i>, and once you get 3, you'll freeze an enemy, stopping them from acting their next turn!<br><br>Use attacks and special skill cards to keep your enemy frozen, so you don't have to end up cold, on the ground... I mean dead, don't end up dead.";
            typewriterText.gameObject.SetActive(true);
            video.clip = (VideoClip)Resources.Load("WeaponAssets/Hammer Demo");
            video.targetTexture = (RenderTexture)Resources.Load("WeaponAssets/HammerTex");
            rawImage.texture = video.targetTexture;
        }
        video.Play();
    }
}
