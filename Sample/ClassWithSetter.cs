namespace Sample
{
    public class ClassWithSetter
    {
        private int a = 0;

        public int A
        {
            set { a = value; }
        }
    }

    public class ClassUsingSetter
    {
        public void Method()
        {
            var classWithSetter = new ClassWithSetter();
            classWithSetter.A = 10;
        }
    }
}