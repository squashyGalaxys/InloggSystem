using System;
using System.Linq;
using InloggSystem;
using System.Data.Entity;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Välkommen till inloggningssidan!");

        //Meny för val
        while (true)
        {
            Console.WriteLine("Välj en åtgärd: ");
            Console.WriteLine("1. Skapa konto");
            Console.WriteLine("2. Logga in");
            Console.WriteLine("3. Hantera användare");
            Console.WriteLine("4. Lista befintliga användare");
            Console.WriteLine("5. Avsluta");
            //Läser in val från användaren.
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Sign in");
                    Console.Write("Ange ett användarnamn: ");
                    string newUsername = Console.ReadLine();

                    Console.Write("Ange ett lösenord: ");
                    string newPassword = Console.ReadLine();

                    // Skapa ett nytt User-objekt och lägg till i databasen
                    AddUser(newUsername, newPassword);

                    Console.WriteLine("Du har skapat ett konto. Gå tillbaka till huvudmeny för att logga in");
                    Console.ReadLine();
                    Console.Clear();
                    break;

                case "2":
                    Console.WriteLine("Log in");
                    Console.Write("Ange ditt användarnamn: ");
                    string username = Console.ReadLine();

                    Console.Write("Ange ditt lösenord: ");
                    string password = Console.ReadLine();

                    // Hämta användaren från databasen om den finns och jämför lösenordet
                    var user = GetUser(username);
                    if (user != null && user.Password == password)
                    {
                        Console.WriteLine("Du är inloggad!");
                    }
                    else
                    {
                        Console.WriteLine("Fel användarnamn eller lösenord.");
                    }
                    Console.WriteLine("Vad vill du göra nu? Vad behöver du idag?");
                    Console.WriteLine("Här får du en virituel kram. KRAM! <3");
                    Console.ReadLine();
                    Console.Clear();
                    break;

                    // case för att hantera användare, ta bort/ändra
                case "3":
                    Console.WriteLine("Hantera användare");
                    Console.WriteLine("1. Ta bort användare");
                    Console.WriteLine("2. Modifiera användare");
                    string choiceRemMod = Console.ReadLine();

                    // om val == 1 ta bort användare om användare finns i systemet
                    if (choiceRemMod == "1")
                    {
                        Console.WriteLine("Ange den användare du vill ta bort: ");
                        string userToHandle = Console.ReadLine();
                        //Skriver ut vilken användare som har tagits bort ur systemet
                        RemoveUser(userToHandle);
                        Console.WriteLine($"Följande användare har tagits bort ur systemet: {userToHandle}");
                    }
                    // om val == 2 ändra användare om användare finns i systemet
                    else if (choiceRemMod == "2")
                    {
                        Console.WriteLine("Ange den användare du vill modifiera: ");
                        string userToHandle = Console.ReadLine();
                        UserToModify(userToHandle);
                        //Skriver ut vilken användare som har ändrats
                        Console.WriteLine($"Följande användare har ändrats: {userToHandle}");
                    }
                    else
                    {
                        Console.WriteLine("Felaktig inmatning, testa igen.");
                    }
                    Console.Clear();
                    break;

                case "4":
                    // Skriv ut alla användare som finns lagrade i databasen
                    listOfUsers();
                    Console.ReadLine();
                    Console.Clear();
                    break;

                case "5":
                    Console.WriteLine("Avslutar programmet...");
                    return;
                default:
                    Console.WriteLine("Felaktigt val, försök igen.");
                    Console.Clear();
                    break;
            }
        }
    }

    static void listOfUsers()
    {
    using (var context = new UserContext())
        {
            var users = context.Users.ToList();
            Console.WriteLine("Lista på alla användare:");
            foreach (var user in users)
                Console.WriteLine($"Id: {user.Id}, Användarnamn: {user.Username}");
        }
    }

    // metod för att ta bort användare ur databas
    static void RemoveUser(string userName)
    {
        using (var context = new UserContext())
        {
            var user = context.Users.FirstOrDefault(u => u.Username == userName);
            if (user != null)
            {
                //Tar bort användaren och sparar ändringarna
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }

    }
    // metod för att ändra användare i databas
    private static void UserToModify(string userName)
    {

        using (var context = new UserContext())
        {
            var user = context.Users.FirstOrDefault(u => u.Username == userName);
            if (user != null)
            {
                Console.Write("Ange nytt användarnamn: ");
                user.Username = Console.ReadLine();

                Console.WriteLine("Ange nytt lösenord:");
                user.Password = Console.ReadLine();

                //Spara ändningar
                context.SaveChanges();
            }
        }
    }

    // metod för att hämta användare från databas
    static User GetUser(string username)
    {
        using (var context = new UserContext())
        {
            return context.Users.FirstOrDefault(u => u.Username == username);
        }
    }

    // metod för att lägga till ny användare i databas
    static void AddUser(string username, string password)
    {
        using (var context = new UserContext())
        {
            var user = new User
            {
                Username = username,
                Password = password
            };

            context.Users.Add(user);
            context.SaveChanges();
        }
    }    
}
