using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using MAZARDIN_Grégoire_TP3_ST2TRD.TP3_Ressources;
using Timer = System.Threading.Timer;

public class TP3
{
    static MovieCollection movieCollection = new MovieCollection();

    public static void Main(string[] args)
    {
        Console.WriteLine("---------------MAZARDIN Grégoire TP3-----------------");
        Console.WriteLine("Enter 1 to check Exercise 1 or 2 to check Exercise 2");
        string firstInput = Console.ReadLine();
        switch (firstInput)
        {
            case "1":
                Console.WriteLine("Enter a number between 1 and 11 to check for the 11 different queries");
                string secondInput = Console.ReadLine();
                switch (secondInput)
                {
                    case "1":
                        Console.WriteLine("Display the oldest movie\n");
                        Console.WriteLine(Query1());
                        break;
                    case "2":
                        Console.WriteLine("Count all movies");
                        Console.WriteLine(Query2());
                        break;
                    case "3":
                        Console.WriteLine("Count all movies with letter 'e' at least once in the title\n");
                        Console.WriteLine(Query3());
                        break;
                    case "4":
                        Console.WriteLine("Count how many times the letter 'f' is in all the titles from this list\n");
                        Console.WriteLine(Query4());
                        break;
                    case "5":
                        Console.WriteLine("Display the film with the highest budget\n");
                        Console.WriteLine(Query5());
                        break;
                    case "6":
                        Console.WriteLine("Display the movie with the lowest box office\n");
                        Console.WriteLine(Query6());
                        break;
                    case "7":
                        Console.WriteLine("Order the movies by reversed alphabetical order and print the first 11\n");
                        IEnumerable<MovieCollection.WaltDisneyMovies> result = Query7();
                        foreach (var value in result)
                        {
                            Console.WriteLine(value.Title);
                            Console.WriteLine("--------------------------------------");
                        }

                        break;
                    case "8":
                        Console.WriteLine("Count all the movies made before 1980\n");
                        Console.WriteLine(Query8());
                        break;
                    case "9":
                        Console.WriteLine(
                            "Display the average running time of movies having a vowel as first letter\n");
                        Console.WriteLine(Query9());
                        break;
                    case "10":
                        Console.WriteLine(
                            "Print all movies with letter 'h' or 'w' in the title, but not letter 'i' or 't'\n");
                        IEnumerable<MovieCollection.WaltDisneyMovies> result1 = Query10();
                        foreach (var value in result1)
                        {
                            Console.WriteLine(value);
                            Console.WriteLine("--------------------------------------");
                        }

                        break;
                    case "11":
                        Console.WriteLine("Calculate the mean of all Budget/Box office of every movie ever\n");
                        Console.WriteLine(Query11());
                        break;
                    default:
                        break;
                }

                break;
            case "2":
                Threading();
                break;
            default:
                Console.WriteLine("Bye!");
                break;
        }
    }

    //-----------------------------------------LINQ QUERIES-------------------------------------
    // Here are all the queries asked in the subject in order one by one
    static MovieCollection.WaltDisneyMovies Query1()
    {
        return movieCollection.Movies.OrderBy(movies => movies.ReleaseDate).FirstOrDefault();
    }

    static int Query2()
    {
        return movieCollection.Movies.Count();
    }

    static int Query3()
    {
        return movieCollection.Movies.Count(movies => movies.Title.Contains('e'));
    }

    static int Query4()
    {
        return movieCollection.Movies.Aggregate(0,
            (acc, movies) => acc + movies.Title.ToCharArray().Count(c => c == 'f'));
    }

    static MovieCollection.WaltDisneyMovies Query5()
    {
        return movieCollection.Movies.OrderByDescending(movies => movies.Budget).FirstOrDefault();
    }

    static MovieCollection.WaltDisneyMovies Query6()
    {
        return movieCollection.Movies.Where(movies => movies.BoxOffice != null)
            .OrderBy(movies => movies.BoxOffice).FirstOrDefault();
    }

    static IEnumerable<MovieCollection.WaltDisneyMovies>? Query7()
    {
        return movieCollection.Movies.OrderByDescending(movies => movies.Title).Take(11);
    }

    static int Query8()
    {
        return movieCollection.Movies.Count(movies => movies.ReleaseDate.Year < 1980);
    }

