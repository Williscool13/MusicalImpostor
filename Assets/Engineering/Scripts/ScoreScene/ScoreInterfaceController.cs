using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreInterfaceController : MonoBehaviour
{
    [SerializeField] ScriptableVariable_Bool isVictorious;
    [SerializeField] ScriptableVariable_BlenderVictim blenderVictim;
    [SerializeField] ScriptableVariable_Float timeLeft;
    [SerializeField] float growTime = 1.0f;
    [SerializeField] float rotSpeed = 2f;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip spin;
    [SerializeField] AudioClip thump;

    [SerializeField] AudioSource stingSource;
    [SerializeField] AudioClip victorySting;
    [SerializeField] AudioClip defeatSting;

    [SerializeField] GameObject newspaper;

    [SerializeField] TextMeshProUGUI newspaperName;
    [SerializeField] TextMeshProUGUI newspaperHeadline;
    [SerializeField] TextMeshProUGUI newspaperBody;
    [SerializeField] TextMeshProUGUI scoreLetter;
    [SerializeField] Image newspaperPicture;

    [SerializeField] TextBobbing playAgainText;
    float baseZRot;

    void Start() {
        baseZRot = newspaper.transform.rotation.eulerAngles.z;
        SetNewspaperTexts();
        newspaper.transform.localScale = new Vector3(0, 0, 0);

        //newspaperName.transform.localScale = new Vector3(0, 0, 0);
        newspaperHeadline.transform.localScale = new Vector3(0, 0, 0);
        newspaperBody.transform.localScale = new Vector3(0, 0, 0);
        newspaperPicture.transform.localScale = new Vector3(0, 0, 0);
        scoreLetter.transform.localScale = new Vector3(0, 0, 0);
    }

    [ContextMenu("Show News")]
    public void ShowNews() {
        StartCoroutine(DisplayNewspaper());
    }


    IEnumerator DisplayNewspaper() {
        float scale = 0;

        stingSource.Stop();
        stingSource.clip = isVictorious.Value ? victorySting : defeatSting;
        stingSource.Play();

        audioSource.Stop();
        audioSource.clip = spin;
        audioSource.loop = true;
        audioSource.Play();
        while (scale < 1) {
            scale += Time.deltaTime / growTime;
            newspaper.transform.localScale = new Vector3(scale, scale, scale);
            newspaper.transform.rotation = Quaternion.Euler(0, 0, newspaper.transform.rotation.eulerAngles.z + 360 * Time.deltaTime * rotSpeed);
            yield return null;
        }
        newspaper.transform.localScale = new Vector3(1, 1, 1);

        while (Mathf.Abs(newspaper.transform.rotation.eulerAngles.z - baseZRot) > 0.1f) {
            float nextZ = Mathf.MoveTowardsAngle(newspaper.transform.rotation.eulerAngles.z, baseZRot, 360 * Time.deltaTime * rotSpeed);
            newspaper.transform.rotation = Quaternion.Euler(0, 0, nextZ);
            yield return null;
        }

        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = thump;
        audioSource.Play();

        WaitForSeconds wait = new(1f);
        //yield return wait;
        //yield return StartCoroutine(SpinGrow(newspaperName.transform));
        yield return wait;
        yield return StartCoroutine(SpinGrow(newspaperHeadline.transform));
        yield return wait;
        yield return StartCoroutine(SpinGrow(newspaperBody.transform));
        yield return wait;
        yield return StartCoroutine(SpinGrow(newspaperPicture.transform));
        yield return wait;
        yield return StartCoroutine(SpinGrow(scoreLetter.transform));

        yield return wait;
        playAgainText.StartBobbing();
        scoreDone = true;

    }

    IEnumerator SpinGrow(Transform targetTransform) {

        float scale = 0;
        while (scale < 1) {
            scale += Time.deltaTime / 0.33f;
            targetTransform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = thump;
        audioSource.Play();

    }

    void SetNewspaperTexts() {
        int _vic = 1;
        if (!isVictorious.Value) {
            if (blenderVictim.Value == FruitType.Banana) {
                _vic = 2;
            }
            else {
                _vic = 3;
            }
        }

        // 1 = win
        // 2 = lose time
        // 3 = lose guess


        // set newspaper name
        // set headline
        // set body
        // set picture
        SetNewspaperInfo(_vic);
    }

    string[] newspaperNames = new string[] {
        "The Grapevine Gazette",
        "The Banana Bunch",
        "The Orange Observer",
        "The Daily Apple",
        "The Pear Press",
    };


    string[] newspaperHeadlinesWin = new string[] {
        "Detective Found the Impostor!",
        "Impostor Among Us Uncovered!",
    };
    string[] newspaperHeadlinesLoseTime = new string[] {
        "Detective Not Fast Enough!",
        "Detective Too Slow!",
    };
    string[] newspaperHeadlinesLoseGuess = new string[] {
        "Innocent Fruit Sentenced to Death!",
        "Impostor Escapes Justice!",
        "Detective Loses All Credibility",
    };
    [SerializeField] Sprite victorySprite;
    [SerializeField] Sprite defeatSpriteGuess;
    [SerializeField] Sprite defeatSpriteTimeout;
    void SetNewspaperInfo(int _vic) {
        newspaperName.text = newspaperNames[Random.Range(0, newspaperNames.Length)];


        switch (_vic) {
            case 1:
                newspaperHeadline.text = newspaperHeadlinesWin[Random.Range(0, newspaperHeadlinesWin.Length)];
                newspaperBody.text =
                    "Incredibly, a detective, on a mission to solve a musical mystery, uncovered a hidden impostor within the orchestra!\n" +
                    "\n" +
                    "\"Being blendered is quite a way to go, but he totally deserved it!\"\n" +
                    " - Anonymous Audience Member"; ;
                newspaperPicture.sprite = victorySprite;
                scoreLetter.text = "Score:\n" + GetWinningScore(timeLeft.Value);
                break;
            case 2:
                newspaperHeadline.text = newspaperHeadlinesLoseTime[Random.Range(0, newspaperHeadlinesLoseTime.Length)];
                newspaperBody.text =
                    "Inside the scandal of the century, a detective called to unravel a musical mystery is uncovered as the unexpected perpetrator!\n" +
                    "\n" +
                    "\"They didn't accuse anyone, so we decided to throw them in the blender!\"\n" +
                    " - The Police Commissioner";
                newspaperPicture.sprite = defeatSpriteTimeout;
                scoreLetter.text = "Score:\nF";
                break;
            case 3:
                newspaperHeadline.text = newspaperHeadlinesLoseGuess[Random.Range(0, newspaperHeadlinesLoseGuess.Length)];
                newspaperBody.text = 
                    "A detective, assigned to unmask the orchestral infiltrator, made an unusual mistake, resulting in the unfortunate and violent passing of an innocent orchestra member.\n" +
                    "\n" +
                    "\"Oops\"\n" +
                    " - The Detective";
                newspaperPicture.sprite = defeatSpriteGuess;
                scoreLetter.text = "Score:\nF-";
                break;
            default:
                Debug.LogError("Not legal");
                break;

        }

    }
    string GetWinningScore(float timeleft) {
        if (timeleft > 10) {
            return "WOW!";
        }
        if (timeleft > 5) {
            return "A++";
        }
        if (timeleft > 4) {
            return "A+";
        }
        if (timeleft > 3) {
            return "A-";
        }
        if (timeleft > 2) {
            return "B+";
        }
        if (timeleft > 1) {
            return "B-";
        }
        if (timeleft > 0) {
            return "C";
        }
        return "C-";
    }

    bool scoreDone = false;
    bool transitioningOut = false;
    public void OnInteract() {
        if (!scoreDone) { return; }
        if (transitioningOut) { return; }
        transitioningOut = true;
        gameRestart.Raise(null);
        StartCoroutine(CanvasExit());
    }

    [SerializeField] ScriptableGameEvent_Null gameRestart;
    IEnumerator CanvasExit() {
        float scale = 1;
        while (scale > 0) {
            scale -= Time.deltaTime / growTime * 4;
            newspaper.transform.localScale = new Vector3(scale, scale, scale);
            newspaper.transform.rotation = Quaternion.Euler(0, 0, newspaper.transform.rotation.eulerAngles.z + 360 * Time.deltaTime * rotSpeed);
            yield return null;
        }
        newspaper.transform.localScale = new Vector3(0, 0, 0);
        SceneTransitioner.Instance.TransitionScene("MainScene");

    }

}
