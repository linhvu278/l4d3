using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBar : MonoBehaviour
{
    public static ProgressBar instance;

    [SerializeField] TextMeshProUGUI progressBarText;
    [SerializeField] Slider progressBarSlider;

    private bool isProgressBarOn;

    void Update(){
        if (isProgressBarOn){
            progressBarSlider.value += Time.deltaTime;
            if (progressBarSlider.value >= progressBarSlider.maxValue){
                ToggleProgressBar(false);
            }
        }
    }

    public void SetProgressBar(string text, float maxValue){
        progressBarSlider.maxValue = maxValue;
        progressBarText.text = text;
    }

    public void ToggleProgressBar(bool value){
        isProgressBarOn = value;
        if (gameObject != null) gameObject.SetActive(isProgressBarOn);
    }

    void OnEnable(){
        progressBarSlider.value = 0;
    }
    
    private void OnDestroy(){
        ToggleProgressBar(false);
    }

    void Start(){
        // progressBarSlider.value = progressValue;

        isProgressBarOn = false;
        gameObject.SetActive(isProgressBarOn);
    }

    void Awake(){
        if (instance != null){
            Debug.Log("More than one instances of ProgressBar found");
            return;
        }
        instance = this;
    }
}