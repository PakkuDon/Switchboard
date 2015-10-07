namespace Switchboard.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Switchboard.Models;
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<BoardDbContext>
    {
        private const string DEFAULT_PASSWORD = "Abc123*";

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Switchboard.Models.BoardDbContext";
        }

        protected override void Seed(BoardDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // Add users
            var roles = new List<string>
            {
                "Admin", "Moderator"
            };

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { UserName = "Administrator", Email = "administrator@switchboard.com" },
                new ApplicationUser { UserName = "Moderator", Email = "moderate@switchboard.com" },
                new ApplicationUser { UserName = "BobbyTables", Email = "bobbytables@example.com" }
            };

            // Add roles and users
            foreach (var role in roles)
            {
                if (!roleManager.RoleExists(role))
                {
                    roleManager.Create(new IdentityRole(role));
                }
            }

            foreach (var user in users)
            {
                userManager.Create(user, DEFAULT_PASSWORD);
            }

            // Assign admin and moderator roles
            userManager.AddToRole(context.Users.SingleOrDefault(u => u.UserName == "Administrator").Id, roles[0]);
            userManager.AddToRole(context.Users.SingleOrDefault(u => u.UserName == "Moderator").Id, roles[1]);

            // Add channel data
            var channels = new List<Channel>
            {
                new Channel { Name = "General", Description = "Casual chat thread." },
                new Channel { Name = "Technology", Description = "Stuff about tech. Maybe." },
                new Channel { Name = "Programming", Description = "C# Master Race." },
                new Channel { Name = "Work", Description = "" },
                new Channel { Name = "TV", Description = "Discussion related TV shows and channels." },
                new Channel { Name = "Lifestyle", Description = "Food, health, etc." },
                new Channel { Name = "News", Description = "Current events, either local, national or overseas." }
            };

            channels.ForEach(c => context.Channels.AddOrUpdate(i => i.Name, c));
            context.SaveChanges();

            // Add post data
            var posts = new List<Post>
            {
                new Post { DatePosted = DateTime.Now, Content = "Welcome to the General chat thread!",
                    ChannelID = context.Channels.SingleOrDefault(c => c.Name == "General").ID,
                    User = context.Users.SingleOrDefault(u => u.UserName == "Administrator")
                },
                new Post { DatePosted = DateTime.Now, Content = "This is a test reply.",
                    ChannelID = context.Channels.SingleOrDefault(c => c.Name == "General").ID,
                    User = context.Users.SingleOrDefault(u => u.UserName == "Administrator")
                },
                new Post { DatePosted = DateTime.Now, Content = "Use the form below to add a response.",
                    ChannelID = context.Channels.SingleOrDefault(c => c.Name == "General").ID,
                    User = context.Users.SingleOrDefault(u => u.UserName == "Administrator")
                },
                new Post { DatePosted = DateTime.Now, Content = "Hello world.",
                    ChannelID = context.Channels.SingleOrDefault(c => c.Name == "Programming").ID,
                    User = context.Users.SingleOrDefault(u => u.UserName == "Administrator")
                },
                new Post { DatePosted = DateTime.Now, Content = "DAE use COBOL?",
                    ChannelID = context.Channels.SingleOrDefault(c => c.Name == "Programming").ID,
                    User = context.Users.SingleOrDefault(u => u.UserName == "BobbyTables")
                },
                new Post { DatePosted = DateTime.Now, Content = "404 Thread not found.",
                    ChannelID = context.Channels.SingleOrDefault(c => c.Name == "Technology").ID,
                    User = context.Users.SingleOrDefault(u => u.UserName == "Moderator")
                }
            };

            posts.ForEach(p => context.Posts.AddOrUpdate(i => new { i.Content, i.ChannelID }, p));
            context.SaveChanges();
        }
    }
}
