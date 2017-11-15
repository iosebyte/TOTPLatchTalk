namespace LatchTalk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class totpfields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "TotpAuthenticatorEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "TotpSecretKey", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "TotpSecretKey");
            DropColumn("dbo.AspNetUsers", "TotpAuthenticatorEnabled");
        }
    }
}
