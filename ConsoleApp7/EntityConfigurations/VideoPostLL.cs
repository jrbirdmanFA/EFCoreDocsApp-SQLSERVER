using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp7.EntityConfigurations
{
    public class VideoPostLLEntityConfiguration : IEntityTypeConfiguration<VideoPostLL>
    {
        public void Configure(EntityTypeBuilder<VideoPostLL> builder)
        {
            builder.HasData(new VideoPostLL { FostLLId = 2, FlogLLId = 1, Title = "Hello World, The Movie", Content = "Lareum ipsum, alpha, beta, crapper...", VideoTitle = "this is the Video Title", ReleaseDate = DateTime.Today.AddDays(-300) });
        }
    }
}
