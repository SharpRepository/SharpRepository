namespace SharpRepository.Samples.MVC5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialContacts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Emails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmailAddress = c.String(),
                        Contact_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.Contact_Id)
                .Index(t => t.Contact_Id);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Emails", "Contact_Id", "dbo.Contacts");
            DropIndex("dbo.Emails", new[] { "Contact_Id" });
            DropTable("dbo.Emails");
            DropTable("dbo.Contacts");
        }
    }
}
