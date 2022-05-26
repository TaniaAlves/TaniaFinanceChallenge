using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Document.Data.Context
{
    public class DocumentDataContext : DataContext
    {
        public DbSet<Domain.Entities.Document> Documents { get; set; }

        public DocumentDataContext(DbContextOptions<DocumentDataContext> options) : base(options)
        {

        }

    }
}
