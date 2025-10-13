using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool playBgm;
    public bool currentBgmLoop;
    private int bgmIndex;

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (!playBgm)
            StopAllBGM();
        //else
        //{
        //    if (!bgm[bgmIndex].isPlaying)
        //    {
        //        PlayBGM(bgmIndex);
        //    }
        //}
    }

    public void PlaySFX(int _sxfIndex)
    {
        if (_sxfIndex < sfx.Length)
        {
            sfx[_sxfIndex].pitch = Random.Range(.85f, 1.1f);
            sfx[_sxfIndex].Play();
        }
    }
    public void PlaySFX(int _sxfIndex, bool _sxfLoop)
    {
        if (_sxfIndex < sfx.Length)
        {
            sfx[_sxfIndex].pitch = Random.Range(.85f, 1.1f);
            sfx[_sxfIndex].Play();
        }
        sfx[_sxfIndex].loop = _sxfLoop;
    }
    public void StopSFX(int _index)
    {
        sfx[_index].loop = false;
        sfx[_index].Stop();
    }

    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;

        StopAllBGM();
        bgm[bgmIndex].Play();

        bgm[bgmIndex].loop = currentBgmLoop;
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
}
