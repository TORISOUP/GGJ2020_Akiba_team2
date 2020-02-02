using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2020.Managers
{
    public class SoundManager : MonoBehaviour
    {
        static public void BGMFadeOut(AudioSource audioSource, float time)
        {
            GameObject fadeObj = new GameObject("BGMFade");
            fadeObj.AddComponent<BGMFadeOut>();
            fadeObj.GetComponent<BGMFadeOut>().Init(audioSource, time);
        }
    }
}

