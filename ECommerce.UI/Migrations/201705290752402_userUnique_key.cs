namespace ECommerce.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userUnique_key : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Users", "UID", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "UID" });
        }
    }
}
