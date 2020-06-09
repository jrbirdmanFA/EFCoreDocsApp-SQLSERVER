using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleApp7
{
    class Program
    {

        static void Main()
        {
            //HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();

            string[] testsToRun = new string[4] { "xStandard", "xPBF", "xLL", "TPH" };

            using (var db = new BloggingContext())
            {
                if (db.Database.EnsureDeleted())
                    Console.WriteLine("Database dropped.");

                Console.WriteLine("Recreating Database");
                db.Database.Migrate();

                if (Array.Exists(testsToRun, x => x == "Standard"))
                {
                    // Create
                    Console.WriteLine("Inserting a new blog");
                    db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
                    db.SaveChanges();


                    // Read
                    Console.WriteLine("Querying for a blog");
                    var blog = db.Blogs
                        .OrderBy(b => b.BlogId)
                        .First();

                    // Update
                    Console.WriteLine("Updating the blog and adding a post");
                    blog.Url = "https://devblogs.microsoft.com/dotnet";
                    blog.Posts.Add(
                        new Post
                        {
                            Title = "Hello World",
                            Content = "I wrote an app using EF Core!"
                        });
                    db.SaveChanges();

                    blog.Posts.Add(
                        new VideoPost
                        {
                            Title = "Hello World, The Movie",
                            Content = "Lareum ipsum, alpha, beta, crapper...",
                            VideoTitle = "this is the video title",
                            ReleaseDate = DateTime.Today.AddDays(-300)
                        });
                    blog.Posts.Add(
                        new DumbAsAPost
                        {
                            Title = "Hello World, The Movie",
                            Content = "Lareum ipsum, alpha, beta, crapper...",
                            Stupid = true
                        });
                    db.SaveChanges();

                    var blogList = db.Blogs.ToList();
                    foreach (var b in blogList)
                    {
                        Console.WriteLine($"Blog Dump: {b.Url}");
                        foreach (var p in b.Posts)
                        {
                            Console.WriteLine($" Post Dump: {p.PostId} {p.Title}");
                        }
                    }
                }

                if (Array.Exists(testsToRun, x => x == "PBF"))
                {

                    ///Now...try the Private Field variations

                    Console.WriteLine("Testing entities with private backing fields...");

                    // Create
                    Console.WriteLine("Inserting a new blog");
                    db.Add(new BlogPBF { Url = "http://blogs.msdn.com/adonet" });
                    db.SaveChanges();


                    // Read
                    Console.WriteLine("Querying for a blog");
                    var blog2 = db.BlogsPBF
                        .OrderBy(b => b.BlogId)
                        .First();

                    // Update
                    Console.WriteLine("Updating the blog and adding a post");
                    blog2.Url = "https://devblogs.microsoft.com/dotnet";
                    blog2.Posts.Add(
                        new PostPBF
                        {
                            Title = "Hello World",
                            Content = "I wrote an app using EF Core!"
                        });
                    db.SaveChanges();

                    blog2.Posts.Add(
                        new VideoPostPBF
                        {
                            Title = "Hello World, The Movie",
                            Content = "Lareum ipsum, alpha, beta, crapper...",
                            VideoTitle = "this is the video title",
                            ReleaseDate = DateTime.Today.AddDays(-300)
                        });
                    blog2.Posts.Add(
                        new DumbAsAPostPBF
                        {
                            Title = "Hello World, The Movie",
                            Content = "Lareum ipsum, alpha, beta, crapper...",
                            Stupid = true
                        });
                    db.SaveChanges();


                    var blogList2 = db.BlogsPBF.ToList();
                    foreach (var b in blogList2)
                    {
                        Console.WriteLine($"PBF Blog Dump: {b.Url}");
                        foreach (var p in b.Posts)
                        {
                            Console.WriteLine($" PBF Post Dump: {p.PostId} {p.Title}");
                        }
                    }
                }

                if (Array.Exists(testsToRun, x => x == "LL"))
                {
                    ///Now...try the Lazy Loading Variations

                    Console.WriteLine("Testing entities with Lazy Loaded fields...");

                    // All data seeded in configuration classes...

                    // Read
                    Console.WriteLine("Querying for a blog");
                    //var blog3 = db.BlogsLL.Find(1);  //this should fire a query without a join.
                    //blog3.Fosts.ForEach(f => Console.WriteLine($" LL1 Post Dump: {f.PostType} {f.FostLLId} {f.Title}"));

                    //var blogs4 = db.BlogsLL.AsQueryable().Include(x => x.Fosts).SingleOrDefault(x => x.FlogLLId == 1); //Will this throw an error?  nope but it does do a join.
                    //blogs4.Fosts.ForEach(f => Console.WriteLine($" LL2 Post Dump: {f.PostType} {f.FostLLId} {f.Title}"));

                    var blog5 = db.BlogsLL.Find(1);  //this should fire a query without a join.
                    blog5.Fosts.Where(x => x.FostLLId == 3).ToList().ForEach(f => Console.WriteLine($" LL3 Post Dump: {f.PostType} {f.FostLLId} {f.Title}"));
                }

                if (Array.Exists(testsToRun, x => x == "TPH"))
                {
                    // Create
                    Console.WriteLine("Inserting a new blog");
                    db.Add(new BlogTPH { Url = "http://blogs.msdn.com/adonet" });
                    db.SaveChanges();

                    // Read
                    Console.WriteLine("Querying for a blog");
                    var blog = db.BlogsTPH
                        .OrderBy(b => b.BlogId)
                        .First();

                    var newWidget = new WidgetTPH();
                    db.Add(newWidget);
                    db.SaveChanges();

                    // Update
                    Console.WriteLine("Updating the blog and adding a post");
                    blog.Url = "https://devblogs.microsoft.com/dotnet";
                    blog.Posts.Add(
                        new PostTPH
                        {
                            Title = "Hello World",
                            Content = "I wrote an app using EF Core!",
                            Foobar = newWidget
                        });
                    db.SaveChanges();

                    blog.Posts.Add(
                        new VideoPostTPH
                        {
                            Title = "Hello World, The Movie",
                            Content = "Lareum ipsum, alpha, beta, crapper...",
                            VideoTitle = "this is the video title",
                            ReleaseDate = DateTime.Today.AddDays(-300),
                            Foobar = newWidget
                        });
                    blog.Posts.Add(
                        new DumbAsAPostTPH
                        {
                            Title = "Hello World, The Movie",
                            Content = "Lareum ipsum, alpha, beta, crapper...",
                            Stupid = true,
                            Foobar = newWidget
                        }); ;
                    db.SaveChanges();

                    var blogList = db.BlogsTPH.ToList();
                    foreach (var b in blogList)
                    {
                        Console.WriteLine($"Blog Dump: {b.Url}");
                        foreach (var p in b.Posts)
                        {
                            Console.WriteLine($" Post Dump: {p.PostId} {p.Title} WidgetID: {p.Foobar.WidgetId}");
                        }
                    }

                }

                ///
                /// Testing timeing of submitting query to database
                /// 
                //var queryable = db.Blogs.AsQueryable();
                //var test2 = queryable.Include(x => x.Posts); //nope
                //var test3 = test2.Where(x => x.BlogId !> 0); //nope
                //var test4 = test3.OrderBy(x => x.Url); //nope
                //var test5 = test4.ToList(); //Yes!

                //var test6 = queryable.FirstOrDefault(); //Yes!
                //var test7 = queryable.Select(x => x.Posts); //nope
                //var test8 = queryable.AsNoTracking(); //nope

                //DbSet<Blog> dbSet = db.Blogs; //nope
                //var test9 = dbSet.Include(x => x.Posts); //nope
                //var test10 = test9.Where(x => x.BlogId! > 0); //nope
                //var test11 = test10.OrderBy(x => x.Url); //nope
                //var test12 = test11.ToList(); //Yes!

                //var test13 = dbSet.FirstOrDefault(); //Yes!
                //var test14 = dbSet.Select(x => x.Posts); //nope
                //var test15 = dbSet.AsNoTracking(); //nope

                //Testing EntityFramework.Plus.EFCore...if installed.  If not, leave commented out.
                //var videoBlogList = db.Blogs.Where(x => (x. as VideoPost).ReleaseDate > DateTime.Today.AddMonths(-12));
                //var testList = db.Blogs.Include(x => (x.Posts as IList<VideoPost))).ToList();

                //Part of original code from docs.  Commented out because Database is dropped and recreated everything this program runs.
                // Delete
                //var blogList2 = db.Blogs.ToList();
                //foreach (var b in blogList2)
                //{
                //    Console.WriteLine("Delete a blog");
                //    db.Remove(b);
                //}
                //db.SaveChanges();
            }

            //using (var db = new BloggingContext())
            //{
            //    //Console.WriteLine("Testing fix 1...");
            //    //var testBigInt = new TestBigInt() { MyProperty = $"test - {DateTime.Now.ToShortTimeString()}" };
            //    //db.Add(testBigInt);
            //    //db.SaveChanges();

            //    //var testRecords = db.TestBigInts.ToList();
            //    //foreach (var tr in testRecords)
            //    //{
            //    //    Console.WriteLine($"Id: {tr.TestBigIntId}  MyProperty:{tr.MyProperty}");
            //    //}

            //    // Original
            //    Console.WriteLine("Testing the original...");
            //    var orgTest = new TestBigIntOrg() { MyProperty = $"test - {DateTime.Now.ToShortTimeString()}" };
            //    db.Add(orgTest);
            //    db.SaveChanges();

            //    var orgRecs = db.TestBigIntsOrg.ToList();
            //    foreach (var tr in orgRecs)
            //    {
            //        Console.WriteLine($"Id: {tr.Id}  MyProperty:{tr.MyProperty}");
            //    }

            //}
        }
    }
}