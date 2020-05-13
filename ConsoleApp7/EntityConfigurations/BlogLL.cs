using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp7.EntityConfigurations
{
    public class BlogLLEntityConfiguration : IEntityTypeConfiguration<BlogLL>
    {
        public void Configure(EntityTypeBuilder<BlogLL> builder)
        {
            builder.HasKey(x => x.FlogLLId);

            //Does LL have to configuration the relationship?  Looks like Nah, but it still LLs as expected.
            builder.HasMany(x => x.Fosts).WithOne(x => x.Flog).HasForeignKey(x => x.FlogLLId);

            builder.HasData(new BlogLL { FlogLLId = 1, Url = "http://blogs.msdn.com/adonet" });

        }
    }
}
