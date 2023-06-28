using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class TagRepository : BaseRepository, ITagRepository
    {
        public TagRepository(IConfiguration config) : base(config) { }
        public List<Tag> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Tag ORDER BY Tag.Name";
                    var reader = cmd.ExecuteReader();

                    var tags = new List<Tag>();

                    while (reader.Read())
                    {
                        tags.Add(new Tag()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                        });
                    }
                    reader.Close();

                    return tags;
                }
            }
        }
        public void AddTag(Tag tag)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using(var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Tag ([Name])
                                        OUTPUT INSERTED.Id
                                        VALUES (@name)";

                    cmd.Parameters.AddWithValue("@name", tag.Name);

                    tag.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
        public void UpdateTag(Tag tag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Tag
                                        SET [Name] = @name
                                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", tag.Id);
                    cmd.Parameters.AddWithValue("@name", tag.Name);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Tag GetTagById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Tag.[Name]
                                        FROM Tag
                                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();
                    Tag tag = new Tag();

                    if (reader.Read())
                    {
                        tag.Id = id;
                        tag.Name = reader.GetString(reader.GetOrdinal("Name"));
                    }

                    reader.Close();

                    return tag;
                    
                }
            }
        }

        public void DeleteTag(int id)
        {
            using (SqlConnection con = Connection)
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Tag WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

