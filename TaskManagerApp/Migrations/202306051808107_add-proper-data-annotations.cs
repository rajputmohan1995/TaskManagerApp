namespace TaskManagerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addproperdataannotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TblTasks", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TblTasks", "Name", c => c.String());
        }
    }
}
