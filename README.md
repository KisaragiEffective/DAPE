# DAPE

DAPE へようこそ！
DAPE は Discrete ActivityPub Environment の頭字語で、実験的な SNS バックエンドの開発プロジェクトです。現段階ではコードネームであり、仕様は今後大きく変わる可能性があります。


## 特徴
* ActivityPub を直接実装せず、内部プロトコルを他プロトコルへ翻訳するプロキシで連携
* バイナリ形式のスキーマによる小さなペイロード
* キャッシュを考慮した API 設計

## 環境構築

### 要求システム環境

DAPE は動作に以下のハードウェアを必要とします。

* Linux ディストリビューション
* ポートを開放できるマシン

### 要求ソフトウェア

DAPE は動作に以下のソフトウェアを必要とします。

* Neo4j 5.24

また、動作確認を行う場合は追加で以下のソフトウェアも必要です。

* .NET SDK 9.0.6
    * ASP.NET もインストールしてください

### 推奨開発環境

* Jetbrains Rider
    * [Graph Database](https://plugins.jetbrains.com/plugin/20417)

### ビルド
```shell
$ dotnet build
```

### DAPE の立ち上げ

```shell
KisaragiTech.Dape/KisaragiTech.Dape$ mkdir ../run
KisaragiTech.Dape/KisaragiTech.Dape$ "$EDITOR" ../run/config.json
KisaragiTech.Dape/KisaragiTech.Dape$ dotnet run -- --run-dir ../run
```

### コンフィグの例

Neo4jの初期ユーザーはすべての権限を持っているため、適宜権限を絞ったユーザーを作成することが**強く**推奨される。
```json
{
  "database": {
    "host": "localhost",
    "port": 12345,
    "user": "dape",
    "password": "dape-database-password"
  }
}
```

### テスト
```shell
$ dotnet test
```

## よくある質問
[FAQ.md](./FAQ.md) を参照

## ライセンス
[LICENSE.md](./LICENSE.md) を参照
