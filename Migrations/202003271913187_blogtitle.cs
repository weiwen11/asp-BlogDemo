namespace BlogDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class blogtitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "BlogTitle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "BlogTitle");
        }
    }
}
