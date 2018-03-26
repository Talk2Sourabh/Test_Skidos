using System;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

[Serializable]
public class ProgrammingLanguage
{
	public Text language_Name;
	public Text language_RegardingVote;
}
public class JsonApplication : MonoBehaviour
{
	[SerializeField]private string url;
	[SerializeField]private Text question;
	public ProgrammingLanguage[] languageDescription;
	[SerializeField]private Button btn_Refrash;
	public delegate void OnComplete();
	public event OnComplete downlodingComplete;


	void Start()
	{
		downlodingComplete += DownloadSuccessFullEvents;
	}
	public void GetJsonData()
	{
		Debug.Log("Btn_Clicked");
		StartCoroutine (StartDownload());
		btn_Refrash.interactable = false;
	}




	IEnumerator StartDownload()
	{
		UnityWebRequest www = UnityWebRequest.Get (url);
		yield return www.Send();

		if (www.isNetworkError)
		{
			Debug.Log(www.error);
		}
		else
		{
			JsonData data = JsonMapper.ToObject(www.downloadHandler.text);
			GenerateUI (data);
			downlodingComplete ();

		}

	}    
	void DownloadSuccessFullEvents()
	{
		Debug.Log("DownloadSuccessfully");
	}

	private void GenerateUI(JsonData jsonString)
	{

		question.text = jsonString [0]["question"].ToString ();

		JsonData choicesData = jsonString[0]["choices"];

		for (int i = 0; i < choicesData.Count; i++)
		{
			languageDescription[i].language_Name.text = ""+choicesData[i]["choice"];
			languageDescription [i].language_RegardingVote.text = "" + choicesData [i] ["votes"];
		}
	}

}
