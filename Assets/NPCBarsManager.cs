using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class NPCBarsManager : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float range;
    [SerializeField] Vector3 offset;

    RectTransform[] npcStatusBars;
    SetSliderValue[] npcSliders;
    TextMeshProUGUI[] npcTitles;

    Collider[] targetInRange;

    private void Start()
    {
        List<RectTransform> tmpRectList = new List<RectTransform>();
        List<SetSliderValue> tmpSlidList = new List<SetSliderValue>();
        List<TextMeshProUGUI> tmpTextList = new List<TextMeshProUGUI>();

        foreach(Transform child in transform)
        {
            if (child.name.Contains("NPC_StatusBars"))
            {
                tmpRectList.Add(child.GetComponent<RectTransform>());
                tmpSlidList.Add(child.GetComponentInChildren<SetSliderValue>());
                tmpTextList.Add(child.GetComponentInChildren<TextMeshProUGUI>());
            }
        }

        npcStatusBars = tmpRectList.ToArray();
        npcSliders = tmpSlidList.ToArray();
        npcTitles = tmpTextList.ToArray();
    }

    Collider[] ClosestThingsInRange()
    {
        List<Collider> tmpTargets = Physics.OverlapSphere(target.position, range, layerMask).ToList();

        if (tmpTargets.Contains(target.GetComponent<Collider>()))
            tmpTargets.Remove(target.GetComponent<Collider>());

        return tmpTargets.ToArray();
    }

    private void Update()
    {
        targetInRange = ClosestThingsInRange().OrderBy(c => (target.position - c.transform.position).sqrMagnitude).ToArray();

        for (int i = 0; i < npcStatusBars.Length; i++)
        {
            if (targetInRange.Length > 0 && i < targetInRange.Length && !targetInRange[i].GetComponent<IDamagable>().IsHealthDepleted())
            {
                if (npcSliders[i].target != targetInRange[i].transform)
                    npcSliders[i].UpdateTarget(targetInRange[i].transform);

                if (npcStatusBars[i].gameObject.activeSelf == false)
                    npcStatusBars[i].gameObject.SetActive(true);

                float distance = Vector3.Distance(Camera.main.transform.position, targetInRange[i].transform.position);
                float scale = 10 / distance;
                Vector2 screenPosition = Camera.main.WorldToScreenPoint(targetInRange[i].transform.position + offset);
                npcStatusBars[i].transform.position = screenPosition;
                npcStatusBars[i].transform.localScale = Vector3.one * scale;

                if (npcTitles[i].text != targetInRange[i].name)
                    npcTitles[i].text = targetInRange[i].name;
            }

            else
            {
                if (npcStatusBars[i].gameObject.activeSelf == true)
                    npcStatusBars[i].gameObject.SetActive(false);
            }
        }
    }
}
