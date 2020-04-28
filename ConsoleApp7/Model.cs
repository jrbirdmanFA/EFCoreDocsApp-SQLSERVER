using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ConsoleApp7
{
    public class BloggingContext : DbContext
    {
        private string _connectionString;

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<VideoPost> VideoPosts { get; set; }
        public DbSet<DumbAsAPost> DumbAsAPosts { get; set; }

        // Entities to terst BigInt vs Int PKs...
        //public DbSet<TestBigInt> TestBigInts { get; set; }
        //public DbSet<TestBigIntOrg> TestBigIntsOrg { get; set; }

        // Entities to test Private Backing Fields...

        public DbSet<BlogPBF> BlogsPBF { get; set; }
        public DbSet<PostPBF> PostsPBF { get; set; }
        public DbSet<VideoPostPBF> VideoPostsPBF { get; set; }
        public DbSet<DumbAsAPostPBF> DumpAsAPostsPBF { get; set; }

        public BloggingContext()
        {

        }

        public BloggingContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(string.IsNullOrEmpty(_connectionString) ? "Data Source=localhost;Initial Catalog=EfCoreDocDB;Integrated Security=SSPI;" : _connectionString );

        
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

        }
    }

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

    // BigInt Test Entities
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


    // Private Backing Field Entities

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


}