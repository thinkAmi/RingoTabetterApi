
using FluentMigrator;

namespace RingoTabetterTask.Migrations
{
    [Migration(1)]
    public class Mig_001_Create_Apples : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("apples")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("name").AsString()
                .WithColumn("tweet_id").AsInt64()
                .WithColumn("tweet_at").AsDateTime()
                .WithColumn("tweet").AsString()
                .WithColumn("timestamp").AsDateTime();
        }
    }
}