    static double? Query9()
    {
        return movieCollection.Movies
            .Where(movies => movies.Title.ToUpper().StartsWith('A') || movies.Title.ToUpper().StartsWith('E') ||
                             movies.Title.ToUpper().StartsWith('I') || movies.Title.ToUpper().StartsWith('O') ||
                             movies.Title.ToUpper().StartsWith('U') || movies.Title.ToUpper().StartsWith('Y'))
            .Average(movies => movies.RunningTime);
    }

    static IEnumerable<MovieCollection.WaltDisneyMovies>? Query10()
    {
        return movieCollection.Movies.Where(movies =>
            (movies.Title.ToLower().Contains('h') || movies.Title.ToLower().Contains('w')) &&
            (!movies.Title.ToLower().Contains('i') && !movies.Title.ToLower().Contains('t')));
    }

    static double? Query11()
    {
        return movieCollection.Movies.Average(movies => movies.Budget / movies.BoxOffice);
    }

    //----------------------------------------MULTITHREADING----------------------------------
    // This part is not working, but you will find all the stuff I have been working on.
    // First here, I chose to declare static variables, the Mutex, three timers and counters
    private static Mutex mut = new Mutex();
    private static System.Timers.Timer timer1 = new System.Timers.Timer();
    private static System.Timers.Timer timer2 = new System.Timers.Timer();
    private static System.Timers.Timer timer3 = new System.Timers.Timer();
    private static int cpt1 = 0;
    private static int cpt2 = 0;
    private static int cpt3 = 0;

    // This function is used to declare the three threads and start them 
    static void Threading()
    {
        Thread Thread1 = new Thread(ThreadProc1);
        Thread1.Name = "EmptySpaces";
        Thread1.Start();

        Thread Thread2 = new Thread(ThreadProc2);
        Thread2.Name = "Stars";
        Thread2.Start();

        Thread Thread3 = new Thread(ThreadProc3);
        Thread3.Name = "Circles";
        Thread3.Start();
    }

    // These are the three ThreadProcs. Each of them executes the method associated that we want our Thread to execute
    private static void ThreadProc1()
    {
        EmptySpaces();
        Console.WriteLine("EmptySpaces executed");
    }

    private static void ThreadProc2()
    {
        Stars();
        Console.WriteLine("Stars executed");
    }

    private static void ThreadProc3()
    {
        Circles();
        Console.WriteLine("Circles executed");
    }

    // Here is the main space for methods. We declare two methods for each thread, one for the timer and one for the
    // event that happens at each timer interval.
    private static void EmptySpaces()
    {
        timer1.Elapsed += new ElapsedEventHandler(DisplayTimeEventSpaces);
        timer1.Interval = 50;
        timer1.Enabled = true;
        timer1.Start();
    }

    public static void DisplayTimeEventSpaces(object source, ElapsedEventArgs e)
    {
        cpt1 += 1;
        PrintParameter(" ");
        if (cpt1 == 200)
        {
            timer1.Enabled = false;
        }
    }

    private static void Stars()
    {
        timer2.Elapsed += new ElapsedEventHandler(DisplayTimeEventStars);
        timer2.Interval = 40;
        timer2.Enabled = true;
        timer2.Start();
    }

    public static void DisplayTimeEventStars(object source, ElapsedEventArgs e)
    {
        cpt2 += 1;
        PrintParameter("*");
        if (cpt2 == 275)
        {
            timer2.Enabled = false;
        }
    }

    private static void Circles()
    {
        timer3.Elapsed += new ElapsedEventHandler(DisplayTimeEventCircles);
        timer3.Interval = 20;
        timer3.Enabled = true;
        timer3.Start();
    }

    public static void DisplayTimeEventCircles(object source, ElapsedEventArgs e)
    {
        cpt3 += 1;
        PrintParameter("°");
        if (cpt3 == 450)
        {
            timer3.Enabled = false;
        }
    }

    // This method is the one that is used by the event methods to display the parameter we want to. This is 
    // where we implement our Mutex because we want each thread to be executed one by one and not having two 
    // or more overlapping at the same time
    public static void PrintParameter(string parameter)
    {
        mut.WaitOne();
        Console.Write(parameter);
        mut.ReleaseMutex();
    }
}