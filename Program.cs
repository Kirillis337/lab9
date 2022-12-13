using System;
using System.Collections.Generic;


namespace lab9
{
    class Program
    {
        static void Main()
        {
            //Стратегия (Strategy)
            Computer comp = new Computer(4, "intel core i-7", new WindWork());
            comp.Move();
            comp.Rebut = new Linuxwork();
            comp.Move();
            //Состояние (State)
            Student std = new Student(new MediumStudentState());
            std.Good();
            std.Bad();
            std.Bad();
            std.Bad();
            std.Good();
            std.Good();
            std.Good();
            //Хранитель (Memento)
            Doljnik std2 = new Doljnik();
            std2.Work();
            StudentHistory story = new StudentHistory();

            story.History.Push(std2.SaveState()); // сохраняем состояние зачетки))

            std2.Work();

            std2.RestoreState(story.History.Pop());

            std2.Work();

            Console.ReadLine();
        }
    }
    //Стратегия (Strategy)
    interface IWork
    {
        void Work();
    }

    class WindWork : IWork
    {
        public void Work()
        {
            Console.WriteLine("работает на Windows 7");
        }
    }

    class Linuxwork : IWork
    {
        public void Work()
        {
            Console.WriteLine("работает на Linux");
        }
    }
    class Computer
    {
        protected int ram; // кол-во оперативки
        protected string proc; // модель процессора

        public Computer(int num, string model, IWork mov)
        {
            this.ram = num;
            this.proc = model;
            Rebut = mov;
        }
        public IWork Rebut { private get; set; }
        public void Move()
        {
            Rebut.Work();
        }
    }
    //Состояние (State)
    class Student
    {
        public IStudentState State { get; set; }

        public Student(IStudentState studstate)
        {
            State = studstate;
        }

        public void Good()
        {
            State.Good(this);
        }
        public void Bad()
        {
            State.Bad(this);
        }
    }

    interface IStudentState
    {
        void Good(Student std);
        void Bad(Student std);
    }

    class BadStudentState : IStudentState
    {
        public void Good(Student std)
        {
            Console.WriteLine("Студент начал ходить на пары");
            std.State = new MediumStudentState();
        }

        public void Bad(Student std)
        {
            Console.WriteLine("Студент продолжает прогуливать пары");
        }
    }
    class MediumStudentState : IStudentState
    {
        public void Good(Student std)
        {
            Console.WriteLine("Студент начал закрывать долги");
            std.State = new GoodStudentState();
        }

        public void Bad(Student std)
        {
            Console.WriteLine("Студент начал прогуливать пары");
            std.State = new BadStudentState();
        }
    }
    class GoodStudentState : IStudentState
    {
        public void Good(Student std)
        {
            Console.WriteLine("Студент продолжает закрывать долги");
        }

        public void Bad(Student std)
        {
            Console.WriteLine("студент просто ходит на пары");
            std.State = new MediumStudentState();
        }
    }
    //Хранитель (Memento)
    // Originator
    class Doljnik
    {
        private int debt = 10; // кол-во долгов за прошлые семестры
        private int tails = 5; // кол-во хвостов за этот семестр

        public void Work()
        {
            if (debt > 0)
            {
                debt--;
                Console.WriteLine("Закрываем долг. Осталось {0} долгов", debt);
            }
            else
                Console.WriteLine("Долгов больше нет");
        }
        // сохранение состояния
        public HeroMemento SaveState()
        {
            Console.WriteLine("Сохранение игры. Параметры: {0} долгов, {1} хвостов", debt, tails);
            return new HeroMemento(debt, tails);
        }

        // восстановление состояния
        public void RestoreState(HeroMemento memento)
        {
            this.debt = memento.Debt;
            this.tails = memento.Tails;
            Console.WriteLine("Восстановление игры. Параметры: {0} долгов, {1} хвостов", debt, tails);
        }
    }
    // Memento
    class HeroMemento
    {
        public int Debt { get; private set; }
        public int Tails { get; private set; }

        public HeroMemento(int debt, int tails)
        {
            this.Debt = debt;
            this.Tails = tails;
        }
    }

    // Caretaker
    class StudentHistory
    {
        public Stack<HeroMemento> History { get; private set; }
        public StudentHistory()
        {
            History = new Stack<HeroMemento>();
        }
    }
}
