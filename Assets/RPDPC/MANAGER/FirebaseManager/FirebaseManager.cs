using Firebase;
using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private const string firebase_url = "https://rpdpc-a1f69-default-rtdb.europe-west1.firebasedatabase.app/";
    private string GetURL(string url) => firebase_url + url + ".json";

    public Action OnFirebaseInitialized;

    public void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => 
        {
            if(task.Exception != null)
            {
                Debug.LogError($"Failed to init Firebase with {task.Exception}");
                return;
            }

            OnFirebaseInitialized?.Invoke();
        });
    }

    //private void AddUser(string name, float x, float y, float z)
    //{
    //    var user = new UserData(name, x, y, z);
    //    RestClient.Put(GetURL($"USER/{name}"), user);
    //}

    //private async Task<UserData> GetUserData(string user)
    //{
    //    bool isDone = false;
    //    int maxIteration = 1000;
    //    int currentIteration = 0;

    //    RestClient.Get<UserData>(GetURL($"USER/{user}")).Then(reponse =>
    //    {
    //        isDone = true;
    //        return reponse;
    //    });

    //    while (!isDone && currentIteration < maxIteration)
    //    {
    //        currentIteration++;
    //        await Task.Delay(10);
    //    }

    //    await Task.Delay(10);
    //    if (!isDone)
    //    {
    //        Debug.LogError("Data is not receive");
    //    }
    //    return new("", 0, 0, 0);
    //}
}

public class UserData
{
    public string name;
    public float x;
    public float y;
    public float z;

    public UserData(string name, float x, float y, float z)
    {
        this.name = name;
        this.x = x;
        this.y = y;
        this.z = z;
    }
}