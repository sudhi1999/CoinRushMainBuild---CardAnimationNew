using UnityEngine;
using Firebase.Database;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using System.Collections.Generic;
using System;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    DatabaseReference reference;
    FirebaseAuth auth;

    private string mPlayerNameData, mPlayerIDData, mCoinData, mEnergyData, mPlayerCurrentLevelData;

    private LevelLoadManager mLevelLoadManager;

    [SerializeField] string mLevelPrefix = "Level";


    string userTitle = "Guest Users";

    bool mIsreceivedUserData;

    bool readUserData;

    DateTime crntDateTime;

    private void Awake()
    {
        mLevelLoadManager = FindObjectOfType<LevelLoadManager>();
        auth = FirebaseAuth.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        reference.Child(userTitle).Child(auth.CurrentUser.UserId).Child("UserDetails").Child("LoginTime").RunTransaction(mutableData =>
        {
            mutableData.Value = ServerValue.Timestamp;

            return TransactionResult.Success(mutableData);
        })
        .ContinueWith(task =>
        {
            long currentTimestamp = (long)(task.Result.Value);
            crntDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(currentTimestamp / 1000d)).ToLocalTime();
            Debug.Log(crntDateTime);
        });

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); //Singleton
            return;
        }
        Destroy(this.gameObject);
    }





    void ReadDataForGuest()
    {
        reference.Child(userTitle).Child(auth.CurrentUser.UserId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                mPlayerNameData = snapshot.Child("UserDetails").Child("_playerName").Value.ToString();
                mPlayerIDData = snapshot.Child("UserDetails").Child("_playerID").Value.ToString();
                mPlayerCurrentLevelData = snapshot.Child("UserDetails").Child("_playerCurrentLevel").Value.ToString();
                mCoinData = snapshot.Child("UserDetails").Child("_coins").Value.ToString();
                mEnergyData = snapshot.Child("UserDetails").Child("_energy").Value.ToString();

                for (int i = 0; i < snapshot.Child("SaveCards").ChildrenCount; i++)
                {
                    GameManager.Instance._SavedCardTypes.Add(int.Parse(snapshot.Child("SaveCards").Child(""+i).Value.ToString()));//Get Save Card Details From Firebase
                }

                string levelName = mLevelPrefix + mPlayerCurrentLevelData;
                GameManager.Instance._buildingGameManagerDataRef.Clear();


                List<GameManagerBuildingData> BuildingDetails = new List<GameManagerBuildingData>();
                for (int i = 0; i < snapshot.Child("Buildings").Child(mLevelPrefix + mPlayerCurrentLevelData).ChildrenCount; i++)
                {

                    GameManagerBuildingData builddata = new GameManagerBuildingData();

                    builddata._buildingName = snapshot.Child("Buildings").Child(mLevelPrefix + mPlayerCurrentLevelData).Child(i.ToString()).Child("_buildingName").Value.ToString();
                    builddata._buildingCurrentLevel = int.Parse(snapshot.Child("Buildings").Child(mLevelPrefix + mPlayerCurrentLevelData).Child(i.ToString()).Child("_buildingCurrentLevel").Value.ToString());
                    builddata._isBuildingSpawned = bool.Parse(snapshot.Child("Buildings").Child(mLevelPrefix + mPlayerCurrentLevelData).Child(i.ToString()).Child("_isBuildingSpawned").Value.ToString());

                    BuildingDetails.Add(builddata);

                }

                //Difference Between  Logout Time and Login Time
                var difference = crntDateTime - DateTime.Parse(snapshot.Child("UserDetails").Child("LogOutTime").Value.ToString());
                int value = difference.Minutes;
                GameManager.Instance.UpdateUserDetails(BuildingDetails, int.Parse(mCoinData),int.Parse(mEnergyData), int.Parse(mPlayerCurrentLevelData));
                readUserData = true;
                mIsreceivedUserData = true;
            }
        });

    }

    public void WritePlayerDataToFirebase()
    {
        Player playerDetails = new Player();

        playerDetails._coins = GameManager.Instance._coins;
        playerDetails._energy = GameManager.Instance._energy;
        playerDetails._playerCurrentLevel = GameManager.Instance._playerCurrentLevel;
        string json = JsonUtility.ToJson(playerDetails);
        reference.Child(userTitle).Child(auth.CurrentUser.UserId).Child("UserDetails").SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Write Successful");
            }
        });
    }


    public void WriteBuildingDataToFirebase()
    {
        int i = 0;
        foreach (GameManagerBuildingData buildings in GameManager.Instance._buildingGameManagerDataRef)
        {

            string json = JsonUtility.ToJson(buildings);
            reference.Child(userTitle).Child(auth.CurrentUser.UserId).Child("Buildings").Child(mLevelPrefix + GameManager.Instance._playerCurrentLevel).Child(i.ToString()).SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Write Successful");
                }
            });
            i++;
        }
    }


    public void WriteCardDataToFirebase()
    {
        
        reference.Child(userTitle).Child(auth.CurrentUser.UserId).Child("SaveCards").SetValueAsync(GameManager.Instance._SavedCardTypes).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Write Successful");
            }
        });
        
       
    }


    public void GuestLogin()
    {
        if (auth.CurrentUser != null)
        {
            ReadDataForGuest();
            /*WriteBuildingDataToFirebase();
            WritePlayerDataToFirebase();
            WriteCardDataToFirebase();*/
        }
        else
        {
            CreateNewUser();
            WriteBuildingDataToFirebase();

        }
    }



    public void CreateNewUser()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            FirebaseUser newUser;
            newUser = task.Result;
            Player newPlayer = new Player(newUser.UserId);
            Debug.Log(newUser.UserId);
            SaveNewUserInFirebase(newPlayer);
        });
    }


    void SaveNewUserInFirebase(Player inPlayerDataToSave)
    {
        string json = JsonUtility.ToJson(inPlayerDataToSave);
        reference.Child(userTitle).Child(auth.CurrentUser.UserId).Child("UserDetails").SetRawJsonValueAsync(json);
    }
    private void Update()
    {
        if (readUserData)
        {
            LoadToTheCurrentLevel(GameManager.Instance._playerCurrentLevel);
        }

    }

    

    //Scenemanager Job.......
    void LoadToTheCurrentLevel(int inCurrentLevelNo)
    {
        mLevelLoadManager.LoadLevelOf(inCurrentLevelNo);
        Debug.Log("Loading");
        readUserData = false;
    }





    private void OnApplicationQuit()
    {
        if (mIsreceivedUserData)
        {
            WriteBuildingDataToFirebase();
            WritePlayerDataToFirebase();
            WriteCardDataToFirebase();
            FirebaseDatabase.DefaultInstance.GetReference("Timestamp").RunTransaction(mutableData =>
            {
                mutableData.Value = ServerValue.Timestamp;

                return TransactionResult.Success(mutableData);
            })
            .ContinueWith(task =>
            {
                long currentTimestamp = (long)(task.Result.Value);
                var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(currentTimestamp / 1000d)).ToLocalTime();
                reference.Child(userTitle).Child(auth.CurrentUser.UserId).Child("UserDetails").Child("LogOutTime").SetValueAsync(dt.ToString());
            });
        }

    }


  

#if PLATFORM_ANDROID

   /* private void OnApplicationFocus(bool focus)
    {
        if (!focus&& mIsreceivedUserData)
        {
            
                WriteBuildingDataToFirebase();
                WritePlayerDataToFirebase();
            
        }
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause && mIsreceivedUserData)
        {
            WriteBuildingDataToFirebase();
            WritePlayerDataToFirebase();
        }
    }*/
#endif
}


