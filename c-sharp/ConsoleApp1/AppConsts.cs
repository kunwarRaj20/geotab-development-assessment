namespace JokesGenerator
{
    public class AppConsts
    {
        public const string AppName = "Joke Generator";
        public class ChuckNorrisApiTypes
        {
            public const string EndPoint = "https://api.chucknorris.io/";
            public const string RandomJokesAction = "jokes/random";
            public const string JokesCategoriesAction = "jokes/categories";
            public const string JokesCategory = "?category=";
            public const int JokeRetryCount = 3;
        }

        public class PrivservNameApiTypes
        {
            public const string EndPoint = "https://www.names.privserv.com/api/";
        }

        public class DefaultTypes
        {
            public const string DefaultName = "Chuck Norris";
        }
    }
}
