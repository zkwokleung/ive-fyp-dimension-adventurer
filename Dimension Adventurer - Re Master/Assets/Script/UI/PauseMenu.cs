using DimensionAdventurer.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DimensionAdventurer.UI
{
	public class PauseMenu : MonoBehaviour
	{

		#region Button Click
		public void OnBtnResumeClick()
		{
			GameManager.Paused = false;
		}

		public void OnBtnMainMenuClick()
		{
			MessageBox.Show("Return to main menu?", "Main Menu", MessageBoxButtons.YesNo, dr =>
			{
				if(dr == DialogResult.Yes)
				{
					if (GamePreference.IsMultiplayer)
						Launcher.singleton.Disconnect();
					else
						SceneManager.LoadSceneAsync("MainMenu");
				}
			});
		}

		public void OnBtnQuitClick()
		{
			MessageBox.Show("Quit?", "Quit", MessageBoxButtons.YesNo, dr =>
			{
				if(dr == DialogResult.Yes)
					Application.Quit();
			});
		}
		#endregion
	}
}