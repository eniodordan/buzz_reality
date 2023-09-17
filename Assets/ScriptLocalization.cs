using UnityEngine;

namespace I2.Loc
{
	public static class ScriptLocalization
	{

		public static string Attempts 		{ get{ return LocalizationManager.GetTranslation ("Attempts"); } }
		public static string Settings 		{ get{ return LocalizationManager.GetTranslation ("Settings"); } }
		public static string Select_Level 		{ get{ return LocalizationManager.GetTranslation ("Select Level"); } }
		public static string SFX_Volume 		{ get{ return LocalizationManager.GetTranslation ("SFX Volume"); } }
		public static string Right 		{ get{ return LocalizationManager.GetTranslation ("Right"); } }
		public static string Reset_Progress 		{ get{ return LocalizationManager.GetTranslation ("Reset Progress"); } }
		public static string Paused 		{ get{ return LocalizationManager.GetTranslation ("Paused"); } }
		public static string Music_Volume 		{ get{ return LocalizationManager.GetTranslation ("Music Volume"); } }
		public static string Level_Completed 		{ get{ return LocalizationManager.GetTranslation ("Level Completed"); } }
		public static string Level 		{ get{ return LocalizationManager.GetTranslation ("Level"); } }
		public static string Left 		{ get{ return LocalizationManager.GetTranslation ("Left"); } }
		public static string Language 		{ get{ return LocalizationManager.GetTranslation ("Language"); } }
		public static string English 		{ get{ return LocalizationManager.GetTranslation ("English"); } }
		public static string Elapsed_Time 		{ get{ return LocalizationManager.GetTranslation ("Elapsed Time"); } }
		public static string Dominant_Hand 		{ get{ return LocalizationManager.GetTranslation ("Dominant Hand"); } }
		public static string Croatian 		{ get{ return LocalizationManager.GetTranslation ("Croatian"); } }
	}

    public static class ScriptTerms
	{

		public const string Attempts = "Attempts";
		public const string Settings = "Settings";
		public const string Select_Level = "Select Level";
		public const string SFX_Volume = "SFX Volume";
		public const string Right = "Right";
		public const string Reset_Progress = "Reset Progress";
		public const string Paused = "Paused";
		public const string Music_Volume = "Music Volume";
		public const string Level_Completed = "Level Completed";
		public const string Level = "Level";
		public const string Left = "Left";
		public const string Language = "Language";
		public const string English = "English";
		public const string Elapsed_Time = "Elapsed Time";
		public const string Dominant_Hand = "Dominant Hand";
		public const string Croatian = "Croatian";
	}
}