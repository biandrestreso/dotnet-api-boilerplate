using BaseAPI.Interfaces;
using BaseAPI.Entities;

namespace BaseAPI.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repositoryContext;
        private IUserRepository _employee;

        public IUserRepository User
        {
            get
            {
                if (_employee == null)
                {
                    _employee = new UserRepository(_repositoryContext);
                }

                return _employee;
            }
        }

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public void Save()
        {
            _repositoryContext.SaveChanges();
        }
    }
}
