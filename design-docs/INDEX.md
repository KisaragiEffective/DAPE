# design-docs

## 全体図

(To Be Documented: わかりやすく図解した画像ファイル)

## リモートサーバーから投稿が入ってきたとき

1. ブリッジサーバーからローカルサーバーあてにアクティビティペイロードが入ってくる
2. スパムフィルターを通す
3. 署名を検証する
4. ローカルサーバーに転送する
5. リモートアクターをGetOrFetchする
6. 当該アクティビティをDapeのエンティティに翻訳する
7. データベース層に書き込みを行う

Cf Queueなどを活用してバッチ処理を行う予定 - 後でちゃんと書く

## ストリームスライス

### ストリームスライスの定義

ここでいうストリームスライスとは、

1. 閲覧者がフォローしているユーザーの投稿
2. ユーザーリストに入れられているユーザーの投稿
3. (Misskey で言うところの) アンテナにマッチしたユーザーの投稿

を、それぞれ作成日時が新しい順から古い順に並べた連続部分列を指す。

要するに、各種SNSで「タイムライン」と言われて思い浮かべるものとおおよそ合致する。

ただし、Twitterやはなみすきーに存在するレコメンデーションベースの投稿アピアランスは慣例的に「タイムライン」と呼ばれているもののこの定義には含まれない。

そのようなレコメンデーションベースの投稿アピアランスは、ソートの順位が時系列にとどまらず異なった指標を用いて算出されるため、このセクションでは扱わない。

### 構築するとき

Redis の [SortedSet] を活用して [Fanout-timeline] を構築する

[SortedSet]: https://valkey.io/topics/sorted-sets/
[Fanout-timeline]: https://www.infoq.com/presentations/Twitter-Timeline-Scalability/

Aliceがノートした時、Aliceの各フォロワーのID`followee_id` に対して `timeline:home:{followee_id}` キーへ更新をかける \
→投稿時刻のUnixTimestampをスコア、投稿IDをキーとする`ZADD`コマンドを発行する

取得時は`ZREVRANGE`コマンドでとってくる

ログインユーザー1人につき、最新N件を持っておく (N <= 3000) \
→古くなったり溢れたりしたらキャッシュ削除でキャッシュがかさまないようにする \
→`ZREMRANGE` コマンドを発行し、常に1ユーザーあたりのストリームスライスの件数を一定以下に抑える。

### データベースにフォールバックすることによって生まれるパフォーマンスのデグレを防ぐための手段

#### Post ノードの created_at フィールドにインデックスを張る

```cypher
CREATE RANGE INDEX idx_note_created_at IF NOT EXISTS
FOR (e:Post) ON (e.createdAt)
```

#### lastPostedAt
これを持たせることで全ユーザー操作ではなく、インデックスを生かした操作になることが期待できる。また、統計的に最近ノートしたユーザーについてはよくノートするので、そういう状況も考えると有効な最適化だと思われる。

```cypher
CREATE RANGE INDEX idx_user_last_posted_at IF NOT EXISTS
FOR (e:User) ON (e.lastPostedAt)
```

#### キャッシュミスしたとしてもキャッシュ生成をバックグラウンドで動かして補填

SSIA

#### インメモリストアは終了前に永続化・開始前に復元

SSIA

## API設計

* HTTPを使う
* HTTPメソッドを適宜選択する
    * 取得はGETメソッドを使う
* 可能な限り、ETagとLast-Modifiedを提供する
