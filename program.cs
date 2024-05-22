using System;
using System.Linq;

class Program
{
    private const string StaffUsername = "staff";
    private const string StaffPassword = "today123";
    private static readonly string[] ValidGenres = { "drama", "adventure", "family", "action", "sci-fi", "comedy", "animated", "thriller", "other" };
    private static readonly string[] ValidClassifications = { "G", "PG", "M15+", "MA15+" };
    private const int MaxDuration = 300; 

    static void Main(string[] args)
    {
        MovieCollection movieCollection = new MovieCollection();
        MemberCollection memberCollection = new MemberCollection();

        while (true)
        {
            Console.WriteLine("=== Community Library System ===");
            Console.WriteLine("1. Staff Login");
            Console.WriteLine("2. Member Login");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    StaffLogin(movieCollection, memberCollection);
                    break;
                case "2":
                    MemberLogin(movieCollection, memberCollection);
                    break;
                case "3":
                    Console.WriteLine("Exiting the system. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void StaffLogin(MovieCollection movieCollection, MemberCollection memberCollection)
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine()?.Trim();
        Console.Write("Enter password: ");
        string password = Console.ReadLine()?.Trim();

        if (username == StaffUsername && password == StaffPassword)
        {
            while (true)
            {
                Console.WriteLine("\n=== Staff Menu ===");
                Console.WriteLine("1. Add Movie");
                Console.WriteLine("2. Remove Movie");
                Console.WriteLine("3. Register Member");
                Console.WriteLine("4. Remove Member");
                Console.WriteLine("5. Find Member's Contact");
                Console.WriteLine("6. Find Members Renting Movie");
                Console.WriteLine("7. Logout");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        AddMovie(movieCollection);
                        break;
                    case "2":
                        RemoveMovie(movieCollection);
                        break;
                    case "3":
                        RegisterMember(memberCollection);
                        break;
                    case "4":
                        RemoveMember(memberCollection);
                        break;
                    case "5":
                        FindMemberContact(memberCollection);
                        break;
                    case "6":
                        FindMembersRentingMovie(movieCollection, memberCollection);
                        break;
                    case "7":
                        Console.WriteLine("Logging out from staff menu.");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid username or password. Access denied.");
        }
    }

    static void MemberLogin(MovieCollection movieCollection, MemberCollection memberCollection)
    {
        Console.Write("Enter first name: ");
        string firstName = Console.ReadLine()?.Trim();
        Console.Write("Enter last name: ");
        string lastName = Console.ReadLine()?.Trim();
        Console.Write("Enter password: ");
        string password = Console.ReadLine()?.Trim();

        Member member = memberCollection.FindMember(firstName, lastName, password);

        if (member != null)
        {
            while (true)
            {
                Console.WriteLine("\n=== Member Menu ===");
                Console.WriteLine("1. Display All Movies");
                Console.WriteLine("2. Display Movie Information");
                Console.WriteLine("3. Borrow Movie");
                Console.WriteLine("4. Return Movie");
                Console.WriteLine("5. List Borrowed Movies");
                Console.WriteLine("6. Display Top 3 Movies");
                Console.WriteLine("7. Logout");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        DisplayAllMovies(movieCollection);
                        break;
                    case "2":
                        DisplayMovieInformation(movieCollection);
                        break;
                    case "3":
                        BorrowMovie(movieCollection, member);
                        break;
                    case "4":
                        ReturnMovie(movieCollection, member);
                        break;
                    case "5":
                        ListBorrowedMovies(member);
                        break;
                    case "6":
                        DisplayTopMovies(movieCollection);
                        break;
                    case "7":
                        Console.WriteLine("Logging out from member menu.");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid member information. Access denied.");
        }
    }

    static void AddMovie(MovieCollection movieCollection)
    {
        try
        {
            Console.Write("Enter movie title: ");
            string title = Console.ReadLine()?.Trim();

            Console.Write("Enter movie genre (drama, adventure, family, action, sci-fi, comedy, animated, thriller, other): ");
            string genre = Console.ReadLine()?.Trim().ToLower();
            if (!ValidGenres.Contains(genre))
            {
                Console.WriteLine("Invalid genre. Please enter a valid genre from the list.");
                return;
            }

            Console.Write("Enter movie classification (G, PG, M15+, MA15+): ");
            string classification = Console.ReadLine()?.Trim().ToUpper();
            if (!ValidClassifications.Contains(classification))
            {
                Console.WriteLine("Invalid classification. Please enter a valid classification.");
                return;
            }

            Console.Write("Enter movie duration (in minutes): ");
            int duration = int.Parse(Console.ReadLine()?.Trim() ?? string.Empty);
            if (duration <= 0 || duration > MaxDuration)
            {
                Console.WriteLine($"Invalid duration. Please enter a duration between 1 and {MaxDuration} minutes.");
                return;
            }

            Console.Write("Enter number of copies: ");
            int copies = int.Parse(Console.ReadLine()?.Trim() ?? string.Empty);
            if (copies < 0)
            {
                Console.WriteLine("Invalid number of copies. Please enter a positive number.");
                return;
            }

            Movie movie = new Movie(title, genre, classification, duration, copies);
            movieCollection.AddMovie(movie);

            Console.WriteLine("Movie added successfully.");
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input format. Please enter valid numeric values.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding movie: {ex.Message}");
        }
    }

    static void RemoveMovie(MovieCollection movieCollection)
    {
        try
        {
            Console.Write("Enter movie title: ");
            string title = Console.ReadLine()?.Trim();

            Console.Write("Enter number of copies to remove: ");
            int copies = int.Parse(Console.ReadLine()?.Trim() ?? string.Empty);

            movieCollection.RemoveMovie(title, copies);

            Console.WriteLine("Movie removed successfully.");
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input format. Please enter a valid numeric value for copies.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing movie: {ex.Message}");
        }
    }

    static void RegisterMember(MemberCollection memberCollection)
    {
        try
        {
            Console.Write("Enter first name: ");
            string firstName = Console.ReadLine()?.Trim();
            Console.Write("Enter last name: ");
            string lastName = Console.ReadLine()?.Trim();
            Console.Write("Enter contact number (minimum 10 digits): ");
            string contactNumber = Console.ReadLine()?.Trim();
            if (contactNumber.Length < 10 || !contactNumber.All(char.IsDigit))
            {
                Console.WriteLine("Invalid contact number. It must be at least 10 digits long and contain only numbers.");
                return;
            }

            Console.Write("Enter password (4 digits): ");
            string password = Console.ReadLine()?.Trim();
            if (password.Length != 4 || !int.TryParse(password, out _))
            {
                Console.WriteLine("Invalid password. Password must be 4 digits.");
                return;
            }

            Member member = new Member(firstName, lastName, contactNumber, password);
            memberCollection.AddMember(member);

            Console.WriteLine("Member registered successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error registering member: {ex.Message}");
        }
    }

    static void RemoveMember(MemberCollection memberCollection)
    {
        try
        {
            Console.Write("Enter first name: ");
            string firstName = Console.ReadLine()?.Trim();
            Console.Write("Enter last name: ");
            string lastName = Console.ReadLine()?.Trim();

            memberCollection.RemoveMember(firstName, lastName);

            Console.WriteLine("Member removed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing member: {ex.Message}");
        }
    }

    static void FindMemberContact(MemberCollection memberCollection)
    {
        try
        {
            Console.Write("Enter first name: ");
            string firstName = Console.ReadLine()?.Trim();
            Console.Write("Enter last name: ");
            string lastName = Console.ReadLine()?.Trim();

            string contactNumber = memberCollection.FindMemberContact(firstName, lastName);

            if (contactNumber != null)
            {
                Console.WriteLine($"Contact number: {contactNumber}");
            }
            else
            {
                Console.WriteLine("Member not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error finding member's contact: {ex.Message}");
        }
    }

    static void FindMembersRentingMovie(MovieCollection movieCollection, MemberCollection memberCollection)
    {
        try
        {
            Console.Write("Enter movie title: ");
            string title = Console.ReadLine()?.Trim();

            Member[] members = memberCollection.FindMembersRentingMovie(title);

            if (members.Length > 0)
            {
                Console.WriteLine("Members renting the movie:");
                foreach (Member member in members)
                {
                    Console.WriteLine($"{member.FirstName} {member.LastName}");
                }
            }
            else
            {
                Console.WriteLine("No members are currently renting this movie.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error finding members renting movie: {ex.Message}");
        }
    }

    static void DisplayAllMovies(MovieCollection movieCollection)
    {
        try
        {
            Movie[] movies = movieCollection.GetAllMovies();

            if (movies.Length > 0)
            {
                Console.WriteLine("All movies in the library:");
                foreach (Movie movie in movies)
                {
                    Console.WriteLine($"Title: {movie.Title}, Genre: {movie.Genre}, Classification: {movie.Classification}, Duration: {movie.Duration} minutes, Copies: {movie.Copies}");
                }
            }
            else
            {
                Console.WriteLine("No movies found in the library.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error displaying all movies: {ex.Message}");
        }
    }

    static void DisplayMovieInformation(MovieCollection movieCollection)
    {
        try
        {
            Console.Write("Enter movie title: ");
            string title = Console.ReadLine()?.Trim();

            Movie movie = movieCollection.GetMovieByTitle(title);

            if (movie != null)
            {
                Console.WriteLine($"Title: {movie.Title}, Genre: {movie.Genre}, Classification: {movie.Classification}, Duration: {movie.Duration} minutes, Copies: {movie.Copies}");
            }
            else
            {
                Console.WriteLine("Movie not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error displaying movie information: {ex.Message}");
        }
    }

    static void BorrowMovie(MovieCollection movieCollection, Member member)
    {
        try
        {
            Console.Write("Enter movie title: ");
            string title = Console.ReadLine()?.Trim();

            bool success = member.BorrowMovie(title, movieCollection);

            if (success)
            {
                Console.WriteLine("Movie borrowed successfully.");
            }
            else
            {
                Console.WriteLine("Failed to borrow movie. Please check the movie availability and your borrowing limit.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error borrowing movie: {ex.Message}");
        }
    }

    static void ReturnMovie(MovieCollection movieCollection, Member member)
    {
        try
        {
            Console.Write("Enter movie title: ");
            string title = Console.ReadLine()?.Trim();

            bool success = member.ReturnMovie(title, movieCollection);

            if (success)
            {
                Console.WriteLine("Movie returned successfully.");
            }
            else
            {
                Console.WriteLine("Failed to return movie. Please check if you have borrowed the movie.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error returning movie: {ex.Message}");
        }
    }

    static void ListBorrowedMovies(Member member)
    {
        try
        {
            string[] borrowedMovies = member.GetBorrowedMovies();

            if (borrowedMovies.Length > 0)
            {
                Console.WriteLine("Borrowed movies:");
                foreach (string title in borrowedMovies)
                {
                    Console.WriteLine(title);
                }
            }
            else
            {
                Console.WriteLine("No movies currently borrowed.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error listing borrowed movies: {ex.Message}");
        }
    }

    static void DisplayTopMovies(MovieCollection movieCollection)
    {
        try
        {
            (string, int)[] topMovies = movieCollection.GetTopMovies(3);

            if (topMovies.Length > 0)
            {
                Console.WriteLine("Top 3 most frequently borrowed movies:");
                foreach (var movie in topMovies)
                {
                    Console.WriteLine($"{movie.Item1}: {movie.Item2} times");
                }
            }
            else
            {
                Console.WriteLine("No movies have been borrowed yet.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error displaying top movies: {ex.Message}");
        }
    }
}

class Movie
{
    public string Title { get; }
    public string Genre { get; }
    public string Classification { get; }
    public int Duration { get; }
    public int Copies { get; private set; }
    public int BorrowCount { get; private set; }

    public Movie(string title, string genre, string classification, int duration, int copies)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Movie title cannot be null or empty.", nameof(title));
        }
        if (string.IsNullOrWhiteSpace(genre))
        {
            throw new ArgumentException("Movie genre cannot be null or empty.", nameof(genre));
        }
        if (string.IsNullOrWhiteSpace(classification))
        {
            throw new ArgumentException("Movie classification cannot be null or empty.", nameof(classification));
        }
        if (duration <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(duration), "Movie duration must be greater than zero.");
        }
        if (copies < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(copies), "Number of copies cannot be negative.");
        }

        Title = title;
        Genre = genre;
        Classification = classification;
        Duration = duration;
        Copies = copies;
        BorrowCount = 0;
    }

    public void AddCopies(int count)
    {
        if (count <= 0)
        {
            throw new ArgumentException("Number of copies to add must be greater than zero.", nameof(count));
        }

        Copies += count;
    }

    public void RemoveCopies(int count)
    {
        if (count <= 0)
        {
            throw new ArgumentException("Number of copies to remove must be greater than zero.", nameof(count));
        }

        if (count > Copies)
        {
            throw new ArgumentException("Cannot remove more copies than available.", nameof(count));
        }

        Copies -= count;
    }

    public void IncrementBorrowCount()
    {
        BorrowCount++;
    }

    public void DecrementBorrowCount()
    {
        if (BorrowCount > 0)
        {
            BorrowCount--;
        }
    }

    public bool IsAvailable()
    {
        return Copies > 0;
    }
}

class MovieCollection
{
    private const int InitialCapacity = 16;
    private const double LoadFactor = 0.75;
    private Movie[] movies;
    private int count;

    public MovieCollection()
    {
        movies = new Movie[InitialCapacity];
        count = 0;
    }

    public void AddMovie(Movie movie)
    {
        if (movie == null)
        {
            throw new ArgumentNullException(nameof(movie), "Movie cannot be null.");
        }

        if (count >= movies.Length * LoadFactor)
        {
            Resize();
        }

        int index = GetMovieIndex(movie.Title);
        if (index >= 0)
        {
            movies[index].AddCopies(movie.Copies);
        }
        else
        {
            index = GetNextAvailableIndex(movie.Title);
            movies[index] = movie;
            count++;
        }
    }

    public void RemoveMovie(string title, int copies)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Movie title cannot be null or empty.", nameof(title));
        }

        if (copies <= 0)
        {
            throw new ArgumentException("Number of copies to remove must be greater than zero.", nameof(copies));
        }

        int index = GetMovieIndex(title);
        if (index >= 0)
        {
            if (movies[index].Copies <= copies)
            {
                copies -= movies[index].Copies;
                RemoveMovieAtIndex(index);
            }
            else
            {
                movies[index].RemoveCopies(copies);
            }
        }
        else
        {
            throw new InvalidOperationException("Movie not found.");
        }
    }

    public Movie GetMovieByTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Movie title cannot be null or empty.", nameof(title));
        }

        int index = GetMovieIndex(title);
        if (index >= 0)
        {
            return movies[index];
        }

        return null;
    }

    public Movie[] GetAllMovies()
{
    Movie[] allMovies = new Movie[count];
    int index = 0;
    for (int i = 0; i < movies.Length; i++)
    {
        if (movies[i] != null)
        {
            allMovies[index++] = movies[i];
        }
    }
    return allMovies;
}

private void QuickSort(Movie[] movies, int low, int high)
{
    if (low < high)
    {
        int pivotIndex = Partition(movies, low, high);
        QuickSort(movies, low, pivotIndex - 1);
        QuickSort(movies, pivotIndex + 1, high);
    }
}

private int Partition(Movie[] movies, int low, int high)
{
    Movie pivot = movies[high];
    int i = low - 1;

    for (int j = low; j < high; j++)
    {
        if (movies[j].BorrowCount > pivot.BorrowCount)  // Sorting in descending order
        {
            i++;
            Swap(movies, i, j);
        }
    }

    Swap(movies, i + 1, high);
    return i + 1;
}

private void Swap(Movie[] movies, int i, int j)
{
    Movie temp = movies[i];
    movies[i] = movies[j];
    movies[j] = temp;
}


public (string, int)[] GetTopMovies(int count)
{
    if (count <= 0)
    {
        throw new ArgumentException("Count must be greater than zero.", nameof(count));
    }

    Movie[] allMovies = GetAllMovies();

    // Sorting the movies by BorrowCount using QuickSort
    QuickSort(allMovies, 0, allMovies.Length - 1);

    // Collecting the top 'count' movies
    int topCount = Math.Min(count, allMovies.Length);
    (string, int)[] topMovies = new (string, int)[topCount];

    for (int i = 0; i < topCount; i++)
    {
        if (allMovies[i] != null)
        {
            topMovies[i] = (allMovies[i].Title, allMovies[i].BorrowCount);
        }
    }

    return topMovies;
}



    private int GetMovieIndex(string title)
    {
        int hash = GetHash(title);
        int index = hash % movies.Length;

        while (movies[index] != null && !movies[index].Title.Equals(title, StringComparison.OrdinalIgnoreCase))
        {
            index = (index + 1) % movies.Length;
        }

        if (movies[index] != null && movies[index].Title.Equals(title, StringComparison.OrdinalIgnoreCase))
        {
            return index;
        }

        return -1;
    }

    private int GetNextAvailableIndex(string title)
    {
        int hash = GetHash(title);
        int index = hash % movies.Length;

        while (movies[index] != null)
        {
            index = (index + 1) % movies.Length;
        }

        return index;
    }

    private void RemoveMovieAtIndex(int index)
    {
        movies[index] = null;
        count--;

        int nextIndex = (index + 1) % movies.Length;
        while (movies[nextIndex] != null)
        {
            int hash = GetHash(movies[nextIndex].Title);
            int targetIndex = hash % movies.Length;

            if ((nextIndex < targetIndex && targetIndex <= index) ||
                (targetIndex <= index && index < nextIndex) ||
                (index < nextIndex && nextIndex < targetIndex))
            {
                movies[index] = movies[nextIndex];
                movies[nextIndex] = null;
                index = nextIndex;
            }

            nextIndex = (nextIndex + 1) % movies.Length;
        }
    }

    private void Resize()
    {
        int newCapacity = movies.Length * 2;
        Movie[] newMovies = new Movie[newCapacity];

        for (int i = 0; i < movies.Length; i++)
        {
            if (movies[i] != null)
            {
                int index = GetNextAvailableIndex(movies[i].Title);
                newMovies[index] = movies[i];
            }
        }

        movies = newMovies;
    }

    private int GetHash(string key)
    {
        int hash = 17;
        foreach (char c in key)
        {
            hash = hash * 23 + c;
        }
        return Math.Abs(hash);
    }
}

class Member
{
    private const int MaxBorrowedMovies = 5;

    public string FirstName { get; }
    public string LastName { get; }
    public string ContactNumber { get; private set; }
    public string Password { get; private set; }

    private string[] borrowedMovies;
    private int borrowedCount;

    public Member(string firstName, string lastName, string contactNumber, string password)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));
        }
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));
        }
        if (string.IsNullOrWhiteSpace(contactNumber))
        {
            throw new ArgumentException("Contact number cannot be null or empty.", nameof(contactNumber));
        }
        if (contactNumber.Length < 10 || !contactNumber.All(char.IsDigit))
        {
            throw new ArgumentException("Contact number must be at least 10 digits long and contain only numbers.", nameof(contactNumber));
        }
        if (string.IsNullOrWhiteSpace(password) || password.Length != 4 || !int.TryParse(password, out _))
        {
            throw new ArgumentException("Password must be a 4-digit number.", nameof(password));
        }

        FirstName = firstName;
        LastName = lastName;
        ContactNumber = contactNumber;
        Password = password;
        borrowedMovies = new string[MaxBorrowedMovies];
        borrowedCount = 0;
    }

    public bool BorrowMovie(string title, MovieCollection movieCollection)
    {
        if (borrowedCount >= MaxBorrowedMovies)
        {
            return false;
        }

        Movie movie = movieCollection.GetMovieByTitle(title);
        if (movie != null && movie.IsAvailable())
        {
            borrowedMovies[borrowedCount++] = title;
            movie.RemoveCopies(1);
            movie.IncrementBorrowCount();
            return true;
        }

        return false;
    }

    public bool ReturnMovie(string title, MovieCollection movieCollection)
    {
        int index = Array.IndexOf(borrowedMovies, title);
        if (index >= 0)
        {
            for (int i = index; i < borrowedCount - 1; i++)
            {
                borrowedMovies[i] = borrowedMovies[i + 1];
            }

            borrowedMovies[--borrowedCount] = null;

            Movie movie = movieCollection.GetMovieByTitle(title);
            if (movie != null)
            {
                movie.AddCopies(1);
                movie.DecrementBorrowCount();
            }

            return true;
        }

        return false;
    }

    public string[] GetBorrowedMovies()
    {
        string[] movies = new string[borrowedCount];
        Array.Copy(borrowedMovies, movies, borrowedCount);
        return movies;
    }
}

