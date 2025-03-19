using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCatcode.Repository.DB.UserService
{
    public class UserService : IDBService<UserDTO, UserInsert, UserUpdate>
    {

        private readonly IDbConnection DbConnection;

        public UserService(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;
        }

        public UserDTO Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserDTO> Get(int maxItems = 100, int page = 1, object filters = null)
        {
            throw new NotImplementedException();
        }

        public UserDTO GetById(int id)
        {
            throw new NotImplementedException();
        }

        public UserDTO Insert(UserInsert data)
        {
            throw new NotImplementedException();
        }

        public UserDTO Update(UserUpdate data)
        {
            throw new NotImplementedException();
        }
    }
}
