using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbulanceLights : MonoBehaviour
{
    [SerializeField] GameObject[] redLights;
    [SerializeField] GameObject[] whiteLights;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FlashingLights());
    }

    IEnumerator FlashingLights()
    {
        while (true)
        {
            foreach (GameObject r in redLights)
            {
                r.SetActive(true);
            }

            foreach (GameObject w in whiteLights)
            {
                w.SetActive(false);
            }

            yield return new WaitForSeconds(1f);

            foreach (GameObject r in redLights)
            {
                r.SetActive(false);
            }

            foreach (GameObject w in whiteLights)
            {
                w.SetActive(true);
            }

            yield return new WaitForSeconds(1f);
        }

    }
}
