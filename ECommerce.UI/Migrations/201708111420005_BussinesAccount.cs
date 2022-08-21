namespace ECommerce.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BussinesAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "Address", c => c.String());
            AddColumn("dbo.Users", "BussinesName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "BussinesName");
            DropColumn("dbo.Users", "Address");
            DropColumn("dbo.Users", "IsApproved");
        }
    }
}
