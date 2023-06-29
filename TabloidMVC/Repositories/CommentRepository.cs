using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(IConfiguration config) : base(config) { }

        public void AddComment(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Comment (PostId, UserProfileId, Subject, Content, CreateDateTime)
                                        OUTPUT INSERTED.Id
                                        VALUES (@postId, @userProfileId, @subject, @content, SYSDATETIME())";

                    cmd.Parameters.AddWithValue("@postId", comment.PostId);
                    cmd.Parameters.AddWithValue("@userProfileId", comment.UserProfileId);
                    cmd.Parameters.AddWithValue("@subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@content", comment.Content);

                    comment.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public List<Comment> GetAllCommentsByPostId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id,
	                                           c.PostId,
	                                           c.UserProfileId,
	                                           c.Subject,
	                                           c.Content,
	                                           c.CreateDateTime AS CommentCreateDateTime,
	                                           u.FirstName,
	                                           u.LastName,
	                                           u.DisplayName,
	                                           u.Email,
	                                           u.CreateDateTime AS UserRegisterDate,
	                                           u.ImageLocation AS AvatarImage,
	                                           u.UserTypeId,
	                                           ut.[Name] AS UserTypeName,
                                               p.Title AS PostTitle
                                               FROM Comment c
	                                           LEFT JOIN UserProfile u ON c.UserProfileId = u.Id
	                                           LEFT JOIN UserType ut ON u.UserTypeId = ut.Id
                                               LEFT JOIN Post p ON c.PostId = p.Id
                                               WHERE PostId = @Id
	                                           ORDER BY CommentCreateDateTime DESC";

                    cmd.Parameters.AddWithValue(@"Id", id);
                    var reader = cmd.ExecuteReader();

                    var comments = new List<Comment>();

                    while (reader.Read())
                    {
                        comments.Add(NewCommentFromReader(reader));
                    }

                    reader.Close();
                    
                    return comments;
                }
            }
        }

        private Comment NewCommentFromReader(SqlDataReader reader)
        {
            return new Comment()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Subject = reader.GetString(reader.GetOrdinal("Subject")),
                Content = reader.GetString(reader.GetOrdinal("Content")),
                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CommentCreateDateTime")),
                PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                PostTitle = reader.GetString(reader.GetOrdinal("PostTitle")),
                UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                UserProfile = new UserProfile()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    CreateDateTime = reader.GetDateTime(reader.GetOrdinal("UserRegisterDate")),
                    ImageLocation = DbUtils.GetNullableString(reader, "AvatarImage"),
                    UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                    UserType = new UserType()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                        Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                    }
                }
            };
        }
    }
}
