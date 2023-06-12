using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventAndDataHandler
{
    #region Data handling
    public Dictionary<string, object> dataDictionary { get; }

    public void SetValue<T>(string key, T value)
    {
        if (eventDictionary.ContainsKey("Get_" + key))
            TriggerEvent("Get_" + key);

        if (dataDictionary.ContainsKey(key))
            dataDictionary[key] = value;

        else
            dataDictionary.Add(key, value);
    }
    public T GetValue<T>(string key)
    {
        if (eventDictionary.ContainsKey("Set_" + key))
            TriggerEvent("Set_" + key);

        if (dataDictionary.ContainsKey(key))
            return (T)dataDictionary[key];

        else
            Debug.LogError($"The key ({key}) doesn't exist in data dictionary.");

        return default(T); // Return default value if key doesn't exist.
    }
    #endregion

    #region Event handling
    public Dictionary<string, Action> eventDictionary { get; }

    public void AddEvent(string eventName, Action eventAction)
    {
        if (eventDictionary.ContainsKey(eventName))
            eventDictionary[eventName] += eventAction;

        else
            eventDictionary.Add(eventName, eventAction);
    }

    public void RemoveEvent(string eventName, Action eventAction)
    {
        if (eventDictionary.ContainsKey(eventName))
            eventDictionary[eventName] -= eventAction;
    }

    public void TriggerEvent(string eventName)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            if (eventDictionary[eventName].GetInvocationList().Length > 0)
                eventDictionary[eventName]?.Invoke();

            else
                Debug.LogWarning($"The invocation list for '{eventName}' is empty.");
        }

        else
            Debug.LogWarning($"A script does not define an event for '{eventName}'");
    }
    #endregion
}
