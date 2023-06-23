using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSliderValue : MonoBehaviour
{
    public Transform target;
    public string valueKey;
    public string maxValueKey;
    public string updateEventKey;

    Image sliderFill;
    IEventAndDataHandler targHandler;

    private void Start()
    {
        sliderFill = transform.Find("Slider_Fill").GetComponent<Image>();

        if(target != null)
        {
            target.TryGetComponent(out targHandler);

            if (targHandler != null)
            {
                //Debug.Log(targHandler);
                targHandler.AddEvent(updateEventKey, UpdateSlider);
            }
        }
    }

    public void UpdateTarget(Transform newTarget)
    {
        if (targHandler != null)
            targHandler.RemoveEvent(updateEventKey, UpdateSlider);

        target = newTarget;
        target.TryGetComponent(out targHandler);
        targHandler.AddEvent(updateEventKey, UpdateSlider);
        UpdateSlider();
    }

    public void UpdateSlider()
    {
        sliderFill.fillAmount = (float)targHandler.GetValue<int>(valueKey) / (float)targHandler.GetValue<int>(maxValueKey);
    }
}
