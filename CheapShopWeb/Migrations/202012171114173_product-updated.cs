namespace CheapShopWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productupdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "updated", c => c.DateTime(defaultValue: DateTime.Now, nullable: false));
            DropColumn("dbo.AspNetRoles", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Products", "updated");
        }
    }
}
