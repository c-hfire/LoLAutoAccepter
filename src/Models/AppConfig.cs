using LoLAutoAccepter.Utilities;
using System.Text.Json;

namespace LoLAutoAccepter.Models
{
    public class AppConfig
    {
        /// <summary>���������@�\�̗L��/����</summary>
        public bool AutoAcceptEnabled { get; set; } = true;
        /// <summary>�����܂ł̒x���b��</summary>
        public int AcceptDelaySeconds { get; set; } = 0;
        /// <summary>Windows�N�����Ɏ����N�����邩</summary>
        public bool StartWithWindows { get; set; } = false;
        /// <summary>������A�v���������I�����邩</summary>
        public bool AutoCloseOnAccept { get; set; } = false;
        /// <summary>Discord Rich Presence�̗L��/����</summary>
        public bool DiscordRpcEnabled { get; set; } = true;
        /// <summary>League of Legends�̃C���X�g�[���f�B���N�g��</summary>
        public string LeagueOfLegendsDirectory { get; set; } = @"C:\Riot Games\League of Legends";
        /// <summary>�����o���@�\�̗L��/����</summary>
        public bool AutoBanEnabled { get; set; } = false;
        /// <summary>�����o���Ώۃ`�����s�I��ID�i���[���ʁj</summary>
        //public int? AutoBanChampionId { get; set; }
        public int? AutoBanChampionIdTop { get; set; }
        public int? AutoBanChampionIdJungle { get; set; }
        public int? AutoBanChampionIdMid { get; set; }
        public int? AutoBanChampionIdAdc { get; set; }
        public int? AutoBanChampionIdSupport { get; set; }
        /// <summary>�����s�b�N�@�\�̗L��/����</summary>
        public bool AutoPickEnabled { get; set; } = false;
        /// <summary>�����s�b�N�Ώۃ`�����s�I��ID�i���[���ʁj</summary>
        public int? AutoPickChampionIdTop { get; set; }
        public int? SubPickChampionIdTop { get; set; }
        public int? AutoPickChampionIdJungle { get; set; }
        public int? SubPickChampionIdJungle { get; set; }
        public int? AutoPickChampionIdMid { get; set; }
        public int? SubPickChampionIdMid { get; set; }
        public int? AutoPickChampionIdAdc { get; set; }
        public int? SubPickChampionIdAdc { get; set; }
        public int? AutoPickChampionIdSupport { get; set; }
        public int? SubPickChampionIdSupport { get; set; }

        /// <summary>
        /// �ݒ�t�@�C���ۑ��f�B���N�g���̃p�X���擾���܂��B
        /// </summary>
        private static string ConfigDir =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LAA");

        /// <summary>
        /// �ݒ�t�@�C���̃p�X���擾���܂��B
        /// </summary>
        private static string ConfigPath => Path.Combine(ConfigDir, "config.json");

        /// <summary>
        /// �ݒ�t�@�C����ǂݍ��݂܂��B���s���̓f�t�H���g�l��Ԃ��܂��B
        /// </summary>
        /// <returns>AppConfig�C���X�^���X</returns>
        public static AppConfig Load()
        {
            try
            {
                if (!File.Exists(ConfigPath))
                    return new AppConfig();

                string json = File.ReadAllText(ConfigPath);
                var config = JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();

                // �ݒ�l�̃o���f�[�V����
                config.AcceptDelaySeconds = Math.Clamp(config.AcceptDelaySeconds, 0, 10);

                return config;
            }
            catch (Exception ex)
            {
                Logger.Write($"�ݒ�t�@�C���̓ǂݍ��݃G���[: {ex.Message}");
                return new AppConfig();
            }
        }

        /// <summary>
        /// �ݒ�t�@�C����ۑ����܂��B
        /// </summary>
        public void Save()
        {
            try
            {
                EnsureConfigDirectory();
                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigPath, json);
            }
            catch (Exception ex)
            {
                Logger.Write($"�ݒ�t�@�C���̕ۑ��G���[: {ex.Message}");
            }
        }

        /// <summary>
        /// �ݒ�t�@�C���ۑ��f�B���N�g�����m�F���A���݂��Ȃ��ꍇ�͍쐬���܂��B
        /// </summary>
        private static void EnsureConfigDirectory()
        {
            if (!Directory.Exists(ConfigDir))
                Directory.CreateDirectory(ConfigDir);
        }
    }
}