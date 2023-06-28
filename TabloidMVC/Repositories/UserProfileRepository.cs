using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration config) : base(config) { }

        //method to get a list of all user profiles from the database
        public List<UserProfile> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"select UserProfile.Id as UserId, DisplayName, FirstName, LastName, Email, CreateDateTime,ImageLocation,UserType.Id as TypeId, UserType.Name
                                        from UserProfile
                                        join UserType on UserProfile.UserTypeId = UserType.Id";

                    List<UserProfile> users = new List<UserProfile>();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        UserProfile newUser = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("UserId")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            UserType = new UserType()
                            {
                            Id = reader.GetInt32(reader.GetOrdinal("TypeId")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                            }
                        };
                        if (reader.IsDBNull(reader.GetOrdinal("ImageLocation"))==false)
                        {
                            newUser.ImageLocation = reader.GetString(reader.GetOrdinal("ImageLocation"));

                        }
                        users.Add(newUser);

      
                    }
                    return users;
                }
            } 
        }

        public UserProfile GetById(int id)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"select UserProfile.Id as UserId, DisplayName, FirstName, LastName, Email, CreateDateTime,ImageLocation, UserType.Id as TypeId, UserType.Name
                                        from UserProfile
                                        join UserType on UserProfile.UserTypeId = UserType.Id
                                        where UserProfile.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    UserProfile selectedUser = new UserProfile();
                    if(reader.Read()) 
                    {
                        
                        {
                            selectedUser.Id = reader.GetInt32(reader.GetOrdinal("UserId"));
                            selectedUser.DisplayName = reader.GetString(reader.GetOrdinal("DisplayName"));
                            selectedUser.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                            selectedUser.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                            selectedUser.Email = reader.GetString(reader.GetOrdinal("Email"));
                            selectedUser.CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"));
                            selectedUser.UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("TypeId")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                        }
                            if (reader.IsDBNull(reader.GetOrdinal("ImageLocation")) == false)
                            {
                            selectedUser.ImageLocation = reader.GetString(reader.GetOrdinal("ImageLocation"));

                            }

                    };
                    return selectedUser;

                }
            }
        }



        public UserProfile GetByEmail(string email)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId,
                              ut.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE email = @email";
                    cmd.Parameters.AddWithValue("@email", email);

                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        };
                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }
        public void Add(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO UserProfile (
                        FirstName, LastName, DisplayName, Email, CreateDateTime, ImageLocation, UserTypeId)
                        OUTPUT INSERTED.ID
                        VALUES ( @FirstName, @LastName, @DisplayName, @Email, @CreateDateTime, @ImageLocation, @UserTypeId)";
                    cmd.Parameters.AddWithValue("@FirstName", userProfile.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", userProfile.LastName);
                    cmd.Parameters.AddWithValue("@DisplayName", userProfile.DisplayName);
                    cmd.Parameters.AddWithValue("@Email", userProfile.Email);
                    cmd.Parameters.AddWithValue("@CreateDateTime", System.DateTime.Now);
                    cmd.Parameters.AddWithValue("@ImageLocation", DbUtils.ValueOrDBNull(userProfile.ImageLocation));
                    cmd.Parameters.AddWithValue("@UserTypeId", 2);

                    userProfile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}
