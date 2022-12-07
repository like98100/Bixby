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
        systemSfxPlayer = gameObject.AddComponent<AudioSource>();
    }

    public AudioClip[] BGMClips = new AudioClip[7];   // Title, Field, FireD, IceD, WaterD, ElecD, BossD
    public AudioClip[] PlayerSFXClips;
    public AudioClip[] SystemSFXClips;

    AudioSource bgmPlayer;
    AudioSource charSfxPlayer;
    AudioSource systemSfxPlayer;

    // Start is called before the first frame update
    void Start()
    {
        PlayBGMSound(SceneManager.GetActiveScene().name, 0.7f); // 배경음악 실행
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void PlayBGMSound(string fieldName, float volume = 1f)
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
        bgmPlayer.volume = volume;
        bgmPlayer.clip = BGMClips[idx];

        bgmPlayer.Play();

        Debug.Log(fieldName + " Bgm 출력 중");
    }

    public void PlaySFXSound(int sfxIdx, string flag, float volume = 1f)
    {
        // flag가 Player일 때와 System일 때 각각 다른 플레이어를 실행할 것
        switch(flag)
        {
            case "Player":

                if (sfxIdx == 0 || sfxIdx == 2 || sfxIdx == 11)      // 걷거나 뛰거나 수영중일 때
                {
                    if (charSfxPlayer.clip == PlayerSFXClips[1]   // 대시 중이고
                            && charSfxPlayer.isPlaying) return;   // 사운드가 재생중이면 패스
                    charSfxPlayer.volume = volume;
                    charSfxPlayer.loop = true;
                    charSfxPlayer.clip = PlayerSFXClips[sfxIdx];
                    charSfxPlayer.Play();
                }
                else
                {
                    if ((charSfxPlayer.clip != PlayerSFXClips[0] || charSfxPlayer.clip != PlayerSFXClips[2])    // 걷거나 달리는 상태가 아니고
                        && charSfxPlayer.isPlaying) return;                                                     // 사운드가 재생중이면 패스
                    charSfxPlayer.volume = volume;
                    charSfxPlayer.loop = false;
                    charSfxPlayer.PlayOneShot(PlayerSFXClips[sfxIdx]);
                }
                break;
            case "System":
                systemSfxPlayer.volume = volume;

                systemSfxPlayer.PlayOneShot(SystemSFXClips[sfxIdx]);
                break;
            default:
                break;
        }

        Debug.Log(flag + "의 " + sfxIdx + " 사운드 출력");
    }

    public AudioSource GetPlayerSFXPlayer()
    {
        return charSfxPlayer;
    }
}
