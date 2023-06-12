using Brad.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Actor : MonoBehaviour
{
    /*
    private NPC_Controller npcCont;

    public CharacterAction currentAction;
    public List<CharacterAction> actionList;

    public void OnEnable()
    {
        TryGetComponent<NPC_Controller>(out npcCont);

        if(npcCont != null)
        {
            npcCont.d_CurrentAction += ReturnCurrentAction;
            npcCont.E_ActionEnd += CompleteAction;
            npcCont.E_NewAction += AddAction;
        }
    }

    public void AddAction(CharacterAction newAction)
    {
        if (currentAction == null)
            currentAction = newAction;

        else
            actionList.Add(newAction);
    }

    public void CompleteAction()
    {
        if (actionList.Count > 0)
        {
            currentAction = actionList[0];
            actionList.RemoveAt(0);
        }

        else
            currentAction = null;
    }

    public CharacterAction ReturnCurrentAction()
    {
        return currentAction;
    }
    */
}
