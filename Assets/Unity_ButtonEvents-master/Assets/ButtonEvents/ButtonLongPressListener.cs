/*--------------------------------------
   Email  : hamza95herbou@gmail.com
   Github : https://github.com/herbou
----------------------------------------*/

using System;
using System.Collections;
using UnityEngine ;
using UnityEngine.Events ;
using UnityEngine.EventSystems ;
using UnityEngine.UI ;


[RequireComponent(typeof(Button))]
public class ButtonLongPressListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    [Tooltip("Hold duration in seconds")]
    [Range(0.3f, 5f)] public float holdDuration = 0.5f;
    public UnityEvent onLongPress;

    private bool isPointerDown = false;
    private bool isLongPressed = false;
    private DateTime pressTime;

    private Button button;

    private WaitForSeconds delay;

    public float myTime;
    private void Awake() {
        button = GetComponent<Button>();
        delay = new WaitForSeconds(0.1f);
    }
    private void Update()
    {
        double elapsedSeconds = (DateTime.Now - pressTime).TotalSeconds;

        if (isPointerDown)
        {
            myTime = (float)elapsedSeconds;
        }
    }
    public void OnPointerDown(PointerEventData eventData) {
        if (TeamController.instance.ActualPlayer.ball != null)
        {
            TeamController.instance.ActualPlayer.Kicking = true;
        }
            isPointerDown = true;
        pressTime = DateTime.Now;
        StartCoroutine(Timer());
    }


    public void OnPointerUp(PointerEventData eventData) {
        if(TeamController.instance.ActualPlayer.ball != null)
        {
            TeamController.instance.ActualPlayer.Kick();
        }
        isPointerDown = false;
        isLongPressed = false;
    }

    private IEnumerator Timer() {
        while (isPointerDown && !isLongPressed) {
            double elapsedSeconds = (DateTime.Now - pressTime).TotalSeconds;

            if (elapsedSeconds >= holdDuration) {
                isLongPressed = true;
                if (button.interactable)
                    onLongPress?.Invoke();

                yield break;
            }

            yield return delay;
        }
    }
}
