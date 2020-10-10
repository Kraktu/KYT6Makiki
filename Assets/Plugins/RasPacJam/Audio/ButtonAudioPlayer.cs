using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RasPacJam.Audio
{
    [RequireComponent(typeof(Button))]
    public class ButtonAudioPlayer : MonoBehaviour
    {
        [SerializeField] private string buttonSoundName = null;
        [SerializeField] private float delay = 0f;

        public void PlaySound()
        {
            AudioManager.Instance.Play(buttonSoundName, delay);
        }
    }
}
