
using FluentMigrator;

namespace RingoTabetterTask.Migrations
{
    [Migration(2)]
    public class Mig_002_Create_LastSearches : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("last_searches")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("tweet_id").AsInt64()
                .WithColumn("updated_at").AsDateTime().Nullable().WithDefault(SystemMethods.CurrentDateTime);
        }
    }
}
