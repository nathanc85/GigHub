namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeyPropertiesToGig : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Gigs", "Artist_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Gigs", "Genre_Id", "dbo.Genres");
            DropIndex("dbo.Gigs", new[] { "Artist_Id" });
            DropIndex("dbo.Gigs", new[] { "Genre_Id" });
            AddColumn("dbo.Gigs", "ArtistId", c => c.String(nullable: false));
            AddColumn("dbo.Gigs", "GenreId", c => c.Byte(nullable: false));
            DropColumn("dbo.Gigs", "Artist_Id");
            DropColumn("dbo.Gigs", "Genre_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Gigs", "Genre_Id", c => c.Byte(nullable: false));
            AddColumn("dbo.Gigs", "Artist_Id", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Gigs", "GenreId");
            DropColumn("dbo.Gigs", "ArtistId");
            CreateIndex("dbo.Gigs", "Genre_Id");
            CreateIndex("dbo.Gigs", "Artist_Id");
            AddForeignKey("dbo.Gigs", "Genre_Id", "dbo.Genres", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Gigs", "Artist_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
