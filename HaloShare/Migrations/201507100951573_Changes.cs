namespace HaloShare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileDownloads",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FileId = c.Int(nullable: false),
                        UserId = c.Int(),
                        UserIP = c.String(),
                        DownloadedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Files", t => t.FileId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.FileId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UploadedOn = c.DateTime(nullable: false),
                        FileName = c.String(),
                        Name = c.String(),
                        FileSize = c.Long(nullable: false),
                        DownloadCount = c.Int(nullable: false),
                        MD5 = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserName = c.String(maxLength: 15),
                        ForumName = c.String(),
                        ForumTitle = c.String(),
                        ForumJoined = c.String(),
                        ForumGroupId = c.Int(nullable: false),
                        ForumDisplayName = c.String(),
                        IsBanned = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GameMapVariants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileId = c.Int(nullable: false),
                        GameMapId = c.Int(nullable: false),
                        AuthorId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 16),
                        ShortDescription = c.String(maxLength: 128),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        MinPlayers = c.Int(),
                        MaxPlayers = c.Int(),
                        Rating = c.Double(nullable: false),
                        RatingCount = c.Int(nullable: false),
                        IsStaffPick = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Files", t => t.FileId, cascadeDelete: true)
                .ForeignKey("dbo.GameMaps", t => t.GameMapId, cascadeDelete: true)
                .Index(t => t.FileId)
                .Index(t => t.GameMapId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.GameMaps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        InternalName = c.String(),
                        InternalId = c.Int(nullable: false),
                        Description = c.String(),
                        IsOriginal = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorId = c.Int(nullable: false),
                        GameMapVariantId = c.Int(),
                        GameTypeVariantId = c.Int(),
                        Rate = c.Int(nullable: false),
                        RatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.GameMapVariants", t => t.GameMapVariantId)
                .ForeignKey("dbo.GameTypeVariants", t => t.GameTypeVariantId)
                .Index(t => t.AuthorId)
                .Index(t => t.GameMapVariantId)
                .Index(t => t.GameTypeVariantId);
            
            CreateTable(
                "dbo.Reactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorId = c.Int(nullable: false),
                        GameMapVariantId = c.Int(),
                        GameTypeVariantId = c.Int(),
                        ParentReactionId = c.Int(),
                        Comment = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        PostedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Reactions", t => t.ParentReactionId)
                .ForeignKey("dbo.GameMapVariants", t => t.GameMapVariantId)
                .ForeignKey("dbo.GameTypeVariants", t => t.GameTypeVariantId)
                .Index(t => t.AuthorId)
                .Index(t => t.GameMapVariantId)
                .Index(t => t.GameTypeVariantId)
                .Index(t => t.ParentReactionId);
            
            CreateTable(
                "dbo.GameTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        InternalName = c.String(),
                        InternalId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GameTypeVariants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileId = c.Int(nullable: false),
                        GameTypeId = c.Int(nullable: false),
                        AuthorId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 16),
                        ShortDescription = c.String(maxLength: 128),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Rating = c.Double(nullable: false),
                        RatingCount = c.Int(nullable: false),
                        IsStaffPick = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Files", t => t.FileId, cascadeDelete: true)
                .ForeignKey("dbo.GameTypes", t => t.GameTypeId, cascadeDelete: true)
                .Index(t => t.FileId)
                .Index(t => t.GameTypeId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.GameMapVariantImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameMapVariantId = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Name = c.String(),
                        FileName = c.String(),
                        UploadedOn = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        GameTypeVariant_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameTypeVariants", t => t.GameTypeVariant_Id)
                .Index(t => t.GameTypeVariant_Id);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorId = c.Int(nullable: false),
                        Description = c.String(),
                        Url = c.String(nullable: false),
                        IsHandled = c.Boolean(nullable: false),
                        ReportedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AuthorId, cascadeDelete: true)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.GameMapVariantGameTypes",
                c => new
                    {
                        GameMapVariantId = c.Int(nullable: false),
                        GameTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GameMapVariantId, t.GameTypeId })
                .ForeignKey("dbo.GameMapVariants", t => t.GameMapVariantId, cascadeDelete: true)
                .ForeignKey("dbo.GameTypes", t => t.GameTypeId, cascadeDelete: true)
                .Index(t => t.GameMapVariantId)
                .Index(t => t.GameTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reports", "AuthorId", "dbo.Users");
            DropForeignKey("dbo.FileDownloads", "UserId", "dbo.Users");
            DropForeignKey("dbo.GameMapVariantGameTypes", "GameTypeId", "dbo.GameTypes");
            DropForeignKey("dbo.GameMapVariantGameTypes", "GameMapVariantId", "dbo.GameMapVariants");
            DropForeignKey("dbo.Reactions", "GameTypeVariantId", "dbo.GameTypeVariants");
            DropForeignKey("dbo.Ratings", "GameTypeVariantId", "dbo.GameTypeVariants");
            DropForeignKey("dbo.GameMapVariantImages", "GameTypeVariant_Id", "dbo.GameTypeVariants");
            DropForeignKey("dbo.GameTypeVariants", "GameTypeId", "dbo.GameTypes");
            DropForeignKey("dbo.GameTypeVariants", "FileId", "dbo.Files");
            DropForeignKey("dbo.GameTypeVariants", "AuthorId", "dbo.Users");
            DropForeignKey("dbo.Reactions", "GameMapVariantId", "dbo.GameMapVariants");
            DropForeignKey("dbo.Reactions", "ParentReactionId", "dbo.Reactions");
            DropForeignKey("dbo.Reactions", "AuthorId", "dbo.Users");
            DropForeignKey("dbo.Ratings", "GameMapVariantId", "dbo.GameMapVariants");
            DropForeignKey("dbo.Ratings", "AuthorId", "dbo.Users");
            DropForeignKey("dbo.GameMapVariants", "GameMapId", "dbo.GameMaps");
            DropForeignKey("dbo.GameMapVariants", "FileId", "dbo.Files");
            DropForeignKey("dbo.GameMapVariants", "AuthorId", "dbo.Users");
            DropForeignKey("dbo.FileDownloads", "FileId", "dbo.Files");
            DropIndex("dbo.GameMapVariantGameTypes", new[] { "GameTypeId" });
            DropIndex("dbo.GameMapVariantGameTypes", new[] { "GameMapVariantId" });
            DropIndex("dbo.Reports", new[] { "AuthorId" });
            DropIndex("dbo.GameMapVariantImages", new[] { "GameTypeVariant_Id" });
            DropIndex("dbo.GameTypeVariants", new[] { "AuthorId" });
            DropIndex("dbo.GameTypeVariants", new[] { "GameTypeId" });
            DropIndex("dbo.GameTypeVariants", new[] { "FileId" });
            DropIndex("dbo.Reactions", new[] { "ParentReactionId" });
            DropIndex("dbo.Reactions", new[] { "GameTypeVariantId" });
            DropIndex("dbo.Reactions", new[] { "GameMapVariantId" });
            DropIndex("dbo.Reactions", new[] { "AuthorId" });
            DropIndex("dbo.Ratings", new[] { "GameTypeVariantId" });
            DropIndex("dbo.Ratings", new[] { "GameMapVariantId" });
            DropIndex("dbo.Ratings", new[] { "AuthorId" });
            DropIndex("dbo.GameMapVariants", new[] { "AuthorId" });
            DropIndex("dbo.GameMapVariants", new[] { "GameMapId" });
            DropIndex("dbo.GameMapVariants", new[] { "FileId" });
            DropIndex("dbo.FileDownloads", new[] { "UserId" });
            DropIndex("dbo.FileDownloads", new[] { "FileId" });
            DropTable("dbo.GameMapVariantGameTypes");
            DropTable("dbo.Reports");
            DropTable("dbo.GameMapVariantImages");
            DropTable("dbo.GameTypeVariants");
            DropTable("dbo.GameTypes");
            DropTable("dbo.Reactions");
            DropTable("dbo.Ratings");
            DropTable("dbo.GameMaps");
            DropTable("dbo.GameMapVariants");
            DropTable("dbo.Users");
            DropTable("dbo.Files");
            DropTable("dbo.FileDownloads");
        }
    }
}
