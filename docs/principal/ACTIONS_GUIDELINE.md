# GitHub Actions に関するガイドライン

## 1. GitHub hosted runner のタグとして `-latest` で終わるものを使わない

GitHub hosted runner のタグとして `windows-latest`、`ubuntu-latest`、`macos-latest` を使用してはならない。

背景: 時期によって *latest* が指すものが変化し、ビルドの再現性が容易に損なわれうるため。

解決方法: `windows-2022` 、 `ubuntu-24.04` 、 `macos-15` など、明示的にバージョンが固定されているタグを使う。

## 2. zizmor の指摘に従う

zizmor は GitHub Actions のワークフローを静的に解析するツールである。
zizmor は悪意のある入力を注入可能な箇所や、[自動的に注入されるトークン](https://docs.github.com/ja/actions/tutorials/authenticate-with-github_token)の権限が強すぎることなど[様々な有用な指摘](https://docs.zizmor.sh/audits/)をもたらす。
そのため、zizmorの指摘に従うことを**強く**推奨する。

## 3. 認証情報は Repository secret に入れる

GitHub Actions において、 `echo "$SECRET"` のような形でシークレット値をログに出力しないこと。

背景: Secrets は `::add-mask::` によってマスクされるが、無防備に扱うと漏洩リスクがある。特に、`run:` ステップで `echo` や `env:` を使ってログ出力されると GitHub 上で閲覧可能になる。

解決方法: 必要に応じて `secrets` コンテキストを使い、環境変数に渡すが、ログ出力には**絶対に含めない**ようにする。

## 4. 適切にキャッシュを使ってCIを高速化する

`actions/cache` を活用して、依存ライブラリやビルドアーティファクトをキャッシュし、CI 時間を短縮することを推奨する。

例: NuGet パッケージ、npm モジュール、Docker レイヤーなど

注意点:
- キャッシュキーにバージョン情報やチェックサムを含め、誤ったキャッシュヒットを防ぐ
- 容量制限があるため、不要なキャッシュのクリーンアップも意識する