class MemberCollection
{
    private const int InitialCapacity = 16;
    private const double LoadFactor = 0.75;
    private Member[] members;
    private int count;

    public MemberCollection()
    {
        members = new Member[InitialCapacity];
        count = 0;
    }

    public void AddMember(Member member)
    {
        if (member == null)
        {
            throw new ArgumentNullException(nameof(member), "Member cannot be null.");
        }

        if (count >= members.Length * LoadFactor)
        {
            Resize();
        }

        int index = GetMemberIndex(member.FirstName, member.LastName);
        if (index < 0)
        {
            index = GetNextAvailableIndex(member.FirstName, member.LastName);
            members[index] = member;
            count++;
        }
        else
        {
            throw new InvalidOperationException("Member already exists.");
        }
    }

    public void RemoveMember(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));
        }

        int index = GetMemberIndex(firstName, lastName);
        if (index >= 0)
        {
            if (members[index].GetBorrowedMovies().Length == 0)
            {
                RemoveMemberAtIndex(index);
            }
            else
            {
                throw new InvalidOperationException("Cannot remove a member with borrowed movies.");
            }
        }
        else
        {
            throw new InvalidOperationException("Member not found.");
        }
    }

    public Member FindMember(string firstName, string lastName, string password)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));
        }

        int index = GetMemberIndex(firstName, lastName);
        if (index >= 0 && members[index].Password == password)
        {
            return members[index];
        }

        return null;
    }

    public string FindMemberContact(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));
        }

        int index = GetMemberIndex(firstName, lastName);
        if (index >= 0)
        {
            return members[index].ContactNumber;
        }

        return null;
    }

    public Member[] FindMembersRentingMovie(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Movie title cannot be null or empty.", nameof(title));
        }

        Member[] rentingMembers = new Member[count];
        int rentingCount = 0;

        for (int i = 0; i < members.Length; i++)
        {
            if (members[i] != null && Array.IndexOf(members[i].GetBorrowedMovies(), title) >= 0)
            {
                rentingMembers[rentingCount++] = members[i];
            }
        }

        Array.Resize(ref rentingMembers, rentingCount);
        return rentingMembers;
    }

    private int GetMemberIndex(string firstName, string lastName)
    {
        int hash = GetHash(firstName, lastName);
        int index = hash % members.Length;

        while (members[index] != null &&
               (!members[index].FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) ||
                !members[index].LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase)))
        {
            index = (index + 1) % members.Length;
        }

        if (members[index] != null &&
            members[index].FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) &&
            members[index].LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase))
        {
            return index;
        }

        return -1;
    }

    private int GetNextAvailableIndex(string firstName, string lastName)
    {
        int hash = GetHash(firstName, lastName);
        int index = hash % members.Length;

        while (members[index] != null)
        {
            index = (index + 1) % members.Length;
        }

        return index;
    }

    private void RemoveMemberAtIndex(int index)
    {
        members[index] = null;
        count--;

        int nextIndex = (index + 1) % members.Length;
        while (members[nextIndex] != null)
        {
            int hash = GetHash(members[nextIndex].FirstName, members[nextIndex].LastName);
            int targetIndex = hash % members.Length;

            if ((nextIndex < targetIndex && targetIndex <= index) ||
                (targetIndex <= index && index < nextIndex) ||
                (index < nextIndex && nextIndex < targetIndex))
            {
                members[index] = members[nextIndex];
                members[nextIndex] = null;
                index = nextIndex;
            }

            nextIndex = (nextIndex + 1) % members.Length;
        }
    }

    private void Resize()
    {
        int newCapacity = members.Length * 2;
        Member[] newMembers = new Member[newCapacity];

        for (int i = 0; i < members.Length; i++)
        {
            if (members[i] != null)
            {
                int index = GetNextAvailableIndex(members[i].FirstName, members[i].LastName);
                newMembers[index] = members[i];
            }
        }

        members = newMembers;
    }

    private int GetHash(string firstName, string lastName)
    {
        int hash = 17;
        foreach (char c in firstName + lastName)
        {
            hash = hash * 23 + c;
        }
        return Math.Abs(hash);
    }
}
