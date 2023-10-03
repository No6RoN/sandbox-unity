using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public GameObject AudioManager;
    public AudioClip audioClip;
    
    private PlayableDirector Director => GetComponent<PlayableDirector>();
    
    private void Awake()
    {
        
        var timeline = ScriptableObject.CreateInstance<TimelineAsset>();
        
        var _audioClip = timeline.CreateTrack<AudioTrack>().CreateClip(audioClip);

        _audioClip.asset = AudioPlayableAsset.Instantiate();
        
        Director.playableAsset = timeline;
    }

    private void Update()
    {
        
    }
}
