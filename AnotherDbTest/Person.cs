

namespace AnotherDbTest
{
    class Person
    {        
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int YearOfBirth { get; private set; }

        public Person (int refId, string first, string last, int year)
        {
            Id = refId;
            FirstName = first;
            LastName = last;
            YearOfBirth = year;
        }
        public override string ToString()
            => $"{Id, 5}  {FirstName, -14}{LastName, -10}{YearOfBirth, 6}";
    }
}
 