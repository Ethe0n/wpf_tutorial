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
                        AddBookDataToList(reader, books);
                    }
                }
            }

            return books;
        }

        public async Task<List<Book>> SearchBooks(string field, string value)
        {
            var books = new List<Book>();

            using (var conn = new SqliteConnection(GetConnectionString()))
            {
                await conn.OpenAsync();

                string query = $"SELECT * FROM book WHERE {field} LIKE @search";

                var cmd = new SqliteCommand(query, conn);
                cmd.Parameters.AddWithValue("@search", $"%{value}%");
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        return books;
                    }

                    while (await reader.ReadAsync())
                    {
                        AddBookDataToList(reader, books);
                    }
                }
            }

            return books;
        }

        private void AddBookDataToList(SqliteDataReader reader, List<Book> books)
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
