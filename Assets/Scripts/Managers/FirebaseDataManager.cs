using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using UnityEngine;

namespace Managers
{
    public class FirebaseDataManager : MonoBehaviour
    {
        private DatabaseReference databaseReference;
        private FirebaseStorage storage;
        private StorageReference storageReference;
        private bool isFirebaseInitialized = false;

        public event Action OnFirebaseInitialized;

        void Start()
        {
            InitializeFirebase();
        }

        private void InitializeFirebase()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result == DependencyStatus.Available)
                {
                    FirebaseApp app = FirebaseApp.DefaultInstance;
                    databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                    storage = FirebaseStorage.DefaultInstance;
                    storageReference = storage.RootReference;
                    isFirebaseInitialized = true;
                    Debug.Log("Firebase initialized successfully.");

                    OnFirebaseInitialized?.Invoke();
                }
                else
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + task.Exception);
                }
            });
        }

        public async Task<Character> GetCharacterDataAsync(string characterID)
        {
            if (!isFirebaseInitialized)
            {
                Debug.LogWarning("Firebase is not initialized yet.");
                return null;
            }

            if (databaseReference == null)
            {
                Debug.LogError("Database reference is null.");
                return null;
            }

            var characterData = await databaseReference.Child("Characters").Child(characterID).GetValueAsync();
            if (characterData.Exists)
            {
                string name = characterData.Child("CharacterName").Value.ToString();
                int health = int.Parse(characterData.Child("CharacterHealth").Value.ToString());
                string imageUrl = characterData.Child("CharacterImageURL").Value.ToString();
                return new Character(name, health, imageUrl);
            }
            else
            {
                Debug.LogWarning($"Character with ID {characterID} not found.");
                return null;
            }
        }
    }

    [Serializable]
    public class Character
    {
        public string Name;
        public int Health;
        public string ImageUrl;

        public Character(string name, int health, string imageUrl)
        {
            Name = name;
            Health = health;
            ImageUrl = imageUrl;
        }
    }

}
