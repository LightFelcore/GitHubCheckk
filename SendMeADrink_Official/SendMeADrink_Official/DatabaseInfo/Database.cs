using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace SendMeADrink_Official
{
    public class MyDatabase
    {
        readonly SQLiteAsyncConnection db;
        public MyDatabase(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<Person>().Wait();
        }

        //Insert and Update new record
        public Task<int> SaveItemAsync(Person person)
        {
            if (person.Id != 0)
            {
                return db.UpdateAsync(person);
            }
            else
            {
                return db.InsertAsync(person);
            }
        }

        //Read Item
        public async Task<Person> GetItemAsync(int personId)
        {
            return await db.Table<Person>().Where(i => i.Id == personId).FirstOrDefaultAsync();
        }

        //Delete
        public async Task<int> DeleteItemAsync(Person person)
        {
            return await db.DeleteAsync(person);
        }
        //Read All Items
        public async Task<List<Person>> GetItemsAsync()
        {
            return await db.Table<Person>().ToListAsync();
        }
    }
}