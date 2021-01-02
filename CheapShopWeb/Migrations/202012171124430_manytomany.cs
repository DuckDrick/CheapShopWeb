namespace CheapShopWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class manytomany : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProducts",
                c => new
                    {
                        User_Id = c.String(nullable: false, maxLength: 128),
                        Product_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Product_id })
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Product_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProducts", "Product_id", "dbo.Products");
            DropForeignKey("dbo.UserProducts", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserProducts", new[] { "Product_id" });
            DropIndex("dbo.UserProducts", new[] { "User_Id" });
            DropTable("dbo.UserProducts");
        }
    }
}
