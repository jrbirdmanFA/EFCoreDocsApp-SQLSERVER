using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using ConsoleApp7.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace ConsoleApp7
{
    public class BloggingContext : DbContext
    {
        private string _connectionString;

        private static readonly LoggerFactory _loggerFactory = new LoggerFactory(new[] { new DebugLoggerProvider() });

        #region Doc Entities

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<VideoPost> VideoPosts { get; set; }
        public DbSet<DumbAsAPost> DumbAsAPosts { get; set; }
        #endregion

        #region BitInt Entities

        //public DbSet<TestBigInt> TestBigInts { get; set; }
        //public DbSet<TestBigIntOrg> TestBigIntsOrg { get; set; }
        // Entities to test Private Backing Fields...
        #endregion

        #region Private Backing Field Entities

        public DbSet<BlogPBF> BlogsPBF { get; set; }
        public DbSet<PostPBF> PostsPBF { get; set; }
        public DbSet<VideoPostPBF> VideoPostsPBF { get; set; }
        public DbSet<DumbAsAPostPBF> DumpAsAPostsPBF { get; set; }
        #endregion

        #region LazyLoading Entities

        public DbSet<BlogLL> BlogsLL { get; set; }
        public DbSet<PostLL> PostsLL { get; set; }
        public DbSet<VideoPostLL> VideoPostsLL { get; set; }
        public DbSet<DumbAsAPostLL> DumpAsAPostsLL { get; set; }
        #endregion

        #region TPH Nav Property Loading Entities

        public DbSet<BlogTPH> BlogsTPH { get; set; }
        public DbSet<PostTPH> PostsTPH { get; set; }
        public DbSet<VideoPostTPH> VideoPostsTPH { get; set; }
        public DbSet<DumbAsAPostTPH> DumpAsAPostsTPH { get; set; }
        #endregion

        public BloggingContext()
        {

        }

        public BloggingContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(string.IsNullOrEmpty(_connectionString) ? "Data Source=localhost;Initial Catalog=EfCoreDocDB;Integrated Security=SSPI;" : _connectionString )
                        .UseLoggerFactory(_loggerFactory);

        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Post>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<VideoPost>("video")
                .HasValue<DumbAsAPost>("dumb");

            //Configs for Private Backing Field examples

            //builder.Entity<TestBigInt>()
            //    .Property(x => x.TestBigIntId)
            //    //.HasConversion(Extensions.IntAndLongConverter());
            //    .HasConversion<long>();

            //builder.Entity<TestBigIntOrg>()
            //    .Property(x => x.Id)
            //    .HasColumnType("bigint")
            //    .UseSqlServerIdentityColumn()
            //    //.ValueGeneratedNever() //SET IDENTITY_INSERT OFF
            //    //.Metadata.BeforeSaveBehavior = PropertySaveBehavior.Ignore
            //    //.HasConversion(Extensions.IntAndLongConverter())
            //    .HasConversion(Extensions.IntAndLongConverter2())
            //    ;

            //builder.Entity<TestBigIntOrg>()
            //    .Property(x => x.MyProperty)
            //    .HasColumnType("nvarchar(50)");

            //Configs for Private Backing Field examples

            #region Private Backing field Configurations

            builder.Entity<BlogPBF>()
                .ToTable("BlogsPBF")
                .HasKey(x => x.BlogId);

            builder.Entity<BlogPBF>()
                .Ignore(x => x.Posts)
                .Property(x => x.Url).HasField("url");

            builder.Entity<BlogPBF>()
                .HasMany(typeof(PostPBF), "posts").WithOne("Blog").HasForeignKey("BlogId");

            builder.Entity<PostPBF>()
                .ToTable("PostsPBF")
                .HasKey(x => x.PostId);

            builder.Entity<PostPBF>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<VideoPostPBF>("video")
                .HasValue<DumbAsAPostPBF>("dumb");
            #endregion

            #region Lazy Loading Configurations

            builder.ApplyConfiguration(new BlogLLEntityConfiguration());
            builder.ApplyConfiguration(new PostLLEntityConfiguration());
            builder.ApplyConfiguration(new VideoPostLLEntityConfiguration());
            builder.ApplyConfiguration(new DumbAsAPostLLEntityConfiguration());
            #endregion

            #region TPH Nav Property Loading Configurations

            builder.Entity<BlogTPH>()
                .HasKey(x => x.BlogId);

            builder.Entity<PostTPH>()
                .HasKey(x => x.PostId);

            builder.Entity<WidgetTPH>()
                .HasKey(x => x.WidgetId);

            builder.Entity<PostTPH>()
                .HasOne(x => x.Foobar)
                .WithMany()
                .HasForeignKey(x => x.WidgetId);

            #endregion
        }
    }

    #region Doc Entities

    public class Blog
    {
        public int BlogId { get; set; }

        public string Url { get; set; }

        public List<Post> Posts { get; } = new List<Post>();
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }

        public string Discriminator { get; set; }
    }

    public class VideoPost : Post
    {
        public string VideoTitle { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

    public class DumbAsAPost : Post
    {
        public bool Stupid { get; set; }
    }
    #endregion

    #region BigInt Entities

    public class TestBigInt
    {
        [Column(TypeName = "bigint")]
        public Int64 TestBigIntId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string MyProperty { get; set; }
    }

    public class TestBigIntOrg
    {
        public int Id { get; set; }

        public string MyProperty { get; set; }
    }
    #endregion

    #region Private Backing Fields Entities

    public class BlogPBF
    {
        public int BlogId { get; set; }

        private List<PostPBF> posts = new List<PostPBF>();
        private string url;

        public List<PostPBF> Posts
        {
            get => posts;
            set { posts = value; }
        }

        public string Url
        {
            get => url;
            set { url = value; }
        }
    }

    public class PostPBF
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public BlogPBF Blog { get; set; }

        public string Discriminator { get; set; }
    }
    public class VideoPostPBF : PostPBF
    {
        public string VideoTitle { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

    public class DumbAsAPostPBF : PostPBF
    {
        public bool Stupid { get; set; }
    }
    #endregion

    #region LazyLoading Entities

    public class BlogLL
    {
        private List<PostLL> fosts = new List<PostLL>();
        private string url;
        private readonly ILazyLoader _lazyLoader;

        public BlogLL()
        {

        }
        
        public BlogLL(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        public int FlogLLId { get; set; }

        public List<PostLL> Fosts
        {
            get => _lazyLoader.Load(this, ref fosts);
            set => fosts = value;
        }

        public string Url { get; set; }

    }

    public class PostLL
    {
        private readonly ILazyLoader _lazyLoader;

        public PostLL()
        {

        }

        public PostLL(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        public int FostLLId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int FlogLLId { get; set; }
        
        private BlogLL flog;

        public BlogLL Flog
        {
            get => _lazyLoader.Load(this, ref flog);
            set => flog = value;
        }

        //Discrimator bucket
        public string PostType { get; set; }
    }
    public class VideoPostLL : PostLL
    {
        public VideoPostLL()
        {

        }

        public VideoPostLL(ILazyLoader lazyLoader) : base(lazyLoader)
        {}

        public string VideoTitle { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

    public class DumbAsAPostLL : PostLL
    {
        public DumbAsAPostLL()
        {

        }

        public DumbAsAPostLL(ILazyLoader lazyLoader) : base(lazyLoader)
        {}

        public bool Stupid { get; set; }
    }
    #endregion

    #region TPH Nav Property Loading Entities

    public class BlogTPH
    {
        public int BlogId { get; set; }

        public string Url { get; set; }

        public List<PostTPH> Posts { get; } = new List<PostTPH>();
    }

    public class PostTPH
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public BlogTPH Blog { get; set; }

        public string Discriminator { get; set; }

        public int? WidgetId { get; set; }
        public WidgetTPH Foobar { get; set; }
    }

    public class VideoPostTPH : PostTPH
    {
        public string VideoTitle { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

    public class DumbAsAPostTPH : PostTPH
    {
        public bool Stupid { get; set; }
    }

    public class WidgetTPH
    {
        public int WidgetId { get; set; }
    }
    #endregion

}