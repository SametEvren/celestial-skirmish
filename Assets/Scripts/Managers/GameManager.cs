using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public FirebaseDataManager firebaseDatabaseManager;

        void Start()
        {
            firebaseDatabaseManager.OnFirebaseInitialized += HandleFirebaseInitialized;
        }

        private async void HandleFirebaseInitialized()
        {
            string characterID = "Character001"; 
            Character character = await firebaseDatabaseManager.GetCharacterDataAsync(characterID);
            if (character != null)
            {
                Debug.Log($"Character Name: {character.Name}, Health: {character.Health}");
            }
        }
    }
}