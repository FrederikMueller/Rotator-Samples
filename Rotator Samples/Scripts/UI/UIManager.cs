using Sirenix.OdinInspector;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UniRx;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        //UI MANAGER
        [TabGroup("Managers", "Managers"), Required] public GameManager gameManager;
        [TabGroup("Managers", "Managers"), Required] public Canvas mainCanvas;

        [TabGroup("Objects", "Objects"), Required] public PlayerCore playerF;
        [TabGroup("Objects", "Objects"), Required] public PlayerCore playerB;
        [TabGroup("Objects", "Objects"), Required] public PlayerCore currentlyActive;
        [TabGroup("Objects", "Objects")] public GameObject pausePanel;
        [TabGroup("Objects", "Objects")] public GameObject settingsPanel;

        [TabGroup("UIElements", "UIElements")] public Text CurrentScore;
        [TabGroup("UIElements", "UIElements")] public TMP_Text stopwatchText;
        [TabGroup("UIElements", "UIElements")] public Text ammoText;
        [TabGroup("UIElements", "UIElements")] public Text hpText;

        [TabGroup("UIElements", "UIElements")] public Image topLeft;
        [TabGroup("UIElements", "UIElements")] public Image topRight;
        [TabGroup("UIElements", "UIElements")] public Image bottomLeft;
        [TabGroup("UIElements", "UIElements")] public Image bottomRight;
        [TabGroup("UIElements", "UIElements")] public Text UptimeText;
        [TabGroup("UIElements", "UIElements")] public Text[] levelTimeTxt;
        [TabGroup("UIElements", "UIElements")] public Text playernamebox;
        [TabGroup("UIElements", "UIElements")] public Text playernamePH;

        private static UIManager instance;
        private StringBuilder sb;
        // Values & States
        private bool rewinding;
        private float runTime;
        private float startHistoryTimestamp = 0f;
        private float endHistoryTimestamp = 0f;

        private Color myred;
        private Color myblue;

        private void Awake()
        {
            //highscoreText.text = ("High Score: " + gameManager.GetHighScore());
            sb = new StringBuilder();
            gameManager.TimeKeeper.GlobalRewindEvent += TurnInterfaceBlue;
            gameManager.TimeKeeper.GlobalRewindEvent += ToggleTimerDirection;

            gameManager.TimeKeeper.GlobalForwardEvent += TurnInterfaceRed;
            gameManager.TimeKeeper.GlobalForwardEvent += ToggleTimerDirection;

            myred = new Color32(152, 0, 1, 190);
            myblue = new Color32(69, 79, 255, 190);
            TurnInterfaceRed();
        }

        private void TurnInterfaceBlue()
        {
            //stopwatchText.color = Color.blue;
            topRight.color = myblue;
            bottomLeft.color = myblue;
            bottomRight.color = myblue;
            topLeft.color = myblue;
            //ammoText.color = Color.blue;
            //hpText.color = Color.blue;
        }

        private void TurnInterfaceRed()
        {
            //stopwatchText.color = Color.red;
            topRight.color = myred;
            topLeft.color = myred;
            bottomLeft.color = myred;
            bottomRight.color = myred;
            //ammoText.color = Color.red;
            //hpText.color = Color.red;
        }

        //private void Start()
        //{
        //    if (instance == null) // This is first object, set the static reference
        //    {
        //        instance = this;
        //        DontDestroyOnLoad(this.gameObject);
        //        return;
        //    }
        //    Destroy(this.gameObject);
        //}
        private void FixedUpdate()
        {
            currentlyActive = gameManager.CurrentlyActivePlayer;
            UpdateTimeDisplay();
            UpdateHPText();
            UpdateAmmoText();
        }

        private void UpdateAmmoText()
        {
            ammoText.text = currentlyActive.pOff.Ammo.ToString();
        }
        private void UpdateHPText()
        {
            hpText.text = currentlyActive.pDef.Health.ToString();
        }

        public void PauseScreen()
        {
            Cursor.visible = true;
            pausePanel.SetActive(true);
            //playernamebox.text = gameManager.playername;
        }

        internal void DisplayLevelTime(float result, int curLevel)
        {
            levelTimeTxt[curLevel - 1].text = ("Level" + curLevel + ": " + result.ToString());
            levelTimeTxt[5].enabled = true;
            levelTimeTxt[5].text = ("Level Complete\n" + "in " + result.ToString() + " seconds\n(+" + Mathf.FloorToInt(80 - result) + " pts)");
            Invoke("LevelPopUpFade", 3f);
            //CurrentScore.text = gameManager.score.ToString();

            // with RP
            // Display Popup. Start Observer with Timer. Subscribe with hide popup method or lambda it. Done.
            Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(_ => Debug.Log("yo"));
            // automaticcally disposed because of oncomplete
            // one shot time based calls are easier with RP.

            // With coroutine
            // display popup. start coroutine via generic one or specific extra one. Either inject the delegate into the coroutine or hardcode it. Done.
            StartCoroutine(DelayedAction(() => Debug.Log("yo"), 2));
        }
        private IEnumerator DelayedAction(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }

        private void UpdateTimeDisplay()
        {
            if (rewinding)
                runTime = (endHistoryTimestamp - startHistoryTimestamp) - (Time.timeSinceLevelLoad - endHistoryTimestamp);
            else
                runTime = Time.timeSinceLevelLoad - startHistoryTimestamp;

            sb.Append(runTime.ToString());
            int totalDigits = 3;

            if (runTime > 10f)
            {
                totalDigits = 4;
            }

            if (sb.Length > totalDigits)
                sb.Remove(totalDigits, sb.Length - totalDigits);

            stopwatchText.text = sb.ToString();
            sb.Clear();
        }
        public void ToggleTimerDirection()
        {
            if (rewinding)
                startHistoryTimestamp = Time.timeSinceLevelLoad;
            else
                endHistoryTimestamp = Time.timeSinceLevelLoad;

            rewinding = !rewinding;
        }

        internal void UpdateHighScore()
        {
            //highscoreText.text = ("High Score: " + gameManager.GetHighScore());
        }

        internal void UpdateScore(int score)
        {
            CurrentScore.text = score.ToString();
        }

        private void LevelPopUpFade()
        {
            levelTimeTxt[5].enabled = false;
        }

        internal void UnpauseScreen()
        {
            pausePanel.SetActive(false);
            //Cursor.visible = false;
        }

        public void Settings()
        {
            //Activate Settings Panel
            pausePanel.SetActive(false);
            settingsPanel.SetActive(true);
        }

        public void BackToMenu()
        {
            pausePanel.SetActive(true);
            settingsPanel.SetActive(false);
        }
    }
}