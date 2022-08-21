namespace ECommerce.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userunique_key2 : DbMigration
    {
        public override void Up()
        {
            RenameIndex(table: "dbo.Users", name: "IX_UID", newName: "Unique_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Users", name: "Unique_Id", newName: "IX_UID");
        }
    }
}
