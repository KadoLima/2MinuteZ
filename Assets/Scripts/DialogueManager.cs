using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
//using Photon.Pun;
//using Photon.Realtime;

[System.Serializable]
public class Dialog
{
    public string charName;
    public Color charNameColor;
    [TextArea(4,4)] public string dialog;
    public bool isPlayer;
}
public class DialogueManager : MonoBehaviour
{
    [SerializeField] Image blackScreen;
    [SerializeField] GameObject waitingMessage;
    [SerializeField] GameObject chatBox;
    [SerializeField] GameObject chatBoxArrow;
    [SerializeField] TMP_Text characterNameText;
    [SerializeField] TMP_Text dialogText;
    [Space(20)]
    [SerializeField]Dialog[] openingDialogue;
    [SerializeField] Dialog finalDialog;

    int currentStep = 0;

    public static DialogueManager instance;

    //ExitGames.Client.Photon.Hashtable myCustomProperties;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        blackScreen.gameObject.SetActive(false);
        waitingMessage.SetActive(false);
    }

    private void Start()
    {
        //if (!PhotonNetwork.InRoom)
        //    return;

        //myCustomProperties = new ExitGames.Client.Photon.Hashtable();
        //myCustomProperties["Ready"] = false;
        //PhotonNetwork.LocalPlayer.SetCustomProperties(myCustomProperties);
    }

    private void Update()
    {
        if (blackScreen.gameObject.activeSelf && (currentStep < openingDialogue.Length))
        {
            if (Input.GetKeyDown(KeyCode.Return) ||
                Input.GetKeyDown(KeyCode.Space))
            {
                NextStep();
            }
        }
    }

    #region Dialogues Flow

    void FillDialogStep(int s, bool isFinal=false)
    {
        if (isFinal)
        {
            characterNameText.color = finalDialog.charNameColor;
            characterNameText.text = finalDialog.charName + ":";
            dialogText.text = finalDialog.dialog;
            dialogText.fontStyle = !openingDialogue[s].isPlayer ? FontStyles.Italic : dialogText.fontStyle = FontStyles.Normal;
            return;
        }

        characterNameText.color = openingDialogue[s].charNameColor;
        characterNameText.text = openingDialogue[s].charName+":";
        dialogText.text = openingDialogue[s].dialog;
        dialogText.fontStyle = !openingDialogue[s].isPlayer ? FontStyles.Italic : dialogText.fontStyle = FontStyles.Normal;


    }

    public void NextStep() //chamado pelo Inspector
    {
        currentStep++;

        if (currentStep < openingDialogue.Length)
            FillDialogStep(currentStep);
        else
            StartCoroutine(ChatBox_FadeOut());
    }

    public void ShowFirstOpenSequence()
    {
        StartCoroutine(FirstOpenSequence());
    }

    IEnumerator FirstOpenSequence()
    {
        FillDialogStep(0);
        chatBox.SetActive(false);
        blackScreen.gameObject.SetActive(true);
        blackScreen.GetComponent<Button>().enabled = false;
        CanvasGroup blackScreenCanvasGrp = blackScreen.GetComponent<CanvasGroup>();
        blackScreenCanvasGrp.alpha = 1;
        yield return new WaitForSeconds(0.5f);
        while (blackScreenCanvasGrp.alpha>0)
        {
            blackScreenCanvasGrp.alpha -= Time.deltaTime*.75f;
            yield return null;
        }

        StartCoroutine(ChatBox_FadeIn());
        yield return new WaitForSeconds(1);
        
        yield return new WaitUntil(() => (currentStep >= openingDialogue.Length));

        //FinishOpenDialogue();

        //yield return new WaitUntil(() => AreAllPlayersDone());

        EventsManager.OnMissionIsPlayable?.Invoke();
    }

    IEnumerator ChatBox_FadeIn()
    {
        chatBox.SetActive(true);
        CanvasGroup canvasGrp = chatBox.GetComponent<CanvasGroup>();
        canvasGrp.alpha = 0;

        while (canvasGrp.alpha <1)
        {
            canvasGrp.alpha += Time.unscaledDeltaTime*1.5f;
            yield return null;
        }
        canvasGrp.alpha = 1f;
        blackScreen.GetComponent<Button>().enabled = true;

    }

    IEnumerator ChatBox_FadeOut(float multiplier=1)
    {
        CanvasGroup canvasGrp = chatBox.GetComponent<CanvasGroup>();
        canvasGrp.alpha = 1;

        while (canvasGrp.alpha > 0)
        {
            canvasGrp.alpha -= Time.unscaledDeltaTime * 3f * multiplier;
            yield return null;
        }
        canvasGrp.alpha = 0f;
        chatBox.SetActive(false);
        blackScreen.gameObject.SetActive(false);
    }

    public void ShowFinalDialogSequence()
    {
        StartCoroutine(FinalDialogSequence());
    }

    IEnumerator FinalDialogSequence()
    {
        FillDialogStep(0, true);
        blackScreen.gameObject.SetActive(false);
        chatBoxArrow.SetActive(false);
        StartCoroutine(ChatBox_FadeIn());
        yield return new WaitForSeconds(2f);
        EnemySpawner.instance.KillAllEnemies();
        yield return new WaitForSeconds(1f);
        GameManager.instance.ReduceTimeScale();
        yield return new WaitForSeconds(3f);
        StartCoroutine(ChatBox_FadeOut(2));
        HUDManager.instance.ShowMissionCompleteScreen();

    }

    #endregion

    //#region Multiplayer Checks

    //void FinishOpenDialogue()
    //{
    //    if (!PhotonNetwork.InRoom)
    //        return;

       
    //    StartCoroutine(SetReadyOnCoop());
    //}

    //bool AreAllPlayersDone()
    //{
    //    if (!PhotonNetwork.InRoom)
    //        return true;

    //    for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
    //    {
    //        PhotonNetwork.LocalPlayer.SetCustomProperties(myCustomProperties);
    //        if ((bool)PhotonNetwork.PlayerList[i].CustomProperties["Ready"] == false)
    //        {
    //            waitingMessage.SetActive(PhotonNetwork.PlayerList.Length>1);
    //            return false;
    //        }
    //    }

    //    waitingMessage.SetActive(false);
    //    return true;
    //}

    //IEnumerator SetReadyOnCoop()
    //{
    //    if (!PhotonNetwork.InRoom)
    //        yield break;

    //    if (PhotonNetwork.InRoom)
    //    {
    //        myCustomProperties["Ready"] = true;
    //        PhotonNetwork.LocalPlayer.SetCustomProperties(myCustomProperties);
    //        //Debug.LogWarning((bool)PhotonNetwork.LocalPlayer.CustomProperties["Ready"]);
    //        yield return new WaitUntil (() => (bool)PhotonNetwork.LocalPlayer.CustomProperties["Ready"] == true);
    //    }
    //} 

    //#endregion


}
