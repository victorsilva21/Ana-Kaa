using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterCutscene : MonoBehaviour
{
    [SerializeField] string cutscene;
    [SerializeField] bool animate;

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.HasKey(cutscene))
        {
            Destroy(gameObject);
        }
    }
}
