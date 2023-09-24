using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualManager : MonoBehaviour
{

    [SerializeField] GameObject postProcessingVolumePhase1;
    [SerializeField] GameObject postProcessingVolumePhase2;
    [SerializeField] GameObject postProcessingVolumePhase3;

    [SerializeField] GameObject lightPhase1;
    [SerializeField] GameObject lightPhase2;
    [SerializeField] GameObject lightPhase3;

    float time = 0;
    [SerializeField] float phase1Duration = 48;
    [SerializeField] float phase2Duration = 50;

    [SerializeField] GameObject[] obstacles;
    GameObject currentObstacle;

    // Start is called before the first frame update
    void Start()
    {
        reset();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > phase1Duration && time < phase1Duration + phase2Duration)
        {
            setPhase2();
        }
        else if (time > phase1Duration + phase2Duration)
        {
            setPhase3();
        }
    }

    public void reset(){
        spawnObstacle();
        time = 0;
        setPhase1();
    }

    public void setPhase1(){
        postProcessingVolumePhase1.SetActive(true);
        postProcessingVolumePhase2.SetActive(false);
        postProcessingVolumePhase3.SetActive(false);

        lightPhase1.SetActive(true);
        lightPhase2.SetActive(false);
        lightPhase3.SetActive(false);
    }

    public void setPhase2(){
        postProcessingVolumePhase1.SetActive(false);
        postProcessingVolumePhase2.SetActive(true);
        postProcessingVolumePhase3.SetActive(false);

        lightPhase1.SetActive(false);
        lightPhase2.SetActive(true);
        lightPhase3.SetActive(false);
    }

    public void setPhase3(){
        postProcessingVolumePhase1.SetActive(false);
        postProcessingVolumePhase2.SetActive(false);
        postProcessingVolumePhase3.SetActive(true);

        lightPhase1.SetActive(false);
        lightPhase2.SetActive(false);
        lightPhase3.SetActive(true);
    }

    public void spawnObstacle(){
        if(currentObstacle != null){
            Destroy(currentObstacle);
        }
        currentObstacle = Instantiate(obstacles[Random.Range(0, obstacles.Length)], new Vector3(0, 0, 0), Quaternion.identity);
    }

}
