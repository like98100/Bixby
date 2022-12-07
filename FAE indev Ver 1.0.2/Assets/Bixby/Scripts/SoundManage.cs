using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManage : MonoBehaviour
{
    public static SoundManage instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);  // 이 오브젝트는 신이 변경되도 제거되지 않는다

        if (instance == null)    // 할당되어 있지 않다면
        {
            instance = this;
        }
        else if (instance != this)        // 현재 할당되어 있는 오브젝트가아니라면
        {
            Destroy(gameObject);        // NAGA
        }

        bgmPlayer = gameObject.AddComponent<AudioSource>();
        charSfxPlayer = gameObject.AddComponent<AudioSource>();
        charLoopSfxPlayer = gameObject.AddComponent<AudioSource>();
        systemSfxPlayer = gameObject.AddComponent<AudioSource>();
    }

    public AudioClip[] BGMClips = new AudioClip[7];   // Title, Field, FireD, IceD, WaterD, ElecD, BossD
    public AudioClip[] PlayerSFXClips;
    public AudioClip[] PlayerLoopSFXClips;
    public AudioClip[] SystemSFXClips;

    AudioSource bgmPlayer;
    AudioSource charSfxPlayer;
    AudioSource charLoopSfxPlayer;
    AudioSource systemSfxPlayer;

    int curLoopIdx;

    float BGMVolume;
    float SFXVolume;
    // Start is called before the first frame update
    void Start()
    {
        BGMVolume = 0.7f;
        SFXVolume = 1f;
        PlayBGMSound(SceneManager.GetActiveScene().name);//, 0.7f); // 배경음악 실행
        curLoopIdx = -1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayBGMSound(string fieldName)//, float volume = 1f)
    {
        bgmPlayer.Pause();

        int idx = -1;
        switch(fieldName)
        {
            case "Title":
                idx = 0;
                break;
            case "FieldScene":
                idx = 1;
                break;
            case "FireDungeon":
                idx = 2;
                break;
            case "IceDungeon":
                idx = 3;
                break;
            case "WaterDungeon":
                idx = 4;
                break;
            case "ElectricDungeon":
                idx = 5;
                break;
            case "BossDungeon":
                idx = 6;
                break;
            default:
                break;
        }

        if (idx == -1) return;

        //BGMSources[idx].loop = true;
        //BGMSources[idx].volume = volume;

        //BGMSources[idx].Play();

        bgmPlayer.loop = true;
        bgmPlayer.volume = BGMVolume;
        bgmPlayer.clip = BGMClips[idx];

        bgmPlayer.Play();

        Debug.Log(fieldName + " Bgm 출력 중");
    }

    public void PlaySFXSound(int sfxIdx, string flag)//, float SFXVolume = 1f)
    {
        // flag가 Player일 때와 System일 때 각각 다른 플레이어를 실행할 것
        switch(flag)
        {
            case "Player":
                //if (charSfxPlayer.isPlaying) break;          // 사운드가 재생중이면 패스(하던건 다 출력하고 실행하기)
                charSfxPlayer.volume = SFXVolume;
                charSfxPlayer.loop = false;
                //charSfxPlayer.PlayOneShot(PlayerSFXClips[sfxIdx]);

                charSfxPlayer.clip = PlayerSFXClips[sfxIdx];
                charSfxPlayer.Play();
                break;
            case "PlayerLoop":
                if (sfxIdx == curLoopIdx && charLoopSfxPlayer.isPlaying) break;            // 반복 출력 제거

                charLoopSfxPlayer.volume = SFXVolume;
                charLoopSfxPlayer.loop = true;
                charLoopSfxPlayer.clip = PlayerLoopSFXClips[sfxIdx];
                charLoopSfxPlayer.Play();

                curLoopIdx = sfxIdx;
                break;
            case "System":
                systemSfxPlayer.volume = SFXVolume;
                systemSfxPlayer.loop = false;
                systemSfxPlayer.PlayOneShot(SystemSFXClips[sfxIdx]);
                break;
            default:
                break;
        }

        Debug.Log(flag + "의 " + sfxIdx + " 사운드 출력");
    }

    public AudioSource GetPlayerLoopSFXPlayer()
    {
        return charLoopSfxPlayer;
    }

    public float GetVolume(bool isBGM)
    {
        if (isBGM)
            return BGMVolume;
        else
            return SFXVolume;
    }
    public void SetVolume(bool isBGM, float value)
    {
        if (isBGM)
        {
            BGMVolume = value;
            bgmPlayer.volume = BGMVolume;
        }
        else
        {
            SFXVolume = value;
            charSfxPlayer.volume = charLoopSfxPlayer.volume = systemSfxPlayer.volume = SFXVolume;
        }
    }
}
