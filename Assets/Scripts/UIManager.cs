using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Shared;

    #region Inspector Control

    [FormerlySerializedAs("UIText")] [Header("Stage Text")] [SerializeField]
    private TextMeshProUGUI uiText;

    [Header("Life Bar")] [SerializeField] private GameObject lifeBar;
    [SerializeField] private GameObject lifeBarDelay;

    [Header("Stage State Indicator")] [SerializeField]
    private GameObject worldStageStatus;

    [SerializeField] private GameObject indicator;

    [Header("Heart Indicator")] [SerializeField]
    private List<Sprite> frames;

    [SerializeField] private GameObject heartIndicator;
    private static readonly int LaunchTimer = Animator.StringToHash("LaunchTimer");

    #endregion


    private void Awake()
    {
        if (Shared == null)
            Shared = this;
    }

    private void Start()
    {
        lifeBar.GetComponent<Image>().fillAmount = 1f;
        lifeBarDelay.GetComponent<Image>().fillAmount = 1f;
        SetUIText();
        if (!GameManager.Shared.inTutorial)
            LaunchWorldStage();
    }

    private void Update()
    {
        SetLifeBarDelay();
    }

    public void SetBarTransparency(float a)
    {
        var opacity = lifeBar.GetComponent<SpriteRenderer>().color;
        opacity.a = a;
        lifeBar.GetComponent<SpriteRenderer>().color = opacity;

        opacity = lifeBar.GetComponent<Image>().color;
        opacity.a = a;
        lifeBar.GetComponent<Image>().color = opacity;

        opacity = lifeBarDelay.GetComponent<SpriteRenderer>().color;
        opacity.a = a;
        lifeBarDelay.GetComponent<SpriteRenderer>().color = opacity;

        opacity = lifeBarDelay.GetComponent<Image>().color;
        opacity.a = a;
        lifeBarDelay.GetComponent<Image>().color = opacity;
    }

    public void SetLifeBar(float progressPercentage)
    {
        lifeBar.GetComponent<Image>().fillAmount = progressPercentage / 100f;
        UpdateHeartIndicator(progressPercentage);
    }

    private void SetLifeBarDelay()
    {
        var lifeBarFill = lifeBar.GetComponent<Image>().fillAmount;
        var lifeBarDelayFill = lifeBarDelay.GetComponent<Image>().fillAmount;
        var barEmptyScaler = Time.timeScale != 0 ? Time.fixedDeltaTime * 0.02f : 0;

        if (!(lifeBarDelayFill > lifeBarFill)) return;
        lifeBarDelayFill = Mathf.Max(lifeBarDelayFill - barEmptyScaler, lifeBarFill);
        lifeBarDelay.GetComponent<Image>().fillAmount = lifeBarDelayFill;
    }

    public void SetStageStateBar(float progressPercentage)
    {
        var indicatorXPosition = (progressPercentage * 71) - 35.5f;
        var currPos = indicator.transform.localPosition;
        currPos.x = indicatorXPosition;
        indicator.transform.localPosition = currPos;
    }

    public void SetUIText(int roundNum = 1, int monstersLeft = 0, int monstersTotal = 100)
    {
        uiText.text = $"<u>Round {roundNum}</u> \nKill  {monstersLeft}/{monstersTotal}";
    }

    public void LaunchWorldStage()
    {
        var anim = worldStageStatus.GetComponent<Animator>();
        anim.SetTrigger(LaunchTimer);
    }

    private void UpdateHeartIndicator(float progressPercentage)
    {
        var amountOfFrames = frames.Count;
        var frame = (int) Mathf.Floor(amountOfFrames - amountOfFrames * progressPercentage / 100);
        frame = frame < amountOfFrames ? frame : amountOfFrames - 1;
        heartIndicator.GetComponent<Image>().sprite = frames[frame];
    }
}