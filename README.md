# TTS Voice Wizard To VOICEVOX (COEIROINK)

Simple ASP.NET Server make VOICEVOX or COEIROINK can work in TTS Voice Wizard

## Setup Instructions

1. Download [TTS Voice Wizard](https://github.com/VRCWizard/TTS-Voice-Wizard)
2. Download [VOICEVOX](https://github.com/VOICEVOX/voicevox)
   or [engine version](https://github.com/VOICEVOX/voicevox_engine)
3. (Optional) Download [COEIROINK](https://coeiroink.com/) only if you want to use
4. Change url port in `ttssettings.json`, VOICEVOX default port is `50021`, but this server default port is `50022`. if
   you
   only want use voicevox full version not this engine version, change port to the default or any port you has set.
5. Change `Text To Speech Mode` to `Locally Hosted` in TTS Voice Wizard

## Usage

- Go to http://localhost:8124/docs for more information.
- If you want to change speaker without close this server, send http post to http://localhost:8124/settings to
  change.  
  Example:
  `curl 127.0.0.1:8124/settings -H "Content-Type: application/json" -d '{"speaker_id":46,"speaker_mode":0,"speaker_uuid":"cb11bdbd-78fc-4f16-b528-a400bae1782d","speaker_style_id":90}'`  
  If you want to get speaker id form VOICEVOX or COEIROINK  
  `curl 127.0.0.1:50022/speakers` for VOICEVOX  
  `curl 127.0.0.1:50032/v1/speakers` for COEIROINK  
  I have a simple script to get speakers but for now I can't get this script
- Change `SpeakerMode` to select VOICEVOX or COEIROINK  
  `0` is VOICEVOX  
  `1` is COEIROINK

---
VOICEVOXとCOEIROINKをTTS Voice Wizardで使用できるにする簡単のASP.NETサーバー

## セットアップ方法

1. [TTS Voice Wizard](https://github.com/VRCWizard/TTS-Voice-Wizard)をダウンロードします
2. [VOICEVOX](https://github.com/VOICEVOX/voicevox)または[エンジン版](https://github.com/VOICEVOX/voicevox_engine)
   をダウンロードします
3. （オプション）[COEIROINK](https://coeiroink.com/)を好きならばダウンロードします
4. `ttssetting.json`中のセットを変更します  
   VOICEVOXのデフォルトポートは`50021`ですが、このサーバーのデフォルトポートは`50022`です  
   VOICEVOXのフルバージョンを使用する場合は、デフォルトのポートまたは設定したポートに変更してください
5. TTS Voice Wizardの`Text To Speech Mode`を`Locally Hosted`に変更します

## 使用方法

- 詳しくは [http://localhost:8124/docs](http://localhost:8124/docs) へ
- このサーバーを閉じずに`speakers`を変更したい場合は、http postを http://localhost:8124/settings
  に送って変更してください  
  サンプル
  `curl 127.0.0.1:8124/settings -H "Content-Type: application/json" -d '{"speaker_id":46,"speaker_mode":0,"speaker_uuid":"cb11bdbd-78fc-4f16-b528-a400bae1782d","speaker_style_id":90}'`  
  VOICEVOXまたはCOEIROINKからスピーカーIDを取得したい場合  
  VOICEVOXの場合は`curl 127.0.0.1:50022/speakers`  
  COEIROINKの場合は`curl 127.0.0.1:50032/v1/speakers`  
  簡単のスクリプトがありますけど、今は見つかったないです
- `SpeakerMode`を変更し、VOICEVOXまたはCOEIROINKを選択します  
  `0` はVOICEVOXです  
  `1` はCOEIROINKです

