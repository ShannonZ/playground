namespace TeachNM
{
    using System.Collections.ObjectModel;

    public partial class MainWindow
    {
        private ObservableCollection<student> students;

        public MainWindow()
        {
            InitializeComponent();
            students = new ObservableCollection<student>()
            {
                new student(){Id=1,Name="Cherry",Details="AAAAAAAAAAAA",ShortDescription="BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB",Image="se.png"},
                new student(){Id=1,Name="Cherry1",Details="AAAAAAAAAAAA1",ShortDescription="BBBBB1",Image="se.png"},
                new student(){Id=1,Name="Cherry2",Details="AAAAAAAAAAAA2",ShortDescription="BBBBBBB2",Image="se.png"},
                new student(){Id=1,Name="Cherry3",Details="AAAAAAAAAAAA3",ShortDescription="BBBBBBBBBBBB3",Image="se.png"}

            };
            this.DataContext = students;
        }

        public class student
        {
            private int _Id;
            public int Id
            {
                get { return _Id; }
                set { _Id = value; }
            }
            private string _Name;
            public string Name
            {
                get { return _Name; }
                set { _Name = value; }
            }
            private string _Details;
            public string Details
            {
                get { return _Details; }
                set { _Details = value; }
            }
            private string _ShortDescription;
            public string ShortDescription
            {
                get { return _ShortDescription; }
                set { _ShortDescription = value; }
            }
            private string _Image;
            public string Image
            {
                get { return _Image; }
                set { _Image = value; }

            }

        }
    }
}