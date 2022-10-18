using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    public class JournalManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private JournalRepository _journalRepository;
        private string _connectionString;

        public JournalManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _journalRepository = new JournalRepository(connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Journal Menu");
            Console.WriteLine("1) List Journals");
            Console.WriteLine("2) Add Journal");
            Console.WriteLine("3) Edit Journal");
            Console.WriteLine("4) Remove Journal");
            Console.WriteLine("0) Go Back");


            Console.Write(">: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    //runs list method, returns to Journal menu
                    List();
                    return this;
                case "2":
                    Add();
                    return this;
                case "3":
                    Console.WriteLine("To be implented soon");
                    Console.WriteLine();
                    return this;
                case "4":
                    Console.WriteLine("To be implented soon");
                    Console.WriteLine();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    Console.WriteLine();
                    return this;
            }
        }

        private void Add()
        {
            Console.WriteLine("New Journal");
            Journal entry = new Journal();

            Console.WriteLine("Title: ");
            entry.Title = Console.ReadLine();

            Console.WriteLine("Content: ");
            entry.Content = Console.ReadLine();

            entry.CreateDateTime = DateTime.Now;

            _journalRepository.Insert(entry);
        }

        private void List()
        {
            //Grabs List of all journals in database, foreach to display needed info
            List<Journal> journals = _journalRepository.GetAll();
            foreach (Journal journal in journals)
            {
                Console.WriteLine($"{journal.Id}) Title: {journal.Title}, Date Created: {journal.CreateDateTime}, Content: {journal.Content}");
            }
        }

        private void Edit()
        {
            Journal journalToEdit = Choose("Which journal would you like to edit?");
            if (journalToEdit == null)
            {
                return;
            }
            Console.WriteLine();
            Console.Write("New ")
        }

        private Journal Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a journal entry";
            }

            Console.WriteLine(prompt);

            List <Journal> journals = _journalRepository.GetAll();

            for (int i = 0; i < journals.Count; i++)
            {
                Journal journal = journals[i];
                Console.WriteLine($"{i + 1} {journal.Title}");
            }
            Console.Write(">: ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return journals[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }
    }
}
