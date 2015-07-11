namespace HaloShare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reports", "AuthorId", "dbo.Users");
            DropIndex("dbo.Reports", new[] { "AuthorId" });
            AddColumn("dbo.Users", "DisplayName", c => c.String());
            AddColumn("dbo.Users", "GroupId", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "Joined", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "UserName", c => c.String());
            AlterColumn("dbo.Reports", "AuthorId", c => c.Int());
            CreateIndex("dbo.Reports", "AuthorId");
            AddForeignKey("dbo.Reports", "AuthorId", "dbo.Users", "Id");
            DropColumn("dbo.Users", "ForumName");
            DropColumn("dbo.Users", "ForumGroupId");
            DropColumn("dbo.Users", "ForumDisplayName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "ForumDisplayName", c => c.String());
            AddColumn("dbo.Users", "ForumGroupId", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "ForumName", c => c.String());
            DropForeignKey("dbo.Reports", "AuthorId", "dbo.Users");
            DropIndex("dbo.Reports", new[] { "AuthorId" });
            AlterColumn("dbo.Reports", "AuthorId", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "UserName", c => c.String(maxLength: 15));
            DropColumn("dbo.Users", "Joined");
            DropColumn("dbo.Users", "GroupId");
            DropColumn("dbo.Users", "DisplayName");
            CreateIndex("dbo.Reports", "AuthorId");
            AddForeignKey("dbo.Reports", "AuthorId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
