namespace mvcBlogDB.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<mvcBlog.Models.mvcblogDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
        //mvcBlogDB.Models.mvcblogDB context
        protected override void Seed(mvcBlog.Models.mvcblogDB content)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
