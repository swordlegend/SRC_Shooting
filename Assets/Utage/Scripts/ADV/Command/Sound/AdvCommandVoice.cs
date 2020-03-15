﻿// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimurausing UnityEngine;
namespace Utage
{

	/// <summary>
	/// コマンド：ボイス再生
	/// </summary>
	internal class AdvCommandVoice : AdvCommand
	{

		public AdvCommandVoice(StringGridRow row, AdvSettingDataManager dataManager)
			: base(row)
		{
			//名前
			this.characterLabel = ParseCell<string>(AdvColumnName.Arg1);

			//ボイス
			InitVoiceFile(dataManager);
			this.isLoop = ParseCellOptional<bool>(AdvColumnName.Arg2, false);
			this.volume = ParseCellOptional<float>(AdvColumnName.Arg3, 1.0f);
		}

		public override void DoCommand(AdvEngine engine)
		{
			if (engine.Page.CheckSkip() && engine.Config.SkipVoiceAndSe) 
			{
				return;
			}
			engine.SoundManager.PlayVoice(characterLabel, voiceFile, volume, isLoop);
		}

		public override void OnChangeLanguage(AdvEngine engine)
		{
			if (!LanguageManagerBase.Instance.IgnoreLocalizeVoice)
			{
				//ボイスファイル設定
				InitVoiceFile(engine.DataManager.SettingDataManager);
			}
		}

		protected virtual void InitVoiceFile(AdvSettingDataManager dataManager)
		{
			//通常処理
			string voice = ParseCell<string>(AdvColumnName.Voice);
			voiceFile = ParseVoiceSub(dataManager, voice);
		}

		protected string characterLabel;
		protected AssetFile voiceFile;
		float volume;
		bool isLoop;
	}
}