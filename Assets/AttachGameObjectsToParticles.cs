using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(ParticleSystem))]
public class AttachGameObjectsToParticles : MonoBehaviour
{
    public GameObject m_Prefab;

    private ParticleSystem m_ParticleSystem;
    private List<GameObject> m_Instances = new List<GameObject>();
    private List<bool> lightAnimating = new List<bool>();
    private ParticleSystem.Particle[] m_Particles;
    public float maxIntensity = 6f;
    public float minStartingSize = 0.02f;
    public float maxStartingSize = 0.03f;

    // Start is called before the first frame update
    void Start()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_Particles = new ParticleSystem.Particle[m_ParticleSystem.main.maxParticles];
    }

    // Update is called once per frame
    void LateUpdate()
    {
        int count = m_ParticleSystem.GetParticles(m_Particles);

        while (m_Instances.Count < count) {
            m_Instances.Add(Instantiate(m_Prefab, transform));
            lightAnimating.Add(false);
        }

        for (int i = 0; i < m_Instances.Count; i++)
        {
            if (i < count)
            {
                m_Instances[i].transform.position = m_Particles[i].position;
                m_Instances[i].SetActive(true);
                // Debug.Log("lifetime: " + m_Particles[i].startLifetime);
                // Debug.Log("remaining: " + m_Particles[i].remainingLifetime);
                // Debug.Log("delta: " +  (m_Particles[i].startLifetime - m_Particles[i].remainingLifetime));
                var light2D = m_Instances[i].GetComponent<Light2D>();
                var fract = (m_Particles[i].startLifetime - m_Particles[i].remainingLifetime) / m_Particles[i].startLifetime;
                // * Mathf.Sin(Mathf.Lerp(0, Mathf.PI, fract));
                light2D.intensity = maxIntensity * m_Particles[i].startSize/maxStartingSize * m_Particles[i].GetCurrentSize(m_ParticleSystem)/m_Particles[i].startSize;
                // if(m_ParticleSystem.main.startLifetime.constant - m_Particles[i].remainingLifetime <= 0.2f && !lightAnimating[i]) {
                //     StartCoroutine(FadeLight(m_Instances[i], i));
                // }
                // if(!lightAnimating[i]) {
                // }
            }
            else
            {
                m_Instances[i].SetActive(false);
            }
        }
    }

    // private IEnumerator FadeLight(GameObject light, int lightIndex) {

    //     lightAnimating[lightIndex] = true;
    //     var totalLifetime = m_ParticleSystem.main.startLifetime.constant;
    //     var halfTime = (totalLifetime - .21f)/2;

    //     for(float i = 0; i < halfTime; i+= 0.1f) {
    //         light2D.intensity = startingIntensity * i / halfTime;
    //         yield return new WaitForSeconds(0.1f);
    //     }

    //     // light2D.intensity = startingIntensity;
    //     Debug.Log(totalLifetime);

    //     for(float i = 0; i < halfTime; i+= 0.1f) {
    //         light2D.intensity = startingIntensity * (halfTime - i) / halfTime;
    //         yield return new WaitForSeconds(0.2f);
    //     }

    //     lightAnimating[lightIndex] = false;
    // }
}
