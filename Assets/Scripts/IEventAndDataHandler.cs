using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventAndDataHandler
{
    #region Data handling
    public Dictionary<string, object> DataDictionary { get; set; }
    public Dictionary<string, Action> EventDictionary { get; set; }

    public void SetValue<T>(string key, T value)
    {
        if(EventDictionary.Count > 0)
        {
            if (EventDictionary.ContainsKey("Get_" + key) != false)
                TriggerEvent("Get_" + key);
        }

        if (DataDictionary.ContainsKey(key))
            DataDictionary[key] = value;

        else
            DataDictionary.Add(key, value);
    }
    public T GetValue<T>(string key)
    {
        if (EventDictionary.ContainsKey("Set_" + key))
            TriggerEvent("Set_" + key);

        if (DataDictionary.ContainsKey(key))
            return (T)DataDictionary[key];

        else
            Debug.LogError($"The key ({key}) doesn't exist in data dictionary.");

        return default(T); // Return default value if key doesn't exist.
    }
    #endregion

    #region Event handling
    

    public void AddEvent(string eventName, Action eventAction)
    {
        if (EventDictionary.ContainsKey(eventName))
            EventDictionary[eventName] += eventAction;

        else
            EventDictionary.Add(eventName, eventAction);
    }

    public void RemoveEvent(string eventName, Action eventAction)
    {
        if (EventDictionary.ContainsKey(eventName))
            EventDictionary[eventName] -= eventAction;
    }

    public void TriggerEvent(string eventName)
    {
        if (EventDictionary.ContainsKey(eventName))
        {
            if (EventDictionary[eventName].GetInvocationList().Length > 0)
                EventDictionary[eventName]?.Invoke();

            else
                Debug.LogWarning($"The invocation list for '{eventName}' is empty.");
        }

        else
            Debug.LogWarning($"A script does not define an event for '{eventName}'");
    }
    #endregion
}
