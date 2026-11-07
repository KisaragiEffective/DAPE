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

### 動作環境

DAPE は動作に以下のソフトウェアを必要とします。

* Neo4j 5.24
* JDK 21 (Eclipse Temurin 推奨)
* sbt 1.10.x

### 推奨開発環境

* IntelliJ IDEA (Scala Plugin)
* [Graph Database Plugin](https://plugins.jetbrains.com/plugin/20417)

### ビルド
```shell
$ sbt compile
```

### DAPE の立ち上げ

```shell
$ mkdir run
$ "$EDITOR" run/config.json
$ sbt "run --run-dir run"
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
$ sbt test
```

### fat JAR のビルド
```shell
$ sbt assembly
```

## よくある質問
[FAQ.md](./FAQ.md) を参照

## ライセンス
[LICENSE.md](./LICENSE.md) を参照

## 開発者向けの文書
[CONTRIBUTING.md](./CONTRIBUTING.md) を参照
