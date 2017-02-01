namespace CalLicenseDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Feature",
                c => new
                    {
                        FeatureId = c.Int(nullable: false, identity: true),
                        FeatureTitle = c.String(),
                    })
                .PrimaryKey(t => t.FeatureId);
            
            CreateTable(
                "dbo.LicenseType",
                c => new
                    {
                        TypeId = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        Description = c.String(),
                        ActiveDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TypeId);
            
            CreateTable(
                "dbo.License",
                c => new
                    {
                        LicenseId = c.Int(nullable: false, identity: true),
                        LicenseKey = c.String(),
                        IsAvailable = c.Boolean(nullable: false),
                        LicenseTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LicenseId)
                .ForeignKey("dbo.LicenseType", t => t.LicenseTypeId, cascadeDelete: true)
                .Index(t => t.LicenseTypeId);
            
            CreateTable(
                "dbo.Team",
                c => new
                    {
                        TeamId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        GroupEmail = c.String(),
                    })
                .PrimaryKey(t => t.TeamId);
            
            CreateTable(
                "dbo.UserModel",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FName = c.String(),
                        LName = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        TeamID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Team", t => t.TeamID, cascadeDelete: true)
                .Index(t => t.TeamID);
            
            CreateTable(
                "dbo.UserLicense",
                c => new
                    {
                        UserLicenseId = c.Int(nullable: false, identity: true),
                        UserKey = c.String(),
                        IsExpired = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        ActivationDate = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        LicenseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserLicenseId)
                .ForeignKey("dbo.License", t => t.LicenseId, cascadeDelete: true)
                .ForeignKey("dbo.UserModel", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.LicenseId);
            
            CreateTable(
                "dbo.LicenseTypeFeature",
                c => new
                    {
                        LicenseType_TypeId = c.Int(nullable: false),
                        Feature_FeatureId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LicenseType_TypeId, t.Feature_FeatureId })
                .ForeignKey("dbo.LicenseType", t => t.LicenseType_TypeId, cascadeDelete: true)
                .ForeignKey("dbo.Feature", t => t.Feature_FeatureId, cascadeDelete: true)
                .Index(t => t.LicenseType_TypeId)
                .Index(t => t.Feature_FeatureId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserModel", "TeamID", "dbo.Team");
            DropForeignKey("dbo.UserLicense", "UserId", "dbo.UserModel");
            DropForeignKey("dbo.UserLicense", "LicenseId", "dbo.License");
            DropForeignKey("dbo.License", "LicenseTypeId", "dbo.LicenseType");
            DropForeignKey("dbo.LicenseTypeFeature", "Feature_FeatureId", "dbo.Feature");
            DropForeignKey("dbo.LicenseTypeFeature", "LicenseType_TypeId", "dbo.LicenseType");
            DropIndex("dbo.LicenseTypeFeature", new[] { "Feature_FeatureId" });
            DropIndex("dbo.LicenseTypeFeature", new[] { "LicenseType_TypeId" });
            DropIndex("dbo.UserLicense", new[] { "LicenseId" });
            DropIndex("dbo.UserLicense", new[] { "UserId" });
            DropIndex("dbo.UserModel", new[] { "TeamID" });
            DropIndex("dbo.License", new[] { "LicenseTypeId" });
            DropTable("dbo.LicenseTypeFeature");
            DropTable("dbo.UserLicense");
            DropTable("dbo.UserModel");
            DropTable("dbo.Team");
            DropTable("dbo.License");
            DropTable("dbo.LicenseType");
            DropTable("dbo.Feature");
        }
    }
}
