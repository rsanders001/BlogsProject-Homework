using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {


            Boolean blogIsRunning = true;
            logger.Info("Program started");
            try
            {

                while (blogIsRunning == true)
                {



                    Console.WriteLine("Select An Option");
                    Console.WriteLine("1.)Add A Blog");
                    Console.WriteLine("2.)Display All Blogs");
                    Console.WriteLine("3.)Create Post");
                    int response = Convert.ToInt32(Console.ReadLine());



                    if (response == 1)
                    {
                        // Create and save a new Blog
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();

                        var blog = new Blog { Name = name };

                        var db = new BloggingContext();
                        db.AddBlog(blog);
                        logger.Info("Blog added - {name}", name);
                    }

                    if (response == 2)
                    {
                        var db = new BloggingContext();
                        // Display all Blogs from the database
                        var query = db.Blogs.OrderBy(b => b.BlogId);

                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Name);
                        }
                    }
                    if (response == 3)
                    {
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        Console.WriteLine("All blogs in the database:");

                        List<int> blogIDArray = new List<int> { };



                        foreach (var item in query)
                        {
                            Console.WriteLine("ID: " + item.BlogId.ToString() + " NAME: " + item.Name);
                            blogIDArray.Add(item.BlogId);
                        }
                        Boolean valid = true;

                        while (valid == true)
                        {
                            Console.WriteLine("Select Blog You Would Like To Change: ");
                            int userChange = Convert.ToInt32(Console.ReadLine());
                            int chosenBlogID = 0;

                            for (int i = 0; i < blogIDArray.Count; i++)
                            {
                                if (userChange == blogIDArray.ElementAt(i))
                                {
                                    valid = false;
                                    chosenBlogID = blogIDArray.IndexOf(blogIDArray.ElementAt(i));
                                }

                            }
                            if (valid == true)
                            {
                                Console.WriteLine("PLEASE ENTER A VALID ID");
                            }
                            else
                            {
                                Console.WriteLine("Enter Post Title: ");
                                var title = Console.ReadLine();
                                Console.WriteLine("Enter Post Details: ");
                                var details = Console.ReadLine();

                                var newID = 1;


                                try
                                {
                                    newID = db.Posts.Select(p => p.PostId).Last() + 1;
                                }
                                catch
                                {
                                    Console.WriteLine("Post Stored");
                                }


                                var post = new Post { Title = title, Content = details, BlogId = chosenBlogID + 1, PostId = newID };
                                db.AddPost(post);
                                logger.Info("Post added - {title}", post);


                                // Display all Blogs from the database
                                var query2 = db.Blogs.OrderBy(b => b.Name);
                                Console.WriteLine("Posts In Database:");
                                foreach (var item in query2)
                                {
                                    Console.WriteLine(item.Name);
                                }
                            }


                        }





                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            Console.WriteLine("Press enter to quit");
            string x = Console.ReadLine();

            logger.Info("Program ended");
        }
    }
}
