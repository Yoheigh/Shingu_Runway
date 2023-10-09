using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;

public class AlembicPlayController : MonoBehaviour
{
    public bool isPlaying = false;

    public AlembicStreamPlayer streamPlayer;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        streamPlayer = GetComponent<AlembicStreamPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAlembicAnimation();
    }

    private void UpdateAlembicAnimation()
    {
        if(isPlaying) 
        {
            streamPlayer.CurrentTime += Time.deltaTime;
        }
    }
}
