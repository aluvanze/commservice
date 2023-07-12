using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LectureApp
{
    class Program
    {
        static List<Student> students = new List<Student>();
        static List<Organization> organizations = new List<Organization>();

        static void Main(string[] args)
        {
            // Add some sample organizations
            organizations.Add(new Organization { Name = "Organization 1" });
            organizations.Add(new Organization { Name = "Organization 2" });
            organizations.Add(new Organization { Name = "Organization 3" });

            bool isLoggedIn = false;
            Lecturer loggedInLecturer = null;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Lecture App");
                Console.WriteLine("--------------------------");

                if (isLoggedIn)
                {
                    Console.WriteLine("Logged in as: " + loggedInLecturer.Username);
                    Console.WriteLine("1. Upload Excel Sheet");
                    Console.WriteLine("2. Assign Organizations");
                    Console.WriteLine("3. Edit Student Data");
                    Console.WriteLine("4. Delete Student Data");
                    Console.WriteLine("5. Logout");
                }
                else
                {
                    Console.WriteLine("1. Login");
                    Console.WriteLine("2. Exit");
                }

                Console.WriteLine("--------------------------");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                if (isLoggedIn)
                {
                    switch (choice)
                    {
                        case "1":
                            UploadExcelSheet();
                            break;
                        case "2":
                            AssignOrganizations();
                            break;
                        case "3":
                            EditStudentData();
                            break;
                        case "4":
                            DeleteStudentData();
                            break;
                        case "5":
                            isLoggedIn = false;
                            loggedInLecturer = null;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Username: ");
                            string username = Console.ReadLine();
                            Console.Write("Password: ");
                            string password = Console.ReadLine();

                            if (Login(username, password))
                            {
                                isLoggedIn = true;
                                loggedInLecturer = GetLecturerByUsername(username);
                            }
                            else
                            {
                                Console.WriteLine("Invalid username or password. Please try again.");
                                Console.ReadLine();
                            }
                            break;
                        case "2":
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
        }

        static bool Login(string username, string password)
        {
            // Implement your login logic here
            // You can use a database or any other method to authenticate the lecturer
            // This is just a sample implementation

            if (username == "admin" && password == "password")
                return true;
            else
                return false;
        }

        static Lecturer GetLecturerByUsername(string username)
        {
            // Implement your logic to retrieve lecturer details by username
            // This is just a sample implementation

            return new Lecturer { Username = username };
        }

        static void UploadExcelSheet()
        {
            Console.Write("Enter the path of the Excel sheet: ");
            string filePath = Console.ReadLine();

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found. Please try again.");
                Console.ReadLine();
                return;
            }

            // Read the Excel sheet and extract data
            // You will need to use a library like EPPlus to read the Excel file
            // This is just a sample implementation using a dummy method
            var excelData = ReadExcelData(filePath);

            if (excelData != null)
            {
                students.Clear();

                foreach (var row in excelData)
                {
                    string studentId = row["StudentId"];
                    string studentName = row["StudentName"];

                    students.Add(new Student { Id = studentId, Name = studentName });
                }

                Console.WriteLine("Excel sheet uploaded successfully.");
            }
            else
            {
                Console.WriteLine("Failed to read Excel sheet.");
            }

            Console.ReadLine();
        }

        static List<Dictionary<string, string>> ReadExcelData(string filePath)
        {
            // Implement your logic to read the Excel file and extract data
            // You can use a library like EPPlus to read the Excel file
            // This is just a dummy implementation that returns hardcoded data

            return new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "StudentId", "1" },
                    { "StudentName", "John" }
                },
                new Dictionary<string, string>
                {
                    { "StudentId", "2" },
                    { "StudentName", "Jane" }
                }
            };
        }

        static void AssignOrganizations()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("No student data found. Please upload an Excel sheet first.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Assigning organizations...");
            Console.WriteLine();

            foreach (var student in students)
            {
                Console.WriteLine("Assign organization for student ID: " + student.Id + ", Name: " + student.Name);
                Console.WriteLine("Available organizations:");

                for (int i = 0; i < organizations.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + organizations[i].Name);
                }

                Console.Write("Enter the organization number: ");
                string orgNumberStr = Console.ReadLine();

                if (int.TryParse(orgNumberStr, out int orgNumber) && orgNumber > 0 && orgNumber <= organizations.Count)
                {
                    student.Organization = organizations[orgNumber - 1];
                    Console.WriteLine("Organization assigned successfully.");
                }
                else
                {
                    Console.WriteLine("Invalid organization number. Skipping student.");
                }

                Console.WriteLine();
            }

            Console.WriteLine("Organization assignment completed.");
            Console.ReadLine();
        }

        static void EditStudentData()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("No student data found. Please upload an Excel sheet first.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Edit student data");
            Console.WriteLine("----------------");
            Console.Write("Enter the student ID: ");
            string studentId = Console.ReadLine();

            var student = students.FirstOrDefault(s => s.Id == studentId);

            if (student != null)
            {
                Console.WriteLine("Student found:");
                Console.WriteLine("ID: " + student.Id);
                Console.WriteLine("Name: " + student.Name);
                Console.WriteLine("Organization: " + (student.Organization != null ? student.Organization.Name : "Not assigned"));
                Console.WriteLine();

                Console.WriteLine("Enter the new student name (or leave blank to keep the existing name):");
                string newStudentName = Console.ReadLine();

                if (!string.IsNullOrEmpty(newStudentName))
                {
                    student.Name = newStudentName;
                    Console.WriteLine("Student name updated successfully.");
                }
                else
                {
                    Console.WriteLine("Student name not updated.");
                }
            }
            else
            {
                Console.WriteLine("Student not found.");
            }

            Console.ReadLine();
        }

        static void DeleteStudentData()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("No student data found. Please upload an Excel sheet first.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Delete student data");
            Console.WriteLine("------------------");
            Console.Write("Enter the student ID: ");
            string studentId = Console.ReadLine();

            var student = students.FirstOrDefault(s => s.Id == studentId);

            if (student != null)
            {
                students.Remove(student);
                Console.WriteLine("Student data deleted successfully.");
            }
            else
            {
                Console.WriteLine("Student not found.");
            }

            Console.ReadLine();
        }
    }

    class Student
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Organization Organization { get; set; }
    }

    class Organization
    {
        public string Name { get; set; }
    }

    class Lecturer
    {
        public string Username { get; set; }
    }
}
