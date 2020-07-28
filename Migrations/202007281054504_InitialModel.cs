namespace NotesApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notebooks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        LastName = c.String(nullable: false, maxLength: 255),
                        Login = c.String(nullable: false, maxLength: 32),
                        Password = c.String(nullable: false, maxLength: 32),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotebookId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 255),
                        CreatedTime = c.DateTime(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                        FileLocation = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Notebooks", t => t.NotebookId, cascadeDelete: true)
                .Index(t => t.NotebookId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "NotebookId", "dbo.Notebooks");
            DropForeignKey("dbo.Notebooks", "UserId", "dbo.Users");
            DropIndex("dbo.Notes", new[] { "NotebookId" });
            DropIndex("dbo.Notebooks", new[] { "UserId" });
            DropTable("dbo.Notes");
            DropTable("dbo.Users");
            DropTable("dbo.Notebooks");
        }
    }
}
