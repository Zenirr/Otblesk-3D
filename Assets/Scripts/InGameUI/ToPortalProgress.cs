using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToPortalProgress : MonoBehaviour
{
    [SerializeField] private Slider _progressSlider;

    private void Start()
    {
        _progressSlider = GetComponent<Slider>();
    }

    public void UpdateCurrentProgress(int createdChunks, int chunkLimit)
    {
        _progressSlider.value = (float)createdChunks / chunkLimit ;
    }

}
