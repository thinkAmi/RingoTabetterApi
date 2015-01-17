require 'albacore/tools/fluent_migrator'

FLUENT_MIGRATOR_VERSION = Dir::glob("packages/FluentMigrator.Tools*").first
LOCAL_RUNNER = "#{FLUENT_MIGRATOR_VERSION}/tools/AnyCPU/40/Migrate.exe"
LOCAL_TARGET = 'RingoTabetterTask\bin\Debug\RingoTabetterTask.exe'
LOCAL_PROVIDER = 'postgres'
LOCAL_CONNECTION = 'Server=127.0.0.1; Port=5432; Database=ringotabetter; User Id=postgres; Password=postgres;'

namespace :db do
  desc "migrator task"     
  task :migrator, :task do |migrator, args|
    # these could also be defined in a YML file
    raise "ERROR: :task must be defined" if args[:task].nil?

    puts runner_path
    puts target_path
    puts connection

    ns = Albacore::Tools::FluentMigrator
    cmd = ns::MigrateCmdFactory.create exe: runner_path, 
                                       dll: target_path, 
                                       db: provider, 
                                       conn: connection,
                                       direction: args[:task],
                                       interactive: false  # trueでは逐一入力が必要
    cmd.execute
  end

  namespace :rollback do
    desc "Rollback the database one iteration"
    task :default do |migrator|
      Rake::Task["db:migrator"].reenable
      Rake.application.invoke_task("db:migrator[rollback]")
    end
  end

  task :rollback => "rollback:default"


  namespace :migrate do
    desc "migrate to current version"      
    task :up do |migratecmd|   
      Rake::Task["db:migrator"].reenable

      # 1.ダブルクオーテーションで囲む場合
      # -conn "Data Source=D:\db\albacore.db" --timeout=200 --task "migrate"
      # => 1: InitialMigration migrating が実行されない
      # 2.ダブルクオーテーションで囲まない場合
      #　-conn "Data Source=D:\db\albacore.db" --timeout=200 --task migrate 
      # => #1: InitialMigration migrating が実行される
      Rake.application.invoke_task("db:migrator[migrate]")  
    end

    desc "migrate down"
      task :down do |migratecmd|  
      Rake::Task["db:migrator"].reenable
      Rake.application.invoke_task("db:migrator[migrate:down]")
    end

    desc "Redo the last migration"
      task :redo => ["db:rollback", "db:migrate"] do |task|
      puts "Redo complete"
    end
  end

  task :migrate => "migrate:up"
end 


def production?
  ENV['DATABASE_URL'] != nil
end

def runner_path
  production? ? LOCAL_RUNNER.split('\\').last : LOCAL_RUNNER
end

def target_path
  production? ? LOCAL_TARGET.split('\\').last : LOCAL_TARGET
end

def provider
  production? ? 'postgres' : LOCAL_PROVIDER
end

def connection
  if production?
    require 'uri'
    db = URI.parse(ENV['DATABASE_URL'])
    "Server=#{db.host}; Port=#{db.port}; Database=#{db.path[1..-1]}; User Id=#{db.user}; Password=#{db.password}; SSL=true;SslMode=Require;"
  else
    LOCAL_CONNECTION
  end
end