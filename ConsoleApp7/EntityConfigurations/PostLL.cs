using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp7.EntityConfigurations
{
    public class PostLLEntityConfiguration : IEntityTypeConfiguration<PostLL>
    {
        public void Configure(EntityTypeBuilder<PostLL> builder)
        {
            builder.HasKey(x => x.FostLLId);

            builder.HasDiscriminator<string>("PostType")
                .HasValue<PostLL>("Moe")
                .HasValue<VideoPostLL>("Larry")
                .HasValue<DumbAsAPostLL>("Curly");

            builder.HasData(new PostLL { FostLLId = 1, FlogLLId = 1, Title = "Hello World", Content = "I wrote an app using EF Core!" });
        }
    }
}
