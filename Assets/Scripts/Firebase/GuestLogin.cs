using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
//using Facebook.Unity;
public class GuestLogin : MonoBehaviour
{

    public void OnClickGuestLogin()
    {

        FirebaseManager.Instance.GuestLogin();
    }


}