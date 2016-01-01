namespace EasyRead.Core.DataAccess.Tests
{
    internal sealed class TestContextFactory : IDbContextFactory
    {
        private readonly EasyReadDbContext _context;

        public TestContextFactory(EasyReadDbContext context)
        {
            _context = context;
        }

        public EasyReadDbContext GetContext()
        {
            return _context;
        }
    }
}
