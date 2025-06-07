# 🎮 LoL Auto Accepter

**League of Legends のマッチメイキング画面で自動的に承諾ボタンを押してくれる、タスクトレイ常駐型の Windows ツールです。**

---

## 🧩 主な機能

- ✅ 自動でマッチ承諾（`/lol-matchmaking/v1/ready-check` を使用）
- 🕒 承諾ディレイを設定可能（0/2/5/10秒など）
- 🖱 タスクトレイメニューから ON/OFF 切り替え
- 🔄 Windows 起動時に自動起動（設定可）
- 💾 設定は `config.json` に保存
- 📁 `C:\Riot Games\League of Legends` 以外のインストール先も設定可能

---


## 🔽 ダウンロードと使い方

1. [こちら](https://github.com/c-hfire/LoLAutoAccepter/releases/latest/download/LoLAutoAccepter.exe) から `.exe` をダウンロード
2. ダブルクリックで起動（初回は設定ファイルが生成されます）
3. タスクトレイから「自動承諾」を ON にして使います

---

## ⚙ 設定ファイル（`config.json`）

```json
{
  "AutoAcceptEnabled": true,
  "AcceptDelaySeconds": 2,
  "StartWithWindows": false,
  "LoLInstallPath": "C:\\Riot Games\\League of Legends"
}
```