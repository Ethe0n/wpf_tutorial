using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TutorialApp.Model;

namespace TutorialApp.Service
{
    public class BookDataManager
    {
        private string GetConnectionString()
        {
            string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "db", "book.db");

            System.Diagnostics.Debug.WriteLine($"db path: {dbPath}");

            string folderPath = Path.GetDirectoryName(dbPath);
            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException($"Directory is not found: {folderPath}");
            }

            return $"Data Source={dbPath}";
        }

        public async Task<List<Book>> LoadAllBooks()
        {
            var books = new List<Book>();

            using (var conn = new SqliteConnection(GetConnectionString()))
            {
                await conn.OpenAsync();
                var cmd = new SqliteCommand("SELECT * FROM book LIMIT 50", conn);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        books.Add(new Book
                        {
                            Id = reader["id"].ToString(),
                            Title = reader["title"].ToString(),
                            Author = reader["author"].ToString(),
                            Genre = reader["genre"].ToString(),
                            Price = reader["price"].ToString(),
                            PublishedDate = reader["published_date"].ToString()
                        });
                    }
                }
            }

            return books;
        }

        public async Task AsyncTest()
        {
            await Task.Delay(2000);
        }
    }
}
