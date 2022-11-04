using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BGM : MonoBehaviour
{
    [SerializeField] float fadeInTime;
    [SerializeField] AudioSource audioSource;
    [SerializeField] bool isPersistent;

    private void Awake()
    {
        if (isPersistent)
            DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource.volume = 0;
        audioSource.DOFade(1, fadeInTime);
    }

    public void FadeOutAndDestroy(float fadeOutTime)
    {
        audioSource.DOFade(0, fadeOutTime + 1).OnComplete(ManualDestroy);
    }

    void ManualDestroy()
    {
        Destroy(this.gameObject);
    }


}
