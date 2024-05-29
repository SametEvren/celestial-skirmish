using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private FirebaseDataManager _firebaseDataManager;
        private UIManager _uiManager;
        private string characterID = "Character001";
        private Sprite characterSprite;

        [Inject]
        public void Construct(FirebaseDataManager firebaseDataManager, UIManager uiManager)
        {
            _firebaseDataManager = firebaseDataManager;
            _uiManager = uiManager;
        }

        async void Start()
        {
            // Firebase'in başlatılmasını bekle
            _firebaseDataManager.OnFirebaseInitialized += HandleFirebaseInitialized;
        }

        private async void HandleFirebaseInitialized()
        {
            Character character = await _firebaseDataManager.GetCharacterDataAsync(characterID);
            if (character != null)
            {
                Debug.Log($"Character Name: {character.Name}, Health: {character.Health}, Image URL: {character.ImageUrl}");
                characterSprite = await LoadImageFromURL(character.ImageUrl);
                if (characterSprite != null)
                {
                    _uiManager.SetFirstSelectedHero(characterSprite, character.Name, character.Health.ToString());
                }
            }
        }

        private async Task<Sprite> LoadImageFromURL(string imageUrl)
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
            {
                var asyncOp = request.SendWebRequest();

                while (!asyncOp.isDone)
                {
                    await Task.Yield();
                }

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Error downloading image: {request.error}");
                    return null;
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(request);
                    if (texture != null)
                    {
                        Debug.Log("Image successfully loaded.");
                        return TextureToSprite(texture);
                    }
                    return null;
                }
            }
        }

        private Sprite TextureToSprite(Texture2D texture)
        {
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            return Sprite.Create(texture, rect, pivot);
        }
    }
}
