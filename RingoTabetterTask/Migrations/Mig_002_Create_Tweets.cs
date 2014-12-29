
using FluentMigrator;

namespace RingoTabetterTask.Migrations
{
    public class Mig_002_Create_Tweets : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("tweets")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("last_searched_id").AsInt64()
                .WithColumn("timestamp").AsDateTime();
        }
    }
}
