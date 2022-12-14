//created postmanager.cs without any starter code 


using System;
using System.Collections.Generic;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    public class PostManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private PostRepository _postRepository;
        private BlogRepository _blogRepository;
        //needed author and blog repository
        private AuthorRepository _authorRepository;        
        private string _connectionString;

        public PostManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _postRepository = new PostRepository(connectionString);
            _authorRepository = new AuthorRepository(connectionString);
            _blogRepository = new BlogRepository(connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Post Menu");
            Console.WriteLine(" 1) List Posts");
            Console.WriteLine(" 2) Add Post");
            Console.WriteLine(" 3) Edit Post");
            Console.WriteLine(" 4) Remove Post");
            //added post details early
            Console.WriteLine(" 5) Post Details");
            //Console.WriteLine(" 6) Note Management");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    Add();
                    return this;
                case "3":
                    Edit();
                    return this;
                case "4":
                    Remove();
                    return this;
                case "5":
                    Post post = Choose();
                    if (post == null)
                    {
                        return this;
                    }
                    else
                    {
                        return new PostDetailManager(this, _connectionString, post.Id);
                    }
                //case "6":
                //    return new NoteManagementManager(this, _connectionString);
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }             
        }
        

        private void List()
        {
            List<Post> posts = _postRepository.GetAll();
            foreach (Post post in posts)
            {
                Console.WriteLine($"Title: {post.Title}  Url: {post.Url}");
            }
        }

        private Post Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a post:";
            }

            Console.WriteLine(prompt);

            List<Post> posts = _postRepository.GetAll();

            for (int i = 0; i < posts.Count; i++)
            {
                Post post = posts[i];
                Console.WriteLine($" {i + 1}) {post.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return posts[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }

        private void Add()
        {
            Console.WriteLine("New Post");
            Post post = new Post();

            Console.Write("Title: ");
            post.Title = Console.ReadLine();

            Console.Write("URL: ");
            post.Url = Console.ReadLine();

            Console.Write("Publish Date Time: ");
            post.PublishDateTime = Convert.ToDateTime(Console.ReadLine());

            Console.WriteLine("Please Choose An Author:");
            List<Author> authors = _authorRepository.GetAll();

            for (int i = 0; i < authors.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {authors[i].FullName}");
            }
            Console.Write("> ");

            int choice = int.Parse(Console.ReadLine());
            Author author = authors[choice - 1];

            post.Author = author;

            
            Console.WriteLine("Please Choose An Blog:");
            List<Blog> blogs = _blogRepository.GetAll();

            for (int i = 0; i < blogs.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {blogs[i].Title}");
            }
            Console.Write("> ");

            choice = int.Parse(Console.ReadLine());
            Blog blog = blogs[choice - 1];

            post.Blog = blog;


            _postRepository.Insert(post);
        }

        private void Edit()
        {
            Post postToEdit = Choose("Which post would you like to edit?");
            if (postToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            Console.Write("New post title (blank to leave unchanged: ");
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
            {
                postToEdit.Title = title;
            }
            Console.Write("New url (blank to leave unchanged: ");
            string url = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(url))
            {
                postToEdit.Url = url;
            }

            Console.Write("New Publish Date Time (blank to leave unchanged): ");
            string date = (Console.ReadLine());
            if (!string.IsNullOrWhiteSpace(date))
            {
                postToEdit.PublishDateTime = Convert.ToDateTime(date);
            }


            Console.WriteLine("New Author (blank to leave unchanged):");
            List<Author> authors = _authorRepository.GetAll();

           for(int i = 0; i < authors.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {authors[i].FullName}");
            }
            Console.Write("> ");

            string authorChoice = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(authorChoice))
            {
                int choice = int.Parse(authorChoice);
                Author author = authors[choice - 1];
                postToEdit.Author = author;
            }

            Console.WriteLine("New blog (blank to leave unchanged):");
            List<Blog> blogs = _blogRepository.GetAll();
            for (int i = 0; i < blogs.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {blogs[i].Title}");
            }
            Console.Write("> ");

            string blogChoice = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(blogChoice))
            {
                int bChoice = int.Parse(blogChoice);
                Blog blog = blogs[bChoice - 1];
                postToEdit.Blog = blog;
            }

            _postRepository.Update(postToEdit);

        }

        private void Remove()
        {
            Post postToDelete = Choose("Which post would you like to remove?");
            if (postToDelete != null)
            {
                _postRepository.Delete(postToDelete.Id);
            }
        }
    }
}
