﻿using System;
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
                    Console.WriteLine("To be implented soon");
                    Console.WriteLine();
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
    }
}
