#--------------------------
# heroku_san settings
#--------------------------
require "bundler/setup"
begin
  require "heroku_san"
  config_file = File.join(File.expand_path(File.dirname(__FILE__)), 'config', 'heroku.yml')
  HerokuSan.project = HerokuSan::Project.new(config_file, deploy: HerokuSan::Deploy::Base)
  load "heroku_san/tasks.rb"
rescue LoadError
  # The gem shouldn't be installed in a production environment
end


task :before_deploy do
  each_heroku_app do |stage|
    # push_configでは、以下の値を環境変数に設定
    # ・引数なしは`heroku.yml`の内容、
    # ・引数ありは引数で指定した値
    stage.push_config
    stage.push_config(CONFIG_BY_RAKEFILE: :heroku_san_rakefile)

    # `heroku.yml`のアドオンを追加
    stage.install_addons
  end
end


task :after_deploy do
  each_heroku_app do |stage|
    # albacoreによるFluentMigratorの実行
    stage.run "rake", "db:migrate"
  end
end