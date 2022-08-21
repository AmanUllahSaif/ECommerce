namespace ECommerce.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "UID", c => c.Long(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "UID");
        }
    }
}
