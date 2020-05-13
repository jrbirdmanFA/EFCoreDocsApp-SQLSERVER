using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp7.EntityConfigurations
{
    public class DumbAsAPostLLEntityConfiguration : IEntityTypeConfiguration<DumbAsAPostLL>
    {
        public void Configure(EntityTypeBuilder<DumbAsAPostLL> builder)
        {
            builder.HasData(new DumbAsAPostLL { FostLLId = 3, FlogLLId = 1, Title = "Hello World, The Silent Picture", Content = "This is as dumb as a post", Stupid = true });
        }
    }
}
