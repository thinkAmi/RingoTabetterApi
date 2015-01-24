RingoTabetterAPI
================

以下の2つのプロジェクトを含むC# ソリューションアプリです。

- RingoTabetterTask
  - Twitterにて`[リンゴ]`で始まるツイートに含まれるリンゴ名を集計し、データベースへと保存

- RingoTabetterApi
  - データベースに保存されいてる集計情報をJSONの形で返すAPI

　  
OWIN, SelfHost を使ったNancyで動作しています。

また、このAPIの結果を元にHighchartsで表示するアプリのソースコードはこちらです。  
[thinkAmi/RingoTabetter · GitHub](https://github.com/thinkAmi/RingoTabetter)

また、Heroku上で稼働しているAPIはこちらです。  
[りんごたべたーAPI - Heroku](http://ringo-tabetter-api.herokuapp.com/)

　  
# 開発環境
- Windows7 x64
- .NET Framework 4.5
- PostgreSQL x64 9.3.5
- Ruby2.1.5 (RubyInstaller)

　  
# アプリで使っているもの
## Heroku 
### buildpack
- [heroku/heroku-buildpack-multi](https://github.com/heroku/heroku-buildpack-multi)
- [friism/heroku-buildpack-mono](https://github.com/friism/heroku-buildpack-mono)
- [heroku/heroku-buildpack-ruby](https://github.com/heroku/heroku-buildpack-ruby)

　  
### addon
- Heroku Postgres
- Heroku Scheduler

　  
## NuGet

|名前|バージョン|役割|
|---|---|---|
|Nancy.Owin|0.23.2|Nancy + OWIN + SelfHost で使う|
|Microsoft.Owin.Hosting|3.0.0|同上|
|Microsoft.Owin.Host.HttpListener|3.0.0|同上|
|Npgsql|2.2.3|Heroku Postgres用のアダプタ|
|CoreTweet|0.4.0|ツイートの収集|
|Dapper|1.38|Micro-ORM|
|Dapper.FluentMap|1.3.3|Dapperでsnake_caseなDBをCamelCaseなクラスにマッピング|
|FluentMigrator|1.3.1.0|DBのマイグレーション|
|FluentMigrator.Tools|1.3.1.0|FluentMigratorをコマンドライン実行|
|YamlDotNet|3.5.0|リンゴ品種ファイルや、設定ファイルの読み込み(ローカルのみ)|

　  
## Ruby Gem

|名前|バージョン|役割|
|---|---|---|
|Albacore|2.3.15|HerokuでFluentMigratorを実行|
|heroku_san|4.3.2|Herokuへのデプロイ管理|

　  
# セットアップ
## ローカル
### Ruby gemのインストール
```
bundle install --path vendor/bundle
```

　  
### PostgreSQLのセットアップ
Database名`ringotabetter`、ユーザIDとパスワードはともに`postgres`でデータベースを作成します。

その後、以下のコマンドでAlbacore + Rakefileを使ってデータベースのマイグレーションを行います。
```
bundle exec rake db:migrate
```

なお、PostgreSQLの設定を修正するには、[Rakefile]の先頭(https://github.com/thinkAmi/RingoTabetterApi/blob/eed79802478eefefa681aa1dd11ddc0e57a57319/Rakefile#L3)にある`LOCAL_...`という定数を修正します。  

　  
### Twitterのセットアップ
[Twitter Developers](https://dev.twitter.com/)より、各種トークンを取得します。

その値を[twitter_settings.example.yaml](https://github.com/thinkAmi/RingoTabetterApi/blob/master/RingoTabetterTask%2Ftwitter_settings.example.yaml)に入力し、`twitter_settings.yaml`へと名前を変更します。

　  
### 動作するポートの確認
ポートは`9876`になります。

ポートを変更する場合は[`Program.cs`のこのあたり](https://github.com/thinkAmi/RingoTabetterApi/blob/9cae4ec8a2ae27eac95e23d9e02b21c2ddd32b29/RingoTabetterApi/Program.cs#L13)を修正してください。

　  
### 名前空間予約の構成の追加
Self Hostingの場合、ローカルで実行するには名前空間予約の構成の追加が必要になります。  
[HTTP および HTTPS の構成 - MSDN](http://msdn.microsoft.com/ja-jp/library/ms733768.aspx)

そのため、PowerShellを管理者で起動し、まずは実行中のユーザーを確認します。

```
whoami
```

次に、そのユーザーに対して、パーミッションの追加を行います。<UserName>は、上記のwhoamiで取得したものをコピペすれば良いです。

```
netsh http add urlacl url=http://+:9876/ user=<UserName>
```

　  
## Heroku
ローカル開発環境で動作を確認後、Gitへコミットしておきます。その後、以下のどちらからの作業を行います。

### heroku_sanを使う場合
1. `heroku.example.yml`を`heroku.yml`へと名前を変更
2. appの名前を変更
3. `heroku login`して、
4. `heroku create <appname>`か、`bundle exec rake all heroku:create` + `bundle exec rake all heroku:remotes`でアプリを作成
5. `bundle exec rake production deploy`でHerokuへデプロイ

詳しくは以下の記事に書きました。  
[C# + Herokuで heroku_san を使って、環境変数・アドオンの追加とFluentMigratorの実行を行う - メモ的な思考的な](http://thinkami.hatenablog.com/entry/2015/01/16/062812)

　  
### 手動で行う場合
`heroku.example.yml`に含まれる環境変数やアドオンをherokuコマンドで追加し、Herokuへデプロイします。

　  
# ライセンス
MIT

　  
# ブログ記事
- [C# + Nancy (OWIN, SelfHost, SSVE) + Dapper + Heroku + Highchartsで食べたリンゴの割合をグラフ化してみた - メモ的な思考的な](http://thinkami.hatenablog.com/entry/2015/01/14/064115)
