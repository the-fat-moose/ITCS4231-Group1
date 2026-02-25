using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Group1 {
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager instance;

        [Header("Player")]
        [SerializeField] private GameObject playerPrefab;

        [Header("Menu Objects")]
        [SerializeField] private GameObject titleScreenMainMenu;
        [SerializeField] private GameObject titleScreenLoadMenu;

        [Header("Buttons")]
        [SerializeField] private Button mainMenuNewGameButton;
        [SerializeField] private Button loadMenuReturnButton;
        [SerializeField] private Button mainMenuLoadGameButton;

        [Header("Pop Ups")]
        [SerializeField] private GameObject noCharacterSlotsPopUp;
        [SerializeField] private Button noCharacterSlotsOkayButton;

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SpawnPlayer()
        {
            Vector3 spawnPos = Vector3.zero;
            Quaternion spawnRot = Quaternion.identity;

            Instantiate(playerPrefab, spawnPos, spawnRot);
        }

        public void StartNewGame()
        {
            WorldSaveGameManager.instance.AttemptToCreateNewGame();
        }

        public void OpenLoadGameMenu()
        {
            // CLOSE MAIN MENU
            titleScreenMainMenu.SetActive(false);

            // OPEN LOAD MENU
            titleScreenLoadMenu.SetActive(true);

            // SELECT THE RETURN BUTTON FIRST
            loadMenuReturnButton.Select();
        }

        public void CloseLoadGameMenu()
        {
            // CLOSE LOAD MENU
            titleScreenLoadMenu.SetActive(false);

            // OPEN MAIN MENU
            titleScreenMainMenu.SetActive(true);

            // SELECT THE MAIN MENU LOAD GAME BUTTON FIRST
            mainMenuLoadGameButton.Select();
        }

        public void DisplayNoFreeCharacterSlotsPopUp()
        {
            noCharacterSlotsPopUp.SetActive(true);
            noCharacterSlotsOkayButton.Select();
        }
    
        public void CloseNoFreeCharacterSlotsPopUp()
        {
            noCharacterSlotsPopUp.SetActive(false);
            mainMenuNewGameButton.Select();
        }
    }
}